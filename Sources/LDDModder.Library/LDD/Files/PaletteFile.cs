using LDDModder.LDD.Palettes;
using LDDModder.Utilities;
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

        public void GeneratePaletteNames()
        {
            int paletteNo = 1;
            foreach (var palette in Palettes)
            {
                if (string.IsNullOrEmpty(palette.Name))
                    palette.Name = FileHelper.GetSafeFileName($"{Info.Name}-{paletteNo++}");
            }
        }

        #region Save/Load From Directory

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

        public void SaveToDirectory(string directory, bool createSubDir)
        {
            directory = Path.GetFullPath(directory);
            if (createSubDir)
                directory = Path.Combine(directory, FileHelper.GetSafeFileName(Info.Name));

            Directory.CreateDirectory(directory);
            GeneratePaletteNames();

            Info.Save(Path.Combine(directory, Bag.FileName));
            foreach (var palette in Palettes)
                palette.Save(Path.Combine(directory, palette.Name + ".paxml"));
        }

        #endregion

        #region Save/Load From Lif

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

        public LifFile CreateLif()
        {
            var lif = new LifFile();
            var ms = new MemoryStream();
            Info.Save(ms);
            ms.Position = 0;

            lif.AddFile(ms, Bag.FileName);
            foreach (var palette in Palettes)
            {
                ms = new MemoryStream();
                palette.Save(ms);
                ms.Position = 0;
                lif.AddFile(ms, palette.Name + ".paxml");
            }

            return lif;
        }

        public void SaveAsLif(string filename)
        {
            filename = Path.GetFullPath(filename);
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var lif = CreateLif())
                lif.SaveAs(filename);
        }

        #endregion


    }
}
