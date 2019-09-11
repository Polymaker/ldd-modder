using LDDModder.LDD.Palettes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Files
{
    public class PaletteFile
    {
        public Bag Info { get; private set; }
        public List<Palette> Palettes { get; private set; }

        public PaletteFile()
        {
            Info = new Bag();
            Palettes = new List<Palette>();
        }

        public PaletteFile(Bag info)
        {
            Info = info;
            Palettes = new List<Palette>();
        }

        public static PaletteFile FromDirectory(string path)
        {
            string bagFilePath = Path.Combine(path, Bag.FileName);
            if (!File.Exists(bagFilePath))
                throw new FileNotFoundException($"File '{bagFilePath} not found.");

            var file = new PaletteFile(Bag.Load(bagFilePath));

            foreach (string filePath in Directory.GetFiles(path, "*.paxml"))
            {
                file.Palettes.Add(Palette.Load(filePath));
            }

            return file;
        }

        public static PaletteFile FromLif(LifFile lif)
        {
            var bagEntry = lif.GetFile(Bag.FileName);
            if (bagEntry == null)
                throw new InvalidDataException($"LIF file does not contains '{Bag.FileName}'");
            var file = new PaletteFile(Bag.Load(bagEntry.GetStream()));

            foreach (var paletteEntry in lif.GetFiles("*.paxml"))
            {
                file.Palettes.Add(Palette.FromLifEntry(paletteEntry));
            }

            return file;
        }

        public static PaletteFile FromLif(string filepath)
        {
            using (var lif = LifFile.Open(filepath))
                return FromLif(lif);
        }

        public void SaveToDirectory(string path)
        {
            path = Path.GetFullPath(path);
            Directory.CreateDirectory(path);

        }
    }
}
