using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.LDD.Primitives.Connectors
{
    public interface IConnector
    {
        ConnectorType Type { get; }
        int SubType { get; set; }
        Transform Transform { get; set; }
    }

    public abstract class Connector : IXmlObject, IConnector
    {
        public abstract ConnectorType Type { get; }
        public int SubType { get; set; }
        public Transform Transform { get; set; }

        public Connector()
        {
            Transform = new Transform();
        }

        public virtual XElement SerializeToXml()
        {
            var elem = new XElement(Type.ToString(), new XAttribute("type", SubType));
            SerializeBeforeTransform(elem);
            elem.Add(Transform.ToXmlAttributes());
            SerializeAfterTransform(elem);
            return elem;
        }

        protected virtual void SerializeBeforeTransform(XElement element)
        {

        }

        protected virtual void SerializeAfterTransform(XElement element)
        {

        }

        public virtual void LoadFromXml(XElement element)
        {
            SubType = element.ReadAttribute<int>("type");
            Transform = Transform.FromElementAttributes(element);
        }

        public static Connector DeserializeConnector(XElement element)
        {
            Connector connector = null;
            
            switch ((ConnectorType)Enum.Parse(typeof(ConnectorType), element.Name.LocalName))
            {
                case ConnectorType.Axel:
                    connector = new AxelConnector();
                    break;
                case ConnectorType.Ball:
                    connector = new BallConnector();
                    break;
                case ConnectorType.Custom2DField:
                    connector = new Custom2DFieldConnector();
                    break;
                case ConnectorType.Fixed:
                    connector = new FixedConnector();
                    break;
                case ConnectorType.Gear:
                    connector = new GearConnector();
                    break;
                case ConnectorType.Hinge:
                    connector = new HingeConnector();
                    break;
                case ConnectorType.Rail:
                    connector = new RailConnector();
                    break;
                case ConnectorType.Slider:
                    connector = new SliderConnector();
                    break;
            }

            if (connector != null)
                connector.LoadFromXml(element);

            return connector;
        }

        public static Type GetClassType(ConnectorType type)
        {
            switch (type)
            {
                case ConnectorType.Axel:
                    return typeof(AxelConnector);
                case ConnectorType.Ball:
                    return typeof(BallConnector);
                case ConnectorType.Custom2DField:
                    return typeof(Custom2DFieldConnector);
                case ConnectorType.Fixed:
                    return typeof(FixedConnector);
                case ConnectorType.Gear:
                    return typeof(GearConnector);
                case ConnectorType.Hinge:
                    return typeof(HingeConnector);
                case ConnectorType.Rail:
                    return typeof(RailConnector);
                case ConnectorType.Slider:
                    return typeof(SliderConnector);
            }

            return null;
        }

        public static Connector CreateFromType(ConnectorType type)
        {
            return (Connector)Activator.CreateInstance(GetClassType(type));
        }
    }
}
