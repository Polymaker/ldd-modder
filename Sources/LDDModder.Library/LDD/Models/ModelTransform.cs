using LDDModder.Simple3D;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.LDD.Models
{
    public struct ModelTransform
    {
        public Vector3d Translation { get; set; }
        public Matrix3d Rotation { get; set; }

        public static readonly ModelTransform Identity = new ModelTransform(Vector3d.Zero, Matrix3d.Identity);

        public ModelTransform(Vector3d translation, Matrix3d rotation)
        {
            Translation = translation;
            Rotation = rotation;
        }

        public static ModelTransform Parse(string values)
        {
            var matValues = values.Split(',');
            var rotationMatrix = new Matrix3d();
            var translation = new Vector3d();
            if (matValues.Length == 12)
            {
                for (int i = 0; i < 9; i++)
                    rotationMatrix[i] = double.Parse(matValues[i].Trim(), CultureInfo.InvariantCulture);
                for (int i = 0; i < 3; i++)
                    translation[i] = double.Parse(matValues[9 + i].Trim(), CultureInfo.InvariantCulture);
            }

            return new ModelTransform(translation, rotationMatrix);
        }

        public string SerializeToString()
        {
            var values = new double[12];
            for (int i = 0; i < 9; i++)
                values[i] = Rotation[i];
            for (int i = 0; i < 3; i++)
                values[9 + i] = Translation[i];
            return string.Join(",", values.Select(v => v.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
