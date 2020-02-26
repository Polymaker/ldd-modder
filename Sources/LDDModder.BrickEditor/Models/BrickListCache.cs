using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LDDModder.BrickEditor.Settings;
using LDDModder.LDD.Files;
using LDDModder.LDD.Parts;
using Newtonsoft.Json;

namespace LDDModder.BrickEditor.Models
{
    public static class BrickListCache
    {
        private const string BRICK_LIST_CACHE_FILENAME = "BrickList.json";

        private static FileSystemWatcher FSW;
        private static HashSet<int> ChangedParts;

        public static BrickListInfo CachedList { get; private set; }

        public static List<BrickInfo> Bricks => CachedList?.Bricks;

        public static CacheSource Source { get; private set; }
        public static string SourcePath { get; private set; }

        public static bool IsCacheDirty { get; private set; }

        public static bool IsCacheEmpty => Bricks == null || Bricks.Count == 0;

        static BrickListCache()
        {
            ChangedParts = new HashSet<int>();
            Source = CacheSource.None;
        }

        public enum CacheSource
        {
            None,
            ExtractedContent,
            LIF
        }

        public static void Initialize()
        {
            DisposeFolderWatcher();

            Source = CacheSource.None;

            LoadCachedList();
            IsCacheDirty = false;

            if (LDD.LDDEnvironment.Current != null)
            {
                string oldSource = CachedList?.SourcePath;
                SourcePath = string.Empty;
                
                if (LDD.LDDEnvironment.Current.IsLifExtracted(LDD.LddLif.DB))
                {
                    Source = CacheSource.ExtractedContent;
                    SourcePath = LDD.LDDEnvironment.Current.GetAppDataSubDir("db\\Primitives");
                }
                else if (LDD.LDDEnvironment.Current.IsLifPresent(LDD.LddLif.DB))
                {
                    Source = CacheSource.LIF;
                    SourcePath = LDD.LDDEnvironment.Current.GetLifFilePath(LDD.LddLif.DB);
                }

                if (SourcePath != oldSource)
                {
                    CachedList = null;
                    IsCacheDirty = true;
                }

                if (CachedList != null)
                {
                    CheckIfCacheIsOutdated();

                }

                //RebuildCache();

                if (Source == CacheSource.ExtractedContent)
                    InitializeFolderWatcher();
            }
        }


        public static void CheckIfCacheIsOutdated()
        {
            if (CachedList == null)
                return;

            CachedList.Bricks.ForEach(x => x.Validated = false);

            if (Source == CacheSource.LIF)
            {
                var lifInfo = new FileInfo(SourcePath);

                if (lifInfo.LastWriteTime > CachedList.LastUpdate ||
                    lifInfo.CreationTime > CachedList.LastUpdate)
                    IsCacheDirty = true;
                else
                    CachedList.Bricks.ForEach(x => x.Validated = true);
            }
            else if (Source == CacheSource.ExtractedContent)
            {
                var primitiveDir = new DirectoryInfo(SourcePath);

                foreach (var fi in primitiveDir.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (GetFilenamePartID(fi.Name, out int partID))
                    {
                        var brickInfo = CachedList.GetBrick(partID);
                        var lastUpdate = brickInfo?.LastUpdate ?? CachedList.LastUpdate;

                        if (brickInfo == null || 
                            fi.CreationTime > lastUpdate ||
                            fi.LastWriteTime > lastUpdate)
                        {
                            if (!ChangedParts.Contains(partID))
                            {
                                ChangedParts.Add(partID);
                                IsCacheDirty = true;
                            }

                            //break;
                        }
                        else if (brickInfo != null)
                            brickInfo.Validated = true;
                    }
                    
                }
            }
        }

        public static bool LoadCachedList()
        {
            string brickListCache = Path.Combine(SettingsManager.AppDataFolder, BRICK_LIST_CACHE_FILENAME);
            
            if (File.Exists(brickListCache))
            {
                try
                {
                    CachedList = JsonConvert.DeserializeObject<BrickListInfo>(
                        File.ReadAllText(brickListCache)
                        );
                    return true;
                }
                catch { }
            }

            return false;
        }

        public static void SaveCachedList()
        {
            if (CachedList != null)
            {
                if (!IsCacheDirty)
                    CachedList.LastUpdate = DateTime.Now;
                File.WriteAllText(Path.Combine(SettingsManager.AppDataFolder, BRICK_LIST_CACHE_FILENAME),
                JsonConvert.SerializeObject(CachedList));
            }
        }

        public static ProgressInfo CurrentProgress { get; private set; }

        public static void RebuildCache(CancellationToken ct)
        {
            //if (FSW != null)
            //    FSW.EnableRaisingEvents = false;


            //if (!force && !IsCacheDirty)
            //    return;
            CurrentProgress = null;

            if (CachedList == null)
            {
                CachedList = new BrickListInfo()
                {
                    SourcePath = SourcePath
                };
            }

            if (ChangedParts.Count > 0)
            {
                DifferencialRebuild(ct);
            }
            else
            {
                RebuildAll(ct);
            }

            CachedList.Bricks.RemoveAll(x => !x.Validated);

            if (!ct.IsCancellationRequested)
                IsCacheDirty = false;

            SaveCachedList();
        }

        #region Building

        public class ProgressInfo
        {
            public int TotalParts { get; set; }
            public int TotalValidated { get; set; }

            public double Percent => TotalParts > 0 ? (double)TotalValidated / (double)TotalParts : 0d;

            public ProgressInfo(int totalParts, int totalValidated)
            {
                TotalParts = totalParts;
                TotalValidated = totalValidated;
            }
        }

        private static void RebuildAll(CancellationToken ct)
        {
            if (CachedList == null)
            {
                CachedList = new BrickListInfo()
                {
                    SourcePath = SourcePath
                };
            }

            CachedList.Bricks.Clear();

            using (var content = new LifContentWrapper(Source, SourcePath))
            {
                var primitiveFiles = content.GetPrimitiveFiles();
                var partIDs = primitiveFiles.Select(x => x.PartID).Distinct();
                CurrentProgress = new ProgressInfo(partIDs.Count(), 0);

                foreach (int partID in partIDs)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    try
                    {
                        var partInfo = content.GetPart(partID);
                        var brickInfo = new BrickInfo(partInfo)
                        {
                            Validated = true,
                            LastUpdate = DateTime.Now
                        };
                        CachedList.Bricks.Add(brickInfo);
                    }
                    catch { }

                    CurrentProgress.TotalValidated++;
                }
            }
        }

        private static void DifferencialRebuild(CancellationToken ct)
        {
            foreach (var brickInfo in CachedList.Bricks)
            {
                if (ChangedParts.Contains(brickInfo.PartId))
                    brickInfo.Validated = false;
            }

            CurrentProgress = new ProgressInfo(ChangedParts.Count, 0);

            foreach (int partID in ChangedParts)
            {
                if (ct.IsCancellationRequested)
                    break;

                var brickInfo = CachedList.GetBrick(partID);

                PartWrapper partInfo = null;
                try
                {
                    partInfo = PartWrapper.GetPartFromDirectory(SourcePath, partID, false);
                }
                catch { }
                
                if (brickInfo != null && partInfo == null)
                {
                    CachedList.Bricks.Remove(brickInfo);
                    continue;
                }
                else if (partInfo != null)
                {
                    if (brickInfo == null)
                    {
                        brickInfo = new BrickInfo(partInfo)
                        {
                            LastUpdate = DateTime.Now,
                            Validated = true
                        };
                        CachedList.Bricks.Add(brickInfo);
                    }
                    else
                    {
                        brickInfo.UpdateInfo(partInfo);
                        brickInfo.LastUpdate = DateTime.Now;
                        brickInfo.Validated = true;
                    }
                }

                CurrentProgress.TotalValidated++;
            }

            ChangedParts.Clear();
        }

        #endregion

        #region LDD Folder Watcher

        public static void DisposeFolderWatcher()
        {
            if (FSW != null)
            {
                FSW.Changed -= FSW_Changed;
                FSW.Renamed -= FSW_Renamed;
                FSW.Deleted -= FSW_Deleted;
                FSW.EnableRaisingEvents = false;
                FSW.Dispose();
            }
        }

        public static void InitializeFolderWatcher()
        {
            if (FSW == null &&
                LDD.LDDEnvironment.Current != null &&
                LDD.LDDEnvironment.Current.IsLifExtracted(LDD.LddLif.DB))
            {

                FSW = new FileSystemWatcher
                {
                    Path = LDD.LDDEnvironment.Current.GetAppDataSubDir("db\\Primitives"),
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime
                };
                FSW.Changed += FSW_Changed;
                FSW.Renamed += FSW_Renamed;
                FSW.Deleted += FSW_Deleted;
                FSW.EnableRaisingEvents = true;
            }
        }

        private static void FSW_Deleted(object sender, FileSystemEventArgs e)
        {
            if (GetFilenamePartID(e.Name, out int partID) &&
                !ChangedParts.Contains(partID))
            {
                ChangedParts.Add(partID);
                IsCacheDirty = true;
            }
        }

        private static void FSW_Changed(object sender, FileSystemEventArgs e)
        {
            if (GetFilenamePartID(e.Name, out int partID) &&
                !ChangedParts.Contains(partID))
            {
                ChangedParts.Add(partID);
                IsCacheDirty = true;
            }
        }

        private static void FSW_Renamed(object sender, RenamedEventArgs e)
        {
            if (GetFilenamePartID(e.OldName, out int partID) &&
                !ChangedParts.Contains(partID))
            {
                ChangedParts.Add(partID);
                IsCacheDirty = true;
            }

            if (GetFilenamePartID(e.Name, out partID) &&
               !ChangedParts.Contains(partID))
            {
                ChangedParts.Add(partID);
                IsCacheDirty = true;
            }
        }

        #endregion

        #region Utils

        private enum PartFileType
        {
            Primitive,
            Model
        }

        private class PartFileInfo
        {
            public int PartID { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public PartFileType FileType { get; set; }
            public DateTime LastWrite { get; set; }

            public PartFileInfo(string name, string fullName, DateTime lastWrite, PartFileType fileType)
            {
                Name = name;
                FullName = fullName;
                LastWrite = lastWrite;
                if (GetFilenamePartID(name, out int partID))
                    PartID = partID;
                FileType = fileType;
            }

            //public PartFileInfo(int partID, string name, string fullName, DateTime lastWrite)
            //{
            //    PartID = partID;
            //    Name = name;
            //    FullName = fullName;
            //    LastWrite = lastWrite;
            //}
        }

        private class LifContentWrapper : IDisposable
        {
            public CacheSource Source { get; }

            private LifFile Lif;
            private DirectoryInfo PrimitiveDir;

            public LifContentWrapper(CacheSource source, string path)
            {
                Source = source;

                if (source == CacheSource.LIF)
                    Lif = LifFile.Open(path);
                else
                    PrimitiveDir = new DirectoryInfo(path);
            }

            public IEnumerable<PartFileInfo> GetPrimitiveFiles()
            {
                if (PrimitiveDir != null)
                {
                    foreach (var fi in PrimitiveDir.GetFiles("*.xml"))
                        yield return new PartFileInfo(fi.Name, fi.FullName, fi.LastWriteTime, PartFileType.Primitive);
                }
                if (Lif != null)
                {
                    foreach (var fi in Lif.GetFolder("Primitives").Files) //.GetFiles("*.xml")
                        yield return new PartFileInfo(fi.Name, fi.FullName, fi.ModifiedDate, PartFileType.Primitive);
                }
            }

            public IEnumerable<PartFileInfo> GetModelFiles()
            {
                if (PrimitiveDir != null)
                {
                    var meshDir = new DirectoryInfo(Path.Combine(PrimitiveDir.FullName, "LOD0"));
                    foreach (var fi in meshDir.GetFiles())
                        yield return new PartFileInfo(fi.Name, fi.FullName, fi.LastWriteTime, PartFileType.Model);
                }
                if (Lif != null)
                {
                    foreach (var fi in Lif.GetFolder("Primitives\\LOD0").Files)
                        yield return new PartFileInfo(fi.Name, fi.FullName, fi.ModifiedDate, PartFileType.Model);
                }
            }

            public PartWrapper GetPart(int partID)
            {
                if (Lif != null)
                    return PartWrapper.GetPartFromLif(Lif, partID, false);
                else
                    return PartWrapper.GetPartFromDirectory(PrimitiveDir.FullName, partID, false);
            }

            public void Dispose()
            {
                if (Lif != null)
                {
                    Lif.Dispose();
                    Lif = null;
                }
            }
        }

        #endregion

        private static bool GetFilenamePartID(string path, out int partID)
        {
            string filename = Path.GetFileNameWithoutExtension(path);
            return int.TryParse(filename, out partID);
        }
    }
}
