using ICSharpCode.SharpZipLib.Zip;
using LDDModder.LDD.General;
using LDDModder.LDD.Primitives;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace LDDModder.Modding
{
    public class BrickPackage : IDisposable
    {
        private ZipFile ZipStream;
        private StreamDataSource sds;

        private PackageInfo ContentInfo;

        private const string HEADER_FILE = "Content.xml";
        private const string DECORATIONS_DIR = "Decorations\\";
        private const string BRICKS_DIR = "Bricks\\";
        private const string ASSEMBLIES_DIR = "Assemblies\\";
        private const string MODELS_DIR = "Models\\";

        public IList<BrickInfo> Bricks
        {
            get { return ContentInfo.Bricks.AsReadOnly(); }
        }

        public IList<string> Models
        {
            get { return ContentInfo.Models.AsReadOnly(); }
        }

        public IList<int> Decorations
        {
            get { return ContentInfo.Decorations.AsReadOnly(); }
        }

        public IList<DecorationMapping> DecorationMappings
        {
            get { return ContentInfo.Mappings.AsReadOnly(); }
        }

        #region Classes

        public class PackageInfo
        {
            [XmlArrayItem]
            public List<BrickInfo> Bricks { get; set; }

            //public List<int> Assemblies { get; set; }
            [XmlArrayItem(ElementName ="File")]
            public List<string> Models { get; set; }

            [XmlArrayItem]
            public List<int> Decorations { get; set; }

            [XmlArrayItem]
            public List<DecorationMapping> Mappings { get; set; }

            public PackageInfo()
            {
                Bricks = new List<BrickInfo>();
                Models = new List<string>();
                Decorations = new List<int>();
                Mappings = new List<DecorationMapping>();
            }
        }

        public class BrickInfo
        {
            private VersionInfo _Version;

            [XmlAttribute]
            public int Id { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public int VersionMajor
            {
                get { return Version.Major; }
                set { Version.Major = value; }
            }
            [XmlAttribute]
            public int VersionMinor
            {
                get { return Version.Minor; }
                set { Version.Minor = value; }
            }
            
            [XmlIgnore]
            public VersionInfo Version
            {
                get { return _Version; }
            }

            //public List<string> Models { get; set; }

            public BrickInfo()
            {
                Id = 0;
                Name = String.Empty;
                _Version = new VersionInfo();
            }

            public BrickInfo(Primitive brick)
            {
                Id = brick.Id;
                Name = brick.Name;
                _Version = new VersionInfo(brick.Version.Major, brick.Version.Minor);
            }
        }

        #endregion

        public BrickPackage()
        {
            ContentInfo = new PackageInfo();
            sds = new StreamDataSource();
        }

        public BrickPackage(Stream stream)
        {
            ContentInfo = new PackageInfo();
            ZipStream = new ZipFile(stream);
            ZipStream.IsStreamOwner = true;
            sds = new StreamDataSource();
            if (!ZipStream.IsNewArchive)
                Load();
        }

        public static BrickPackage Create(string filepath)
        {
            return new BrickPackage(File.Create(filepath));
        }

        public static BrickPackage OpenOrCreate(string filepath)
        {
            return new BrickPackage(File.Open(filepath, FileMode.OpenOrCreate));
        }

        private void Load()
        {
            var headerEntry = ZipStream.GetEntry(HEADER_FILE);
            var xmlSer = new XmlSerializer(typeof(PackageInfo));
            using (var zs = ZipStream.GetInputStream(headerEntry))
            {
                ContentInfo = (PackageInfo)xmlSer.Deserialize(zs);
            }
        }
        
        #region Writing

        #region Decorations

        public void AddDecoration(int id, Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                AddDecoration(id, ms);
            }
        }

        public void AddDecoration(FileStream stream)
        {
            int decorationId = int.Parse(Path.GetFileNameWithoutExtension(stream.Name));
            AddDecoration(decorationId, stream);
        }

        public void AddDecoration(int id, Stream image)
        {
            if(!ContentInfo.Decorations.Contains(id))
                ContentInfo.Decorations.Add(id);
            WriteEntry(string.Format("{0}{1}.png", DECORATIONS_DIR, id), image);
        }

        public void AddDecorationMapping(int brickId, int decorationId, int surfaceId)
        {
            AddDecorationMapping(new DecorationMapping(decorationId, brickId, surfaceId));
        }

        public void AddDecorationMapping(DecorationMapping mapping)
        {
            if (ContentInfo.Mappings.Any(m => m.DesignID == mapping.DesignID && 
                m.DecorationID == mapping.DecorationID &&
                m.SurfaceID == mapping.SurfaceID))
                return;

            ContentInfo.Mappings.Add(mapping);
        }

        #endregion

        #region Parts

        public void AddBrick(Primitive brick)
        {
            using (var ms = new MemoryStream())
            {
                brick.Save(ms);
                AddBrick(new BrickInfo(brick), ms);
            }
        }

        public void AddBrick(string filepath)
        {
            using (var fs = File.OpenRead(filepath))
                AddBrick(fs);
        }

        public void AddBrick(Stream stream)
        {
            var brick = XSerializable.LoadFrom<Primitive>(stream);
            AddBrick(new BrickInfo(brick), stream);
        }

        public void AddBrick(BrickInfo info, Stream data)
        {
            if (ContentInfo.Bricks.Any(b => b.Id == info.Id))
                ContentInfo.Bricks.Remove(ContentInfo.Bricks.First(b => b.Id == info.Id));
            ContentInfo.Bricks.Add(info);
            WriteEntry(string.Format("{0}{1}.xml", BRICKS_DIR, info.Id), data);
        }

        public void AddModel(int brickId, Stream stream)
        {
            AddModel(brickId, 0, stream);
        }

        public void AddModel(int brickId, int surfaceId, Stream stream)
        {
            string fileName = string.Format("{0}.g{1}", brickId, surfaceId > 0 ? surfaceId.ToString() : string.Empty);
            AddModel(fileName, stream);
        }

        public void AddModel(string filepath)
        {
            using (var fs = File.OpenRead(filepath))
                AddModel(fs);
        }

        public void AddModel(FileStream stream)
        {
            AddModel(Path.GetFileName(stream.Name), stream);
        }

        public void AddModel(string name, Stream stream)
        {
            if (!ContentInfo.Models.Contains(name))
                ContentInfo.Models.Add(name);
            WriteEntry(MODELS_DIR + name, stream);
        }

        #endregion

        private void WriteEntry(string filename, Stream data)
        {
            ZipStream.BeginUpdate();
            sds.SetStream(data);
            ZipStream.Add(sds, filename);
            ZipStream.CommitUpdate();
        }

        private void FinishWriting()
        {
            using (var ms = new MemoryStream())
            {
                XSerializationHelper.SerializeToStream(ContentInfo, ms);
                ms.Seek(0, SeekOrigin.Begin);
                WriteEntry(HEADER_FILE, ms);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeRush", "Class should implement IDisposable")]
        private class StreamDataSource : IStaticDataSource
        {
            private Stream _stream;
            // Implement method from IStaticDataSource
            public Stream GetSource()
            {
                return _stream;
            }

            // Call this to provide the memorystream
            public void SetStream(Stream inputStream)
            {
                _stream = inputStream;
                _stream.Position = 0;
            }
        }

        #endregion


        public Stream GetBrick(BrickInfo bi)
        {
            return GetBrick(bi.Id);
        }

        public Stream GetBrick(int id)
        {
            if (!ContentInfo.Bricks.Any(b => b.Id == id))
                return null;
            var entryName = string.Format("{0}{1}.xml", BRICKS_DIR, id);
            var entry = ZipStream.GetEntry(entryName);
            if (entry == null)
                return null;
            return ZipStream.GetInputStream(entry);
        }

        public Stream GetModel(string modelName)
        {
            if (!ContentInfo.Models.Contains(modelName))
                return null;
            var entryName = string.Format("{0}{1}", MODELS_DIR, modelName);
            var entry = ZipStream.GetEntry(entryName);
            if (entry == null)
                return null;
            return ZipStream.GetInputStream(entry);
        }

        public void Close()
        {
            if (ZipStream != null)
            {
                FinishWriting();
                ZipStream.Close();
                ZipStream = null;
            }
        }

        public void Dispose()
        {
            if (ZipStream != null)
            {
                if (ZipStream.IsUpdating)
                    ZipStream.AbortUpdate();
                ZipStream.Close();
                ZipStream = null;
            }
        }
    }
}
