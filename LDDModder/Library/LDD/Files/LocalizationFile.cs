using LDDModder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.Files
{
    public class LocalizationFile : IDisposable
    {
        private Dictionary<string, string> _Entries;
        private Stream FileStream;

        public string this[string keyName]
        {
            get
            {
                return Entries[keyName];
            }
            set
            {
                if (!Entries.ContainsKey(keyName))
                    Entries.Add(keyName, value);
                else
                    Entries[keyName] = value;
            }
        }

        public Dictionary<string, string> Entries
        {
            get { return _Entries; }
        }

        public LocalizationFile(Stream fileStream)
        {
            FileStream = fileStream;
            _Entries = new Dictionary<string, string>();
            if (fileStream.Length > 0)
                Read();
        }

        #region Static Open

        public static LocalizationFile Open(string filePath, FileMode mode, FileAccess access)
        {
            return new LocalizationFile(File.Open(filePath, mode, access));
        }

        public static LocalizationFile Open(string filePath)
        {
            return Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        public static LocalizationFile Open(string filePath, FileMode mode)
        {
            return Open(filePath, mode, FileAccess.ReadWrite);
        }

        public static LocalizationFile OpenRead(string filePath)
        {
            return Open(filePath, FileMode.Open, FileAccess.Read);
        }

        #endregion

        private void Read()
        {
            _Entries.Clear();
            //var encoding = Encoding.GetEncoding(1252);

            using (var reader = new BinaryReaderEx(FileStream, Encoding.UTF8))
            {

                reader.ReadInt16();//0x32, 0x00
                var sb = new StringBuilder();
                string lastKey = string.Empty;
                while (reader.PeekChar() != -1)
                {
                    var nChar = reader.ReadChar();

                    if ((int)nChar == 0)
                    {
                        if (string.IsNullOrEmpty(lastKey))
                        {
                            lastKey = sb.ToString();
                            sb.Clear();
                        }
                        else
                        {
                            _Entries.Add(lastKey, sb.ToString());
                            sb.Clear();
                            lastKey = string.Empty;
                        }
                    }
                    else
                    {
                        sb.Append(nChar);
                    }
                }
            }
        }

        public void Close()
        {
            if (FileStream != null)
            {
                FileStream.Close();
            }
        }

        public void Dispose()
        {
            if (FileStream != null)
            {
                FileStream.Dispose();
                FileStream = null;
            }
        }

        public void Save()
        {
            
            if (!FileStream.CanWrite)
                return;
            using (var writer = new BinaryWriter(FileStream))
            {
                //var encoding = Encoding.GetEncoding(1252);
                writer.Write((byte)0x32); writer.Write((byte)0x00);
                foreach (var pair in Entries)
                {
                    writer.Write(Encoding.UTF8.GetBytes(pair.Key)); writer.Write((byte)0x00);
                    writer.Write(Encoding.UTF8.GetBytes(pair.Value)); writer.Write((byte)0x00);
                }
            }
        }


        public static string GetLocFilename(LocalizationFileKind file, string languageKey)
        {
            if(file == LocalizationFileKind.AvailableLanguages)
                return Path.Combine(LDDManager.GetLifDirectory(LifInstance.Assets), "Languages.loc");
            else if(file == LocalizationFileKind.ApplicationStrings)
                return Path.Combine(LDDManager.GetLifDirectory(LifInstance.Assets), languageKey, "localizedStrings.loc");
            else// if (file == LocalizationFileKind.MaterialNames)
                return Path.Combine(LDDManager.GetDirectory(LDDManager.DbDirectories.MaterialNames), languageKey, "localizedStrings.loc");
        }
    }
}
