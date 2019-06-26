using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
namespace LDDModder.Serialization
{
    public class XmlTextWriterEx : XmlTextWriter
    {
        private int indentLevel;
        private bool isInsideAttribute;
        private XmlWriterSettings _Settings;

        public override XmlWriterSettings Settings
        {
            get
            {
                return _Settings;
            }
        }

        #region Ctors

        public XmlTextWriterEx(System.IO.Stream w, XmlWriterSettings settings)
            : base(w, settings.Encoding)
        {
            _Settings = settings;
            Formatting = settings.Indent ? Formatting.Indented : Formatting.None;
            Indentation = settings.IndentChars.Length;
        }

        public XmlTextWriterEx(System.IO.Stream w, Encoding encoding)
            : base(w, encoding)
        {

        }
        public XmlTextWriterEx(string filename, Encoding encoding)
            : base(filename, encoding)
        {

        }
        public XmlTextWriterEx(System.IO.TextWriter w)
            : base(w)
        {
            //XmlWriterSettings
        }



        #endregion

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            isInsideAttribute = true;
            base.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteEndAttribute()
        {
            base.WriteEndAttribute();
            isInsideAttribute = false;
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            indentLevel++;
            base.WriteStartElement(prefix, localName, ns);
        }

        public override void WriteFullEndElement()
        {
            indentLevel--;
            base.WriteFullEndElement();
        }

        public override void WriteEndElement()
        {
            indentLevel--;
            base.WriteEndElement();
        }

        public override void WriteValue(object value)
        {
            base.WriteValue(value);
        }

        public override void WriteString(string text)
        {
            if (String.IsNullOrEmpty(text) || isInsideAttribute || Formatting != Formatting.Indented || XmlSpace == XmlSpace.Preserve)
            {
                base.WriteString(text);
                return;
            }
            //already indented
            if (text.StartsWith("\n" + StringExtensions.Tab(indentLevel, Settings.IndentChars)) &&
                text.EndsWith("\n" + StringExtensions.Tab(indentLevel - 1, Settings.IndentChars)))
            {
                base.WriteString(text);
                return;
            }

            var indentedText = text.Indent(indentLevel, Settings.IndentChars).Replace(Environment.NewLine, "\n");
            base.WriteString(string.Format("\n{0}\n", indentedText) + StringExtensions.Tab(indentLevel - 1, Settings.IndentChars));
        }
    }
}
