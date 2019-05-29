using System.Globalization;
using System.Xml.Linq;
using LDDModder.Serialization;
using LDDModder.Simple3D;

namespace LDDModder.LDD.Primitives
{
    public class PhysicsAttributes : IXmlObject
    {
        public Matrix3 InertiaTensor { get; set; }

        public Vector3 CenterOfMass { get; set; }

        public float Mass { get; set; }

        public int FrictionType { get; set; }

        public void LoadFromXml(XElement element)
        {
            var inertiaMatrix = new Matrix3();

            var inertiaTensor = element.Attribute("inertiaTensor")?.Value ?? string.Empty;
            var matValues = inertiaTensor.Split(',');
            if (matValues.Length == 9)
            {
                for (int i = 0; i < 9; i++)
                    inertiaMatrix[i] = float.Parse(matValues[i].Trim(), CultureInfo.InvariantCulture);
            }
            InertiaTensor = inertiaMatrix;

            var centerOfMass = element.Attribute("centerOfMass")?.Value ?? string.Empty;
            var centerValues = centerOfMass.Split(',');
            if (centerValues.Length == 3)
            {
                CenterOfMass = new Vector3(
                    float.Parse(centerValues[0].Trim(), CultureInfo.InvariantCulture),
                    float.Parse(centerValues[1].Trim(), CultureInfo.InvariantCulture),
                    float.Parse(centerValues[2].Trim(), CultureInfo.InvariantCulture));
            }

            Mass = element.ReadAttribute<float>("mass");
            FrictionType = element.ReadAttribute<int>("frictionType");
        }

        public XElement SerializeToXml()
        {
            var elem = new XElement("PhysicsAttributes");

            elem.Add(new XAttribute("inertiaTensor", 
                string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4},{5},{6},{7},{8}", 
                InertiaTensor.M11, InertiaTensor.M12, InertiaTensor.M13,
                InertiaTensor.M21, InertiaTensor.M22, InertiaTensor.M23,
                InertiaTensor.M31, InertiaTensor.M32, InertiaTensor.M33)));

            elem.Add(new XAttribute("centerOfMass",
                string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}",
                CenterOfMass.X, CenterOfMass.Y, CenterOfMass.Z)));

            elem.AddNumberAttribute("mass", Mass);
            elem.AddNumberAttribute("frictionType", FrictionType);

            return elem;
        }
    }
}
