using LDDModder.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LDDModder.LDD.Files
{
    public class LifFile : IDisposable
    {
        private Stream BaseStream;

        public FolderEntry RootFolder { get; private set; }

        internal LifFile(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        public IEnumerable<FileEntry> GetAllFiles()
        {
            return RootFolder.GetAllFiles();
        }

        private void ExtractFile(FileEntry entry, Stream target)
        {
            BaseStream.Position = entry.BlockInfo.PositionInStream + LIFFBLOCK_SIZE;
            byte[] buffer = new byte[4096];
            int bytesRead = 0;

            while (bytesRead < entry.FileSize)
            {
                int bytesToRead = Math.Min(buffer.Length, entry.FileSize - bytesRead);
                bytesRead += BaseStream.Read(buffer, 0, bytesToRead);
                target.Write(buffer, 0, bytesToRead);
            }
        }

        private void ExtractFile(FileEntry entry, string destinationPath)
        {
            using (var fs = File.Open(destinationPath, FileMode.Create))
                ExtractFile(entry, fs);

            File.SetCreationTime(destinationPath, entry.CreatedDate);
            File.SetLastWriteTime(destinationPath, entry.ModifiedDate);
        }

        private void ExtractFolder(FolderEntry folder, string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            foreach (var file in folder.Files)
                ExtractFile(file, Path.Combine(destinationPath, file.Name));

            foreach (var subFolder in folder.Folders)
                ExtractFolder(subFolder, Path.Combine(destinationPath, subFolder.Name));
        }

        public void ExtractTo(string directoryName)
        {
            ExtractFolder(RootFolder, directoryName);
        }

        #region LIF READING

        public static LifFile Open(string filename)
        {
            return Read(File.OpenRead(filename));
        }

        public static LifFile Read(Stream stream)
        {
            using (var br = new BinaryReaderEx(stream, Encoding.UTF8, true))
            {
                br.DefaultEndian = Endianness.BigEndian;

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

                stream.Skip(contentBlock.BlockSize - LIFFBLOCK_SIZE);

                var rootFolder = ReadBlockHierarchy(br);
                return ReadEntryHierarchy(br, rootFolder);

            }
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

        private static LifFile ReadEntryHierarchy(BinaryReaderEx br, BlockHierarchy rootBlock)
        {
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


            var lifFile = new LifFile(br.BaseStream);
            var rootFolder = new FolderEntry(lifFile, rootBlock, rootFolderEntry);

            ReadFolderFileHierarchy(br, rootFolder);
            lifFile.RootFolder = rootFolder;

            return lifFile;
        }

        private static void ReadFolderFileHierarchy(BinaryReaderEx br, FolderEntry parentFolder)
        {
            if (parentFolder.BlockInfo.ChildCount != parentFolder.EntryInfo.EntryCount)
                throw new IOException("The file is not a valid LIF. Entry count mismatch.");

            for (int i = 0; i < parentFolder.EntryInfo.EntryCount; i++)
            {
                var expectedBlock = parentFolder.BlockInfo.Childs[i];
                short entryType = br.ReadInt16();
                br.BaseStream.Skip(-2);

                LifEntry entry;
                if (entryType == 1)
                {
                    var folderInfo = br.ReadStruct<LIFFFolderEntry>();
                    entry = new FolderEntry(parentFolder, expectedBlock, folderInfo);
                    ReadFolderFileHierarchy(br, (FolderEntry)entry);
                }
                else if (entryType == 2)
                {
                    var fileInfo = br.ReadStruct<LIFFFileEntry>();
                    entry = new FileEntry(parentFolder, expectedBlock, fileInfo);
                }
                else
                    throw new IOException("The file is not a valid LIF.");

                parentFolder.Entries.Add(entry);
            }
        }

        #endregion

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="folder"></param>
        public static void CreateLif(DirectoryInfo folder)
        {
            var header = new LIFFHeader
            {
                Header = "LIFF",
                Reserved2 = 1
            };
            var rootBlock = new LIFFBlock
            {
                BlockHeader = 1,
                BlockType = 1,
            };
            var contentBlock = new LIFFBlock
            {
                BlockHeader = 1,
                BlockType = 2,
                Spacing2 = 1,
                BlockSize = 26
            };

            var rootFolder = new LIFFBlock
            {
                BlockHeader = 1,
                BlockType = 3,
            };
            var blockList = new List<Tuple<FileSystemInfo, LIFFBlock>>();

            foreach (var entryInfo in folder.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
            {
                if (entryInfo is DirectoryInfo directory)
                {
                    var folderBlock = new LIFFBlock
                    {
                        BlockHeader = 1,
                        BlockType = 3,
                        BlockSize = LIFFBLOCK_SIZE 
                    };
                    blockList.Add(new Tuple<FileSystemInfo, LIFFBlock>(entryInfo, folderBlock));
                }
                else if(entryInfo is FileInfo file)
                {
                    var fileBlock = new LIFFBlock
                    {
                        BlockHeader = 1,
                        BlockType = 4,
                        BlockSize = LIFFBLOCK_SIZE + (int)file.Length
                    };
                    blockList.Add(new Tuple<FileSystemInfo, LIFFBlock>(entryInfo, fileBlock));
                }
            }

            foreach (var folderTuple in blockList.Where(x => x.Item1 is DirectoryInfo))
            {
                var directory = folderTuple.Item1 as DirectoryInfo;
                var fileBlocks = blockList.Where(x => x.Item1 is FileInfo fi && fi.Directory == directory).Select(x => x.Item2);
                
                //folderTuple.Item2.BlockSize = LIFFBLOCK_SIZE + fileBlocks.Sum(x => x.BlockSize);
            }
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
            [Encoding(CharSet.Unicode)]
            public string Filename;
            public int Spacing1;
            public int Reserved2; //14
            public int EntryCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LIFFFileEntry
        {
            public short EntryType;
            public int Reserved1; //0 or 7
            [Encoding(CharSet.Unicode)]
            public string Filename;
            public int Spacing1;
            //public int Reserved2; //0
            public int FileSize;
            public long Created;
            public long Modified;
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

        #endregion

        #region Public types

        public abstract class LifEntry
        {
            internal BlockHierarchy BlockInfo;
            public LifFile Lif { get; }
            public FolderEntry Parent { get; }

            public abstract string Name { get; }

            public string Fullname => !string.IsNullOrEmpty(Parent?.Name) ? Path.Combine(Parent.Fullname, Name) : (Name ?? string.Empty);

            internal LifEntry(LifFile lif, BlockHierarchy blockInfo)
            {
                Lif = lif;
                BlockInfo = blockInfo;
            }

            internal LifEntry(FolderEntry parent, BlockHierarchy blockInfo) : this(parent.Lif, blockInfo)
            {
                Parent = parent;
            }

            public override string ToString()
            {
                return Parent == null ? "root" : Name;
            }
        }

        public class FolderEntry : LifEntry
        {
            internal LIFFFolderEntry EntryInfo;

            public override string Name => EntryInfo.Filename;

            public int ContentSize => BlockInfo.Block.BlockSize - LIFFBLOCK_SIZE;

            internal List<LifEntry> Entries { get; }

            public IEnumerable<FolderEntry> Folders => Entries.OfType<FolderEntry>();
            public IEnumerable<FileEntry> Files => Entries.OfType<FileEntry>();

            internal FolderEntry(LifFile lif, BlockHierarchy blockInfo, LIFFFolderEntry entryInfo) : base(lif, blockInfo)
            {
                EntryInfo = entryInfo;
                Entries = new List<LifEntry>();
            }

            internal FolderEntry(FolderEntry parent, BlockHierarchy blockInfo, LIFFFolderEntry entryInfo) : base(parent, blockInfo)
            {
                BlockInfo = blockInfo;
                EntryInfo = entryInfo;
                Entries = new List<LifEntry>();
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
        }

        public sealed class FileEntry : LifEntry
        {
            internal LIFFFileEntry EntryInfo;

            public override string Name => EntryInfo.Filename;

            public int FileSize => EntryInfo.FileSize;

            public DateTime CreatedDate { get; }

            public DateTime ModifiedDate { get; }

            public DateTime ModifiedDate2 { get; }

            internal FileEntry(FolderEntry folder, BlockHierarchy blockInfo, LIFFFileEntry entryInfo) : base(folder, blockInfo)
            {
                EntryInfo = entryInfo;
                CreatedDate = DateTime.FromFileTime(entryInfo.Created);
                ModifiedDate = DateTime.FromFileTime(entryInfo.Modified);
                ModifiedDate2 = DateTime.FromFileTime(entryInfo.Accessed);
            }

            public void ExtractTo(Stream targetStream)
            {
                Lif.ExtractFile(this, targetStream);
            }

            public void ExtractTo(string filename)
            {
                using (var fs = File.Open(filename, FileMode.Create))
                    ExtractTo(fs);
            }

            public void ExtractToDirectory(string directory)
            {
                ExtractTo(Path.Combine(directory, Name));
            }
        }

        #endregion

    }
}
