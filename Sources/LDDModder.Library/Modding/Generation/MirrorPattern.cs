using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LDDModder.Simple3D;

namespace LDDModder.Modding
{
    public class MirrorPattern : ClonePattern
    {
        public override ClonePatternType Type => ClonePatternType.Mirror;
        public override ItemTransform ApplyTransform(ItemTransform transform, int instance)
        {
            throw new NotImplementedException();
        }

        public override Matrix4d GetPatternMatrix()
        {
            throw new NotImplementedException();
        }
    }
}
