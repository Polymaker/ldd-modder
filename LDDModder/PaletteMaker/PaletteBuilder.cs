using LDDModder.LDD.Palettes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.PaletteMaker
{
    class PaletteBuilder
    {

        static PaletteItem GetPaletteItem(int designId, int lddColor, string elementId, int quantity)
        {
            /*
             * if elementId exists in base LDD palette
             *      return LDD palette items[elementId]
             *      
             * else if designId exists in base LDD palette
             * 
             *      if item is simple brick with no decorations and submaterials
             *          return new palette item(designId, lddColor, elementId, quantity)
             *      else
             *          get info from Rebrickable
             *          if decorations, pick default or most recurrent
             *          if submaterials, check if related to color, if not: good, else must ask user input
             *          if item is assembly, get sub-parts info from Rebrickable
             *              get sub-part designId, color (quantity of subpart is not supported by ldd, that also mean that all part with this designId will have the same color)
             *              repeat process (to fill material and/or decorations and/or sub-materials
             * else if designId exists in LDD primitives (db.lif)
             *      get info from Rebrickable
             *      
             * else
             *      return null
            */
            return null;
        }
    }
}
