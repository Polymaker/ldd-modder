using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDDModder.LDD.Palettes
{
    public class PaletteFile
    {
        // Fields...
        private List<Palette> _Palettes;
        private Bag _Info;
        private string _PalettePath;
        private PaletteType _Type;

        public PaletteType Type
        {
            get { return _Type; }
        }

        public string PalettePath
        {
            get { return _PalettePath; }
            internal set { _PalettePath = value; }
        }

        public Bag Info
        {
            get { return _Info; }
        }

        public List<Palette> Palettes
        {
            get { return _Palettes; }
        }

        internal PaletteFile(Bag info, string palettePath, PaletteType type)
        {
            _Palettes = new List<Palette>();
            _Info = info;
            _PalettePath = palettePath;
            _Type = type;
        }

        public PaletteFile(Bag info, Palette palette)
        {
            _Palettes = new List<Palette>();
            _Palettes.Add(palette);
            _Info = info;
            _PalettePath = String.Empty;
            _Type = PaletteType.User;
        }

        public PaletteFile(Bag info, List<Palette> palettes)
        {
            _Palettes = palettes;
            _Info = info;
            _PalettePath = String.Empty;
            _Type = PaletteType.User;
        }

        public PaletteFile(string name, IEnumerable<PaletteItem> items)
        {
            _Info = new Bag(name, items.Any(i => i.Quantity > 0));
            _Palettes = new List<Palette>();
            _Palettes.Add(new Palette(items));
        }

        internal void OverwritedLDD(string fullpath)
        {
            if (Type != PaletteType.LDD)
                return;
            _Type = PaletteType.ExtendedLDD;
            _PalettePath = fullpath;
        }
    }
}
