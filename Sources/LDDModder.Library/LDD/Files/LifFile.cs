using LDDModder.IO;
using LDDModder.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace LDDModder.LDD.Files
{
    public class LifFile : IDisposable
    {
        private Stream BaseStream;

        public FolderEntry RootFolder { get; private set; }

        public string FilePath => BaseStream is FileStream fs ? fs.Name : string.Empty;

        public string Name => !string.IsNullOrEmpty(FilePath) ? Path.GetFileNameWithoutExtension(FilePath) : string.Empty;

        internal LifFile(Stream baseStream)
        {
            BaseStream = baseStream;
            RootFolder = new FolderEntry(this);
        }

        public LifFile()
        {
            RootFolder = new FolderEntry(this);
        }

        public IEnumerable<LifEntry> GetEntryHierarchy(bool drillDown = true)
        {
            yield return RootFolder;

            foreach (var entry in RootFolder.GetEntryHierarchy(drillDown))
                yield return entry;
        }

        #region Folder Methods

        public FolderEntry GetFolder(string folderName)
        {
            return RootFolder.GetFolder(folderName);
        }

        public FolderEntry CreateFolder(string folderName)
        {
            return RootFolder.CreateFolder(folderName);
        }

        #endregion

        #region File Methods

        public IEnumerable<FileEntry> GetAllFiles()
        {
            return RootFolder.GetAllFiles();
        }

        public FileEntry GetFile(string fileName)
        {
            return RootFolder.GetFile(fileName);
        }

        public IEnumerable<FileEntry> GetFiles(string searchFilter)
        {
            return RootFolder.GetFiles(searchFilter);
        }

        public FileEntry AddFile(FileStream fileStream)
        {
            return RootFolder.AddFile(fileStream, Path.GetFileName(fileStream.Name));
        }

        public FileEntry AddFile(Stream fileStream, string fileName)
        {
            return RootFolder.AddFile(fileStream, fileName);
        }

        public FileEntry AddFile(FileInfo fileInfo)
        {
            return RootFolder.AddFile(fileInfo);
        }

        public FileEntry AddFile(FileInfo fileInfo, string fileName)
        {
            return RootFolder.AddFile(fileInfo, fileName);
        }

        public FileEntry AddFile(string filePath)
        {
            return RootFolder.AddFile(filePath);
        }

        public FileEntry AddFile(string filePath, string fileName)
        {
            return RootFolder.AddFile(filePath, fileName);
        }

        #endregion

        #region Extraction Methods

        private void WriteFileToStream(FileEntry entry, Stream target) => WriteFileToStream(entry, target, CancellationToken.None);

        private void WriteFileToStream(FileEntry entry, Stream target, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            var dataStream = entry.GetStream();
            dataStream.Seek(0, SeekOrigin.Begin);

            while (bytesRead < dataStream.Length)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                int bytesToRead = Math.Min(buffer.Length, (int)(dataStream.Length - bytesRead));
                bytesRead += dataStream.Read(buffer, 0, bytesToRead);
                target.Write(buffer, 0, bytesToRead);

                if (bytesRead == 0)
                {
                    if (bytesToRead > 0)
                        throw new EndOfStreamException();
                    break;
                }
            }

        }

        private void ExtractFile(FileEntry entry, string destinationPath, CancellationToken cancellationToken)
        {
            using (var fs = File.Open(destinationPath, FileMode.Create))
                WriteFileToStream(entry, fs, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return;

            File.SetCreationTime(destinationPath, entry.CreatedDate);
            File.SetLastWriteTime(destinationPath, entry.ModifiedDate);
        }

        private void ExtractFolder(FolderEntry folder, string destinationPath, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            foreach (var file in folder.Files)
            {
                ExtractFile(file, Path.Combine(destinationPath, file.Name), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
            }

            foreach (var subFolder in folder.Folders)
            {
                ExtractFolder(subFolder, Path.Combine(destinationPath, subFolder.Name), cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
            }
        }

        public void ExtractTo(string directoryName)
        {
            ExtractFolder(RootFolder, directoryName, CancellationToken.None);
        }

        public void ExtractTo(string directoryName, CancellationToken cancellationToken)
        {
            ExtractFolder(RootFolder, directoryName, cancellationToken);
        }

        #endregion

        #region LIF READING

        public static LifFile Open(string filename)
        {
            return Read(File.OpenRead(filename));
        }

        private void LoadFromStream(Stream stream)
        {
            using (var br = new BinaryReaderEx(stream, Encoding.UTF8, true))
            {
                br.DefaultEndian = Endianness.BigEndian;
                ReadLifHeader(br);

                ClearBaseStream();
                BaseStream = stream;

                ReadEntryHierarchy(this, br);
            }
        }

        private void ClearBaseStream()
        {
            RootFolder.Entries.Clear();
            BaseStream.SafelyDispose();
            BaseStream = null;
        }

        public static LifFile Read(Stream stream)
        {
            using (var br = new BinaryReaderEx(stream, Encoding.UTF8, true))
            {
                br.DefaultEndian = Endianness.BigEndian;

                ReadLifHeader(br);
                var lifFile = new LifFile(stream);
                ReadEntryHierarchy(lifFile, br);
                return lifFile;
            }
        }

        private static void ReadLifHeader(BinaryReaderEx br)
        {
            var header = br.ReadStruct<LIFFHeader>();

            if (header.Header != "LIFF")
                throw new IOException("The file is not a valid LIF. Header is not 'LIFF'.");
            if (header.Reserved2 != 1)
                Trace.WriteLine($"Unexpected value {header.Reserved2}");

            var rootBlock = br.ReadStruct<LIFFBlock>();
            if (rootBlock.BlockType != 1)
                throw new IOException("The file is not a valid LIF. Root block type is not '1'.");

            var contentBlock = br.ReadStruct<LIFFBlock>();
            if (contentBlock.BlockType != 2)
                throw new IOException("The file is not a valid LIF. Content block type is not '2'.");

            br.BaseStream.Skip(contentBlock.BlockSize - LIFFBLOCK_SIZE);
        }

        private static BlockHierarchy ReadBlockHierarchy(BinaryReaderEx br)
        {
            long currentPosition = br.BaseStream.Position;
            var rootFolderBlock = br.ReadStruct<LIFFBlock>();
            if (rootFolderBlock.BlockType != 3)
                throw new IOException("The file is not a valid LIF. Root folder block type is not '3'.");

            var rootFolder = new BlockHierarchy(rootFolderBlock, currentPosition);
            ReadFolderBlocks(br, rootFolder);
            return rootFolder;
        }

        private static void ReadFolderBlocks(BinaryReaderEx br, BlockHierarchy folderHierarchy)
        {
            int remainingSize = folderHierarchy.Block.BlockSize - LIFFBLOCK_SIZE;
            //if (folderHierarchy.Block.Spacing1 != 0)
            //    Trace.WriteLine($"Unexpected folder block spacing 1 '{folderHierarchy.Block.Spacing1}'");
            //if (folderHierarchy.Block.Spacing2 != 0)
            //    Trace.WriteLine($"Unexpected folder block spacing 2 '{folderHierarchy.Block.Spacing2}'");
            //if (folderHierarchy.Block.Spacing3 != 0)
            //    Trace.WriteLine($"Unexpected folder block spacing 3 '{folderHierarchy.Block.Spacing3}'");

            while (remainingSize > 0)
            {
                long currentPosition = br.BaseStream.Position;

                var childBlock = br.ReadStruct<LIFFBlock>();
                remainingSize -= childBlock.BlockSize;

                if (childBlock.BlockType < 3 || childBlock.BlockType > 4)
                    throw new IOException($"Unexpected block type '{childBlock.BlockType}'.");

                var block = new BlockHierarchy(childBlock, currentPosition);
                folderHierarchy.Childs.Add(block);

                if (childBlock.BlockType == 3)
                    ReadFolderBlocks(br, block);
                else if (childBlock.BlockType == 4)
                {
                    //if (childBlock.Spacing1 != 0)
                    //    Trace.WriteLine($"Unexpected file block spacing 1 '{childBlock.Spacing1}'");
                    //if (childBlock.Spacing2 != 1)
                    //    Trace.WriteLine($"Unexpected file block spacing 2 '{childBlock.Spacing2}'");
                    //if (childBlock.Spacing3 != 0)
                    //    Trace.WriteLine($"Unexpected file block spacing 3 '{childBlock.Spacing3}'");
                    br.BaseStream.Skip(childBlock.BlockSize - LIFFBLOCK_SIZE);//skip over file content
                }
            }
        }

        private static void ReadEntryHierarchy(LifFile lifFile, BinaryReaderEx br)
        {
            var rootBlock = ReadBlockHierarchy(br);

            var hierarchyBlock = br.ReadStruct<LIFFBlock>();
            if (hierarchyBlock.BlockType != 5)
                throw new IOException("The file is not a valid LIF. Hierarchy block type is not '5'.");

            //if (hierarchyBlock.Spacing1 != 0)
            //    Trace.WriteLine($"Unexpected hierarchy block spacing 1 '{hierarchyBlock.Spacing1}'");
            //if (hierarchyBlock.Spacing2 != 1)
            //    Trace.WriteLine($"Unexpected hierarchy block spacing 2 '{hierarchyBlock.Spacing2}'");
            //if (hierarchyBlock.Spacing3 != 0)
            //    Trace.WriteLine($"Unexpected hierarchy block spacing 3 '{hierarchyBlock.Spacing3}'");

            var rootFolderEntry = br.ReadStruct<LIFFFolderEntry>();
            if (rootFolderEntry.EntryType != 1)
                throw new IOException("The file is not a valid LIF.");

            PopulateFolderFileHierarchy(br, lifFile.RootFolder, rootBlock, rootFolderEntry);
        }

        private static void PopulateFolderFileHierarchy(BinaryReaderEx br, FolderEntry parentFolder, BlockHierarchy hierarchy, LIFFFolderEntry folderEntry)
        {
            if (hierarchy.ChildCount != folderEntry.EntryCount)
                throw new IOException("The file is not a valid LIF. Entry count mismatch.");

            for (int i = 0; i < folderEntry.EntryCount; i++)
            {
                var expectedBlock = hierarchy.Childs[i];
                short entryType = br.ReadInt16();
                br.BaseStream.Skip(-2);

                LifEntry entry;
                if (entryType == 1)
                {
                    var folderInfo = br.ReadStruct<LIFFFolderEntry>();
                    entry = new FolderEntry(folderInfo.Filename);
                    PopulateFolderFileHierarchy(br, (FolderEntry)entry, expectedBlock, folderInfo);
                }
                else if (entryType == 2)
                {
                    var fileInfo = br.ReadStruct<LIFFFileEntry>();
                    var dataStream = new StreamPortion(br.BaseStream, expectedBlock.PositionInStream + LIFFBLOCK_SIZE, fileInfo.FileSize - LIFFBLOCK_SIZE);
                    entry = new FileEntry(dataStream, fileInfo.Filename);
                    ((FileEntry)entry).SetFileAttributes(
                        DateTime.FromFileTime(fileInfo.Created), 
                        DateTime.FromFileTime(fileInfo.Modified), 
                        DateTime.FromFileTime(fileInfo.Accessed));
                }
                else
                    throw new IOException("The file is not a valid LIF.");

                parentFolder.Entries.Add(entry);
            }
        }

        #endregion

		#region LIF WRITING
		
        public void Save()
        {
            if (BaseStream != null)
            {
                var tmpPath = Path.GetTempFileName();
                SaveAs(tmpPath);

                if (BaseStream is FileStream baseFs)
                {
                    string origPath = baseFs.Name;
                    BaseStream.SafelyDispose();
                    File.Delete(origPath);
                    File.Move(tmpPath, origPath);
                    LoadFromStream(File.OpenRead(origPath));
                }
                else
                {
                    ClearBaseStream();

                    var ms = new MemoryStream();
                    using (var fs = File.Open(tmpPath, FileMode.Create))
                        fs.CopyTo(ms);

                    LoadFromStream(ms);
                }
                File.Delete(tmpPath);
            }
            //else
            //    throw new InvalidOperationException("");
        }

        public void SaveAs(string filename)
        {
            filename = Path.GetFullPath(filename);
            if (BaseStream is FileStream baseFs && baseFs.Name == filename)
                throw new InvalidOperationException("Cannot overwrite the same LIF file.");

            using (var fs = File.Open(filename, FileMode.Create))
                WriteToStream(fs);
        }

		public void WriteToStream(Stream stream)
		{
            if (stream == BaseStream)
                throw new InvalidOperationException("Cannot overwrite the same stream.");

            using (var bw = new BinaryWriterEx(stream, true))
            {
                bw.DefaultEndian = Endianness.BigEndian;
                var header = LIFFHeader.Default;
                bw.WriteStruct(header);
                long rootBlockPos = stream.Position;
                var rootBlock = new LIFFBlock
                {
                    BlockHeader = 1,
                    BlockType = 1,
                };
                bw.WriteStruct(rootBlock);
                var contentBlock = new LIFFBlock
                {
                    BlockHeader = 1,
                    BlockType = 2,
                    Spacing2 = 1,
                    BlockSize = 26
                };
                bw.WriteStruct(contentBlock);
                bw.WriteInt16(1);
                bw.WriteInt32(0);

                WriteFolderBlocks(bw, RootFolder);

                long hierarchyBlockPos = stream.Position;
                var hierarchyBlock = new LIFFBlock
                {
                    BlockHeader = 1,
                    BlockType = 5,
                    Spacing2 = 1
                };
                bw.WriteStruct(hierarchyBlock);

                WriteFolderEntries(bw, RootFolder);
                hierarchyBlock.BlockSize = (int)(stream.Position - hierarchyBlockPos);
                stream.Position = hierarchyBlockPos;
                bw.WriteStruct(hierarchyBlock);

                rootBlock.BlockSize = (int)(stream.Length - rootBlockPos);
                stream.Position = rootBlockPos;
                bw.WriteStruct(rootBlock);

                header.FileSize = (int)stream.Length;
                stream.Position = 0;
                bw.WriteStruct(header);
            }
		}

        private void WriteFolderBlocks(BinaryWriterEx bw, FolderEntry folder)
        {
            var allChilds = folder.GetEntryHierarchy().ToList();
            var totalFilesSize = allChilds.OfType<FileEntry>().Sum(x => x.FileSize);
            var folderBlock = new LIFFBlock
            {
                BlockHeader = 1,
                BlockType = 3,
                BlockSize = ((allChilds.Count() + 1) * LIFFBLOCK_SIZE) + (int)totalFilesSize
            };
            bw.WriteStruct(folderBlock);

            foreach (var entry in folder.Entries.OrderBy(x => x.Name))
            {
                if (entry is FileEntry file)
                {
                    var fileBlock = new LIFFBlock
                    {
                        BlockHeader = 1,
                        BlockType = 4,
                        Spacing2 = 1,
                        BlockSize = LIFFBLOCK_SIZE + (int)file.FileSize
                    };
                    bw.WriteStruct(fileBlock);
                    var fileStream = file.GetStream();
                    //fileStream.CopyTo(bw.BaseStream, 4096);

                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while (bytesRead < file.FileSize)
                    {
                        int b = fileStream.Read(buffer, 0, buffer.Length);
                        bytesRead += b;
                        bw.Write(buffer, 0, b);
                        if (b == 0)
                            break;
                    }
                }
                else
                {
                    WriteFolderBlocks(bw, (FolderEntry)entry);
                }
            }
        }

        private void WriteFolderEntries(BinaryWriterEx bw, FolderEntry folder)
        {
            var allChilds = folder.GetEntryHierarchy().ToList();

            var folderEntry = new LIFFFolderEntry()
            {
                EntryType = 1,
                Filename = folder.IsRootDirectory ? null : folder.Name,
                EntryCount = folder.Entries.Count,
                Reserved1 = folder.IsRootDirectory ? 0 : 7,
                BlockSize = 20
            };
            bw.WriteStruct(folderEntry);

            foreach (var entry in folder.Entries.OrderBy(x => x.Name))
            {
                if (entry is FileEntry file)
                {
                    var fileEntry = new LIFFFileEntry()
                    {
                        EntryType = 2,
                        Filename = file.Name,
                        FileSize = (int)file.FileSize + LIFFBLOCK_SIZE,
                        Reserved1 = 7,
                        Created = file.CreatedDate.ToFileTime(),
                        Modified = file.ModifiedDate.ToFileTime(),
                        Accessed = file.AccessedDate.ToFileTime()
                    };
                    bw.WriteStruct(fileEntry);
                }
                else
                {
                    WriteFolderEntries(bw, (FolderEntry)entry);
                }
            }
        }

        #endregion

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="folder"></param>
        public static LifFile CreateFromDirectory(DirectoryInfo folder)
        {
            var lif = new LifFile();

            foreach (var entryInfo in folder.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
            {
                if(entryInfo is FileInfo file)
                {
                    string relativeDir = file.DirectoryName.Substring(folder.FullName.Length);
                    var lifFolder = lif.GetFolder(relativeDir);
                    if (lifFolder == null)
                        lifFolder = lif.CreateFolder(relativeDir);
                    lifFolder.AddFile(file);
                }
            }

            return lif;
        }

        public void Dispose()
        {
            if (BaseStream != null)
            {
                BaseStream.Dispose();
                BaseStream = null;
            }

            if (RootFolder != null)
            {
                foreach(var f in GetAllFiles())
                    f.Dispose();
                RootFolder.Entries.Clear();
                RootFolder = null;
            }
        }

        #region Internal types

        [StructLayout(LayoutKind.Sequential)]
        internal struct LIFFHeader
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Header;
            public int Reserved1;
            public int FileSize;
            public short Reserved2;
            public int Reserved3;

            public static LIFFHeader Default => new LIFFHeader
            {
                Header = "LIFF",
                Reserved2 = 1
            };
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LIFFBlock
        {
            public short BlockHeader;
            public short BlockType;
            /// <summary>
            /// Always equals 0
            /// </summary>
            public int Spacing1;
            public int BlockSize;
            /// <summary>
            /// Equals 1 for block types 2,4 and 5
            /// </summary>
            public int Spacing2;
            /// <summary>
            /// Always equals 0
            /// </summary>
            public int Spacing3;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LIFFEntry
        {
            public short EntryType;
            public int Reserved1;
            [Encoding(CharSet.Unicode)]
            public string Filename;
            public int Spacing1;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LIFFFolderEntry
        {
            public short EntryType;
            public int Reserved1; //0 or 7
            [Encoding(CharSet.Unicode), StringMarshaling(StringMarshalingMode.NullTerminated)]
            public string Filename;
            public int Spacing1;
            public int BlockSize; //20
            public int EntryCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LIFFFileEntry
        {
            public short EntryType;
            public int Reserved1; //0 or 7
            [Encoding(CharSet.Unicode), StringMarshaling(StringMarshalingMode.NullTerminated)]
            public string Filename;
            public int Spacing1;
            //public int Reserved2; //0
            public int FileSize;
            public long Modified;
            public long Created;
            public long Accessed;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            //public byte[] Checksum;
        }

        internal static readonly int LIFFBLOCK_SIZE = 20;

        internal class BlockHierarchy
        {
            public LIFFBlock Block { get; }
            public int BlockSize => Block.BlockSize;
            public long PositionInStream { get; }
            public List<BlockHierarchy> Childs { get; }
            public int ChildCount => Childs.Count;

            public BlockHierarchy(LIFFBlock block, long positionInStream)
            {
                Block = block;
                PositionInStream = positionInStream;
                Childs = new List<BlockHierarchy>();
            }
        }

        internal interface ILifEntry
        {
            void SetParent(FolderEntry parent);
        }

        #endregion

        #region Public types

        public sealed class LifEntryCollection : IEnumerable<LifEntry>, ICollection<LifEntry>
        {
            public FolderEntry Owner { get; }

            public int Count => Entries.Count;

            public bool IsReadOnly => ((ICollection<LifEntry>)Entries).IsReadOnly;

            private List<LifEntry> Entries;

            public LifEntryCollection(FolderEntry owner)
            {
                Owner = owner;
                Entries = new List<LifEntry>();
            }

            public IEnumerator<LifEntry> GetEnumerator()
            {
                return ((IEnumerable<LifEntry>)Entries).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<LifEntry>)Entries).GetEnumerator();
            }

            public void Add(LifEntry item)
            {
                if (item.Parent != null)
                    throw new InvalidOperationException("The entry belongs to another folder");
                if (Owner.GetAllFiles().Contains(item))
                    throw new InvalidOperationException("Cannot add a parent entry into a child");

                ((ILifEntry)item).SetParent(Owner);
                Entries.Add(item);
            }

            public void Clear()
            {
                Entries.ForEach(e => ((ILifEntry)e).SetParent(null));
                Entries.Clear();
            }

            public bool Contains(LifEntry item)
            {
                return Entries.Contains(item);
            }

            public void CopyTo(LifEntry[] array, int arrayIndex)
            {
                ((ICollection<LifEntry>)Entries).CopyTo(array, arrayIndex);
            }

            public bool Remove(LifEntry item)
            {
                bool result = Entries.Remove(item);
                if (result)
                    ((ILifEntry)item).SetParent(null);
                return result;
            }
        }

        public abstract class LifEntry : ILifEntry
        {
            public virtual LifFile Lif => Parent?.Lif;
            public FolderEntry Parent { get; private set; }

            public string Name { get; protected set; }

            public string Fullname => !string.IsNullOrEmpty(Parent?.Name) ? Path.Combine(Parent.Fullname, Name) : (Name ?? string.Empty);

            internal LifEntry(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                if (this is FolderEntry fe)
                {
                    if (fe.IsRootDirectory)
                        return "Root folder";
                    return "Folder: " + Name;
                }
                    
                return "File: " + Name;
            }

            void ILifEntry.SetParent(FolderEntry parent)
            {
                Parent = parent;
            }

            public virtual void Rename(string newName)
            {
                ValidateEntryName(newName);
                Name = newName;
            }

            protected virtual void ValidateEntryName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Name cannot be empty");
            }

            public int GetLevel()
            {
                if (Parent != null)
                    return Parent.GetLevel() + 1;
                return 0;
            }

            public void ExtractToDirectory(string directory) => ExtractToDirectory(directory, CancellationToken.None);

            public virtual void ExtractToDirectory(string directory, CancellationToken cancellationToken)
            {
                if (this is FolderEntry folder)
                    Lif.ExtractFolder(folder, Path.Combine(directory, Name), cancellationToken);
                else if (this is FileEntry file)
                    Lif.ExtractFile(file, Path.Combine(directory, Name), cancellationToken);
            }
        }

        public class FolderEntry : LifEntry, IEnumerable<LifEntry>
        {
            private LifFile _Lif;
            public override LifFile Lif => _Lif ?? base.Lif;

            public bool IsRootDirectory { get; }

            public LifEntryCollection Entries { get; }

            public IEnumerable<FolderEntry> Folders => Entries.OfType<FolderEntry>();
            public IEnumerable<FileEntry> Files => Entries.OfType<FileEntry>();

            internal FolderEntry(LifFile lif) : base(string.Empty)
            {
                _Lif = lif;
                Entries = new LifEntryCollection(this);
                IsRootDirectory = true;
            }

            internal FolderEntry(string name) : base(name)
            {
                Entries = new LifEntryCollection(this);
            }

            public FolderEntry GetFolder(string folderName)
            {
                string fullName = Path.Combine(Fullname, folderName);
                return GetEntryHierarchy(false).OfType<FolderEntry>().FirstOrDefault(x => x.Fullname.Equals(fullName, StringComparison.InvariantCultureIgnoreCase));
            }

            public FileEntry GetFile(string filename)
            {
                string fullName = Path.Combine(Fullname, filename);
                return GetEntryHierarchy(false).OfType<FileEntry>().FirstOrDefault(x => x.Fullname.Equals(fullName, StringComparison.InvariantCultureIgnoreCase));
            }

            public IEnumerable<FileEntry> GetFiles(string searchFilter)
            {
                return Files.Where(x => x.Name.MatchesWildcard(searchFilter));
            }

            public IEnumerable<FileEntry> GetAllFiles()
            {
                foreach (var entry in Entries)
                {
                    if (entry is FileEntry file)
                    {
                        yield return file;
                    }
                    else if (entry is FolderEntry folder)
                    {
                        foreach (var childFile in folder.GetAllFiles())
                            yield return childFile;
                    }
                }
            }
			
			public IEnumerable<LifEntry> GetEntryHierarchy(bool drillDown = true)
            {
                foreach (var entry in Entries.OrderBy(x => x.Name))
                {
					yield return entry;
                    if (drillDown && entry is FolderEntry folder)
                    {
                        foreach (var childEntry in folder.GetEntryHierarchy(drillDown))
                            yield return childEntry;
                    }
                }

                if (!drillDown)
                {
                    foreach (var folder in Folders.OrderBy(x => x.Name))
                    {
                        foreach (var childEntry in folder.GetEntryHierarchy(drillDown))
                            yield return childEntry;
                    }
                }
            }

            public override void Rename(string newName)
            {
                if (IsRootDirectory)
                    throw new InvalidOperationException();
                base.Rename(newName);
            }

            public FolderEntry CreateFolder(string folderName)
            {
                ValidateEntryName(folderName);
                string[] subDirs = folderName.Split(Path.PathSeparator);
                var curFolder = this;

                for (int i = 0; i < subDirs.Length; i++)
                {
                    var subFolder = curFolder.GetFolder(subDirs[i]);
                    if (subFolder == null)
                        subFolder = curFolder.CreateFolder(subDirs[i]);
                    curFolder = subFolder;
                }
                return curFolder;
            }
			
/* 			public FolderEntry AddExistingFolder(string folderPath)
            {
				return AddExistingFolder(folderPath, Path.GetDirectoryName(folderPath));
            }
			
			public FolderEntry AddExistingFolder(string folderPath, string folderName)
            {
				if (!Directory.Exists(folderPath))
					throw new FileNotFoundException();
				
                ValidateEntryName(folderName);
                if (Folders.Any(x => x.Name.Equals(folderName, StringComparison.InvariantCultureIgnoreCase)))
                    throw new ArgumentException("Folder already exist");
                var newFolder = new FolderEntry(folderName);
                Entries.Add(newFolder);
				foreach (var filePath in Directory.GetFiles
                return newFolder;
            } */

            public FileEntry AddFile(FileStream fileStream)
            {
                return AddFile(fileStream, Path.GetFileName(fileStream.Name));
            }

            public FileEntry AddFile(Stream fileStream, string filename)
            {
                ValidateEntryName(filename);

                if (Files.Any(x => x.Name.Equals(filename, StringComparison.InvariantCultureIgnoreCase)))
                    throw new ArgumentException("File already exist");

                var newFile = new FileEntry(fileStream, filename);
                Entries.Add(newFile);

                if (fileStream is FileStream fs)
                {
                    newFile.SetFileAttributes(
                        File.GetCreationTime(fs.Name),
                        File.GetLastWriteTime(fs.Name),
                        File.GetLastAccessTime(fs.Name));
                }

                return newFile;
            }

            public FileEntry AddFile(FileInfo fileInfo)
            {
                return AddFile(fileInfo.OpenRead());
            }

            public FileEntry AddFile(FileInfo fileInfo, string fileName)
            {
                return AddFile(fileInfo.OpenRead(), fileName);
            }

            public FileEntry AddFile(string filePath)
            {
                return AddFile(File.OpenRead(filePath));
            }

            public FileEntry AddFile(string filePath, string fileName)
            {
                return AddFile(File.OpenRead(filePath), fileName);
            }

            public IEnumerator<LifEntry> GetEnumerator()
            {
                return Entries.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return Entries.GetEnumerator();
            }

            protected override void ValidateEntryName(string name)
            {
                base.ValidateEntryName(name);
                if (!(FileHelper.IsValidDirectory(name) || Path.IsPathRooted(name)))
                    throw new InvalidOperationException("Name contains invalid character.");
            }
        }

        public sealed class FileEntry : LifEntry, IDisposable
        {
            private Stream DataStream;

            public long FileSize => DataStream.Length;

            public DateTime CreatedDate { get; private set; }

            public DateTime ModifiedDate { get; private set; }

            public DateTime AccessedDate { get; private set; }

            internal FileEntry(Stream dataStream, string filename) : base(filename)
            {
                DataStream = dataStream;
            }
			
			~FileEntry()
			{
				Dispose();
			}

            public void ExtractToStream(Stream targetStream)
            {
                Lif.WriteFileToStream(this, targetStream);
            }

            public void ExtractTo(string filename)
            {
                ExtractTo(filename, CancellationToken.None);
            }

            public void ExtractTo(string filename, CancellationToken cancellationToken)
            {
                Lif.ExtractFile(this, filename, cancellationToken);
            }

            public override void ExtractToDirectory(string directory, CancellationToken cancellationToken)
            {
                Directory.CreateDirectory(directory);
                base.ExtractToDirectory(directory, cancellationToken);
            }

            public Stream GetStream()
            {
                return DataStream;
            }

            public void Dispose()
            {
                if (DataStream != null && !(DataStream is StreamPortion))
                {
                    DataStream.Dispose();
                    DataStream = null;
                }
				GC.SuppressFinalize(this);
            }

            public void SetFileAttributes(DateTime createdDate, DateTime modifiedDate, DateTime accessedDate)
            {
                CreatedDate = createdDate;
                ModifiedDate = modifiedDate;
                AccessedDate = accessedDate;
            }

            protected override void ValidateEntryName(string name)
            {
                base.ValidateEntryName(name);
                if (!FileHelper.IsValidFileName(name))
                    throw new InvalidOperationException("Name contains invalid character.");
            }
        }

        #endregion

    }
}
