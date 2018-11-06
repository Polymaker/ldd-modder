using LDDModder.LDD.General;
using LDDModder.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LDDModder.LDD.Models
{
    [Serializable, XmlRoot("LXFML")]
    public class Assembly : XSerializable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MainGroup Group { get; set; }

        public Platform Platform { get; set; }

        public VersionInfo Version { get; set; }

        public string Wire { get; set; }

        public List<int> Aliases { get; }

        public Assembly()
        {
            Aliases = new List<int>();
            Id = 0;
            Name = string.Empty;
            Group = null;
            Platform = null;
            Version = null;
            Wire = null;
        }

        #region Xml Serialization

        protected void DeserializeAnnotation(XElement annotation)
        {
            var annotAttr = annotation.FirstAttribute;
            switch (annotAttr.Name.LocalName.ToLower())
            {
                case "aliases":
                    var aliases = annotAttr.Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < aliases.Length; i++)
                    {
                        int aliasId = 0;
                        if (int.TryParse(aliases[i], out aliasId))
                            Aliases.Add(aliasId);
                    }
                    break;
                case "designname":
                    Name = annotAttr.Value; break;
                case "maingroupid":
                    if (Group == null)
                        Group = new MainGroup();
                    Group.Id = int.Parse(annotAttr.Value);
                    break;
                case "maingroupname":
                    if (Group == null)
                        Group = new MainGroup();
                    Group.Name = annotAttr.Value;
                    break;
                case "platformid":
                    if (Platform == null)
                        Platform = new Platform();
                    Platform.Id = int.Parse(annotAttr.Value);
                    break;
                case "platformname":
                    if (Platform == null)
                        Platform = new Platform();
                    Platform.Name = annotAttr.Value;
                    break;
                case "wire":
                    Wire = annotAttr.Value;
                    break;
            }
        }


        #endregion

        protected override void DeserializeFromXElement(XElement element)
        {
            throw new NotImplementedException();
        }

        protected override XElement SerializeToXElement()
        {
            throw new NotImplementedException();
        }
    }
}
