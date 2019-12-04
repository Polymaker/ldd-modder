using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using LDDModder.LDD.Primitives;
using LDDModder.LDD.Primitives.Connectors;
using LDDModder.Utilities;

namespace LDDModder.Modding.Editing
{
    [XmlRoot("Connection")]
    public /*abstract*/ class PartConnection : PartElement, IPhysicalElement
    {
        public const string NODE_NAME = "Connection";

        private ItemTransform _Transform;

        public ItemTransform Transform
        {
            get => _Transform;
            set => SetPropertyValue(ref _Transform, value);
        }

        public dynamic ConnectorProxy { get; private set; }
        private bool IsAssigningConnectorProperties;

        [XmlIgnore]
        public Connector Connector
        {
            get => ConnectorProxy/* != null ? _ConnectorProxy : null*/;
            set => SetConnector(value);
        }

        [XmlAttribute]
        public ConnectorType ConnectorType { get; set; }

        public PartConnection()
        {
            Transform = new ItemTransform();
        }

        protected PartConnection(ConnectorType connectorType)
        {
            Transform = new ItemTransform();
            ConnectorType = connectorType;
        }

        public PartConnection(Connector connector)
        {
            ConnectorType = connector.Type;
            SetConnector(connector);
            Transform = ItemTransform.FromLDD(connector.Transform);
        }

        #region Connector Handling

        private void SetConnector(Connector connector)
        {
            if (ConnectorProxy != null)
            {
                (ConnectorProxy as INotifyPropertyValueChanged)
                    .PropertyValueChanged -= PartConnection_PropertyValueChanged;
            }

            if (connector != null)
            {
                ConnectorProxy = new ConnectorProxy(connector);
                (ConnectorProxy as INotifyPropertyValueChanged)
                    .PropertyValueChanged += PartConnection_PropertyValueChanged;
            }
            else
                ConnectorProxy = null;
        }

        private void PartConnection_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (IsAssigningConnectorProperties)
                return;

            var changeArgs = new ElementValueChangedEventArgs(this, Connector, e.PropertyName, e.OldValue, e.NewValue);
            RaisePropertyValueChanged(changeArgs);
        }

        #endregion

        public static PartConnection FromLDD(Connector connector)
        {
            return new PartConnection(connector);
        }

        public static PartConnection FromXml(XElement element)
        {
            var connectorType = element.ReadAttribute<ConnectorType>("Type");
            var partConn = new PartConnection(connectorType);
            partConn.LoadFromXml(element);
            return partConn;
        }

        public static PartConnection Create(ConnectorType connectorType)
        {
            return new PartConnection(Connector.CreateFromType(connectorType));
        }

        public T GetConnector<T>() where T : Connector
        {
            return ConnectorProxy;
        }

        public Connector GenerateLDD()
        {
            IsAssigningConnectorProperties = true;
            Connector.Transform = Transform.ToLDD();
            IsAssigningConnectorProperties = false;
            return Connector;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            base.LoadFromXml(element);

            if (element.HasElement(nameof(Transform), out XElement transElem))
            {
                Transform = ItemTransform.FromXml(transElem);
                transElem.Remove();
            }

            var connElem = new XElement(ConnectorType.ToString());

            connElem.Add(Transform.ToLDD().ToXmlAttributes());

            foreach (var attr in element.Element("Properties").Attributes())
                connElem.Add(new XAttribute(attr.Name.LocalName, attr.Value));

            if (element.HasElement("StudsArray", out XElement studs))
                connElem.Value = studs.Value;

            IsAssigningConnectorProperties = true;
            Connector = Connector.CreateFromType(ConnectorType);
            Connector.LoadFromXml(connElem);
            IsAssigningConnectorProperties = false;

        }

        public override XElement SerializeToXml()
        {
            var elem = SerializeToXmlBase(NODE_NAME);

            elem.Add(new XAttribute("Type", ConnectorType));

            if (Transform != null)
            {
                elem.Add(new XComment(Transform.GetLddXml().ToString()));
                elem.Add(Transform.SerializeToXml());
            }

            if (Connector != null)
            {
                var connectorXml = Connector.SerializeToXml();

                var propElem = elem.AddElement("Properties");

                foreach (var attr in connectorXml.Attributes())
                {
                    if (LDD.Primitives.Transform.AttributeNames.Contains(attr.Name.LocalName))
                        continue;

                    propElem.Add(new XAttribute(attr.Name.LocalName.Capitalize(), attr.Value));
                    //propElem.AddElement(attr.Name.LocalName.Capitalize(), attr.Value);
                }

                if (!string.IsNullOrEmpty(connectorXml.Value))//Custom2DField
                {
                    elem.AddElement("StudsArray", connectorXml.Value
                        .Replace("\r", string.Empty)
                        .Replace("\n", string.Empty)
                        .Trim()
                    );
                }
            }
            
            return elem;
        }
    }
    
}
