using LDDModder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.Files
{
    //TODO: clean-up and maybe convert to static class to expose only an extraction method, as I don't plan to allow modifing or creating lif file
    //(I don't know whether or not it can be done because there is an hash/checksum that I haven't reverse engineered so I've not researched further)
    public class LifFile : IDisposable
    {
        private Dictionary<string, LifEntry> _Entries;
        private BinaryReaderEx Reader;
        private Stream _Stream;

        public ICollection<LifEntry> Entries
        {
            get { return _Entries.Values; }
        }

        public LifEntry this[string fullname]
        {
            get
            {
                return _Entries[fullname];
            }
        }

        public LifEntry this[int index]
        {
            get
            {
                return Entries.ElementAt(index);
            }
        }

        public abstract class LifEntry
        {
            // Fields...
            private string _FullPath;
            private string _Name;
            private LifFile _Source;

            public abstract bool IsDirectory { get; }

            public bool IsFile
            {
                get { return !IsDirectory; }
            }

            internal long FileDataPosition;

            public string Name
            {
                get { return _Name; }
            }

            public string FullPath
            {
                get { return _FullPath; }
            }

            public LifFile Source
            {
                get { return _Source; }
            }

            public LifEntry(LifFile source, string path)
            {
                _FullPath = path;
                _Source = source;
                _Name = path;
                if (_Name.IndexOf("\\") > 0)
                {
                    _Name = _Name.Substring(_Name.LastIndexOf("\\") + 1);
                }

                source._Entries.Add(path, this);
            }
        }

        public class FileEntry : LifEntry
        {
            private int _FileSize;

            public int FileSize
            {
                get { return _FileSize; }
                internal set { _FileSize = value; }
            }

            public override bool IsDirectory
            {
                get { return false; }
            }

            public FileEntry(LifFile source, string path)
                : base(source, path)
            {

            }

            public Stream OpenStream()
            {
                return new StreamPortion(Source._Stream, FileDataPosition + 20, FileSize - 20);
            }

            public void Extract()
            {
                if (!Directory.Exists(Path.GetDirectoryName(FullPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

                using (var fs = File.Create(FullPath))
                {
                    Extract(fs);
                }
            }

            public void Extract(string filename)
            {
                using (var fs = File.Create(filename))
                {
                    Extract(fs);
                }
            }

            public void Extract(Stream stream)
            {
                using (var data = OpenStream())
                {
                    data.CopyTo(stream);
                }
            }

        }

        public class DirectoryEntry : LifEntry
        {
            private uint _EntryCount;

            public uint EntryCount
            {
                get { return _EntryCount; }
                internal set { _EntryCount = value; }
            }

            public override bool IsDirectory
            {
                get { return true; }
            }

            public DirectoryEntry(LifFile source, string path)
                : base(source, path)
            {

            }
        }

        private LifFile(Stream baseStream)
        {
            _Stream = baseStream;
            Reader = new BinaryReaderEx(_Stream, Endians.BigEndian);
            _Entries = new Dictionary<string, LifEntry>();
        }

        public static LifFile Open(string filename)
        {
            return Open(File.OpenRead(filename));
        }

        public static LifFile Open(Stream stream)
        {
            var file = new LifFile(stream);
            file.StartRead();
            return file;
        }

        public void Close()
        {
            if (_Stream != null)
                _Stream.Close();
        }

        private void StartRead()
        {
            _Stream.Seek(72, SeekOrigin.Begin);
            Skip(Reader.ReadUInt32() - 12);
            Skip(20);
            InternalReadEntry(null);
            long dataPos = 64;
            foreach (var entry in Entries)
            {
                entry.FileDataPosition = dataPos;
                if (entry.IsDirectory)
                    dataPos += 20;
                else
                    dataPos += ((FileEntry)entry).FileSize;
            }
        }

        private void InternalReadEntry(LifEntry parent)
        {
            ushort entryType = Reader.ReadUInt16();
            Skip(4);//unknown header + spacing

            var entryName = ReadEntryName();
            var entryPath = parent != null ? Path.Combine(parent.FullPath, entryName) : entryName;

            Skip(4);//(0000 or 0007)

            if (entryType == 1)
            {
                ReadDirectoryEntry(new DirectoryEntry(this, entryPath));
            }
            else if (entryType == 2)
            {
                ReadfileEntry(new FileEntry(this, entryPath));
            }
            else
                throw new NotImplementedException("Entry type");
        }

        private void ReadDirectoryEntry(DirectoryEntry entry)
        {
            Skip(4);
            entry.EntryCount = Reader.ReadUInt32();
            for (int i = 0; i < entry.EntryCount; i++)
            {
                InternalReadEntry(entry);
            }
        }

        private void ReadfileEntry(FileEntry entry)
        {
            entry.FileSize = Reader.ReadInt32();
            Skip(24);//kind of checksum / crc
        }

        private void Skip(long numBytes)
        {
            _Stream.Seek(numBytes, SeekOrigin.Current);
        }

        string ReadEntryName()
        {
            ushort curChar;
            var bytes = new List<byte>();
            while ((curChar = Reader.ReadUInt16()) != 0)
            {
                bytes.AddRange(BitConverter.GetBytes(curChar));
            }
            return Encoding.Unicode.GetString(bytes.ToArray());
        }

        public void Extract(string extractToPath)
        {
            if (!string.IsNullOrEmpty(extractToPath) && !Directory.Exists(extractToPath))
                Directory.CreateDirectory(extractToPath);

            foreach (var fileEntry in Entries.Where(x => !x.IsDirectory))
            {
                var targetPath = Path.Combine(extractToPath, fileEntry.FullPath);
                if (!Directory.Exists(Path.GetDirectoryName(targetPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                ((FileEntry)fileEntry).Extract(targetPath);
            }
        }

        public void Dispose()
        {
            if (_Stream != null)
            {
                _Stream.Dispose();
                _Stream = null;
            }
            if (Reader != null)
            {
                Reader.Dispose();
                Reader = null;
            }
        }
    }
}
