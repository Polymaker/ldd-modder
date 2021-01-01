using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.Modding
{
    public class LinearPattern : ClonePattern
    {
        //public Vector3 Origin { get; set; }
        private Vector3 _Direction;
        private float _Offset;

        public Vector3 Direction
        {
            get => _Direction;
            set => SetPropertyValue(ref _Direction, value);
        }

        public float Offset
        {
            get => _Offset;
            set => SetPropertyValue(ref _Offset, value);
        }

        public override ItemTransform ApplyTransform(ItemTransform transform, int instance)
        {
            var baseTrans = transform.ToMatrixD();
            var translation = Matrix4d.FromTranslation((Vector3d)Direction * (Offset * instance));
            var final = baseTrans * translation;
            return ItemTransform.FromMatrix(final);
        }
    }
}
