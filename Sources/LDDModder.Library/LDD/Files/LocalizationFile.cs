using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files
{
    public class LocalizationFile : Dictionary<string, string>
    {
        public static LocalizationFile Read(string filename)
        {
            using (var fs = File.OpenRead(filename))
                return Read(fs);
        }

        public static LocalizationFile Read(Stream stream)
        {
            var localizations = new LocalizationFile();

            using (var br = new BinaryReader(stream, Encoding.UTF8))
            {
                short header = br.ReadInt16();
                if (header != 50)
                    throw new IOException("The file is not a LDD localization file (*.loc)");

                try
                {
                    while (br.PeekChar() != -1)
                    {
                        string key = br.ReadNullTerminatedString();
                        string value = br.ReadNullTerminatedString();
                        localizations.Add(key, value);
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException("Error reading localization file entries", ex);
                }
            }

            return localizations;
        }

        public void Save(string filename)
        {
            using (var fs = File.Open(filename, FileMode.Create))
                Save(fs);
        }

        public void Save(Stream stream)
        {
            using (var bw = new BinaryWriter(stream, Encoding.UTF8))
            {
                bw.Write((short)50); //header

                foreach (var keyVal in this)
                {
                    bw.Write(Encoding.UTF8.GetBytes(keyVal.Key)); bw.Write((byte)0);
                    bw.Write(Encoding.UTF8.GetBytes(keyVal.Value)); bw.Write((byte)0);
                }
            }
        }
    }
}
