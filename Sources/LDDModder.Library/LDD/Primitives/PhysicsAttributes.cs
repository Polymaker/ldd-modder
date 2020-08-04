using System.Globalization;
using System.Xml.Linq;
using LDDModder.Serialization;
using LDDModder.Simple3D;

namespace LDDModder.LDD.Primitives
{
    public class PhysicsAttributes : IXmlObject
    {
        public Matrix3d InertiaTensor { get; set; }

        public Vector3d CenterOfMass { get; set; }

        public double Mass { get; set; }

        /// <summary>
        /// Always 0 or 1, so it may be a boolean
        /// </summary>
        public int FrictionType { get; set; }

        public bool IsEmpty => 
            InertiaTensor == Matrix3d.Zero && 
            CenterOfMass == Vector3d.Zero &&
            Mass == 0 && FrictionType == 0;

        public PhysicsAttributes()
        {
            InertiaTensor = Matrix3d.Zero;
            CenterOfMass = Vector3d.Zero;
            Mass = 0;
            FrictionType = 0;
        }

        public void LoadFromXml(XElement element)
        {
            var inertiaMatrix = new Matrix3d();

            var inertiaTensor = element.Attribute("inertiaTensor")?.Value ?? string.Empty;
            var matValues = inertiaTensor.Split(',');
            if (matValues.Length == 9)
            {
                for (int i = 0; i < 9; i++)
                    inertiaMatrix[i] = double.Parse(matValues[i].Trim(), CultureInfo.InvariantCulture);
            }
            InertiaTensor = inertiaMatrix;

            var centerOfMass = element.Attribute("centerOfMass")?.Value ?? string.Empty;
            var centerValues = centerOfMass.Split(',');
            if (centerValues.Length == 3)
            {
                CenterOfMass = new Vector3d(
                    double.Parse(centerValues[0].Trim(), CultureInfo.InvariantCulture),
                    double.Parse(centerValues[1].Trim(), CultureInfo.InvariantCulture),
                    double.Parse(centerValues[2].Trim(), CultureInfo.InvariantCulture));
            }

            Mass = element.ReadAttribute("mass", 0d);
            FrictionType = element.ReadAttribute<int>("frictionType");
        }

        public XElement SerializeToXml(string elementName)
        {
            var elem = new XElement(elementName);

            elem.Add(new XAttribute("inertiaTensor", GetInertiaTensorString()));

            elem.Add(new XAttribute("centerOfMass",
                string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}",
                CenterOfMass.X, CenterOfMass.Y, CenterOfMass.Z)));

            elem.AddNumberAttribute("mass", Mass);
            elem.AddNumberAttribute("frictionType", FrictionType);

            return elem;
        }

        public string GetInertiaTensorString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2},{3},{4},{5},{6},{7},{8}",
                InertiaTensor.A1, InertiaTensor.A2, InertiaTensor.A3,
                InertiaTensor.B1, InertiaTensor.B2, InertiaTensor.B3,
                InertiaTensor.C1, InertiaTensor.C2, InertiaTensor.C3);
        }

        public XElement SerializeToXml()
        {
            return SerializeToXml("PhysicsAttributes");
        }

        public PhysicsAttributes Clone()
        {
            return new PhysicsAttributes()
            {
                CenterOfMass = CenterOfMass,
                FrictionType = FrictionType,
                InertiaTensor = InertiaTensor,
                Mass = Mass
            };
        }
    }
}
