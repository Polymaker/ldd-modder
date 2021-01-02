using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDDModder.Modding
{
    public class ProjectInfo : PartElement
    {
        public const string NODE_NAME = "Info";

        private string _Authors;
        private string _DerivedFrom;
        private string _OriginalAuthor;
        private string _Comments;
        private DateTime _LastModification;

        public string Authors
        {
            get => _Authors;
            set => SetPropertyValue(ref _Authors, value);
        }

        public string DerivedFrom
        {
            get => _DerivedFrom;
            set => SetPropertyValue(ref _DerivedFrom, value);
        }

        public string OriginalAuthor
        {
            get => _OriginalAuthor;
            set => SetPropertyValue(ref _OriginalAuthor, value);
        }

        public string Comments
        {
            get => _Comments;
            set => SetPropertyValue(ref _Comments, value);
        }

        public DateTime LastModification
        {
            get => _LastModification;
            set => SetPropertyValue(ref _LastModification, value);
        }

        public ProjectInfo()
        {
            Authors = string.Empty;
            Comments = string.Empty;
            DerivedFrom = string.Empty;
            OriginalAuthor = string.Empty;
            _LastModification = DateTime.Now;
        }

        public ProjectInfo(PartProject project) : this()
        {
            _Project = project;
        }

        public override XElement SerializeToXml()
        {
            var root = SerializeToXmlBase(NODE_NAME);
            root.RemoveAttributes();

            if (!string.IsNullOrWhiteSpace(Authors))
                root.Add(new XElement(nameof(Authors), Authors));

            if (!string.IsNullOrWhiteSpace(DerivedFrom))
                root.Add(new XElement(nameof(DerivedFrom), DerivedFrom));

            if (!string.IsNullOrWhiteSpace(OriginalAuthor))
                root.Add(new XElement(nameof(OriginalAuthor), OriginalAuthor));

            if (!string.IsNullOrWhiteSpace(Comments))
                root.Add(new XElement(nameof(Comments), Comments));
            
            if (LastModification != DateTime.MinValue)
                root.Add(new XElement(nameof(LastModification), LastModification.ToString(CultureInfo.InvariantCulture)));
            return root;
        }

        protected internal override void LoadFromXml(XElement element)
        {
            Authors = element.ReadElement(nameof(Authors), string.Empty);
            DerivedFrom = element.ReadElement(nameof(DerivedFrom), string.Empty);
            OriginalAuthor = element.ReadElement(nameof(OriginalAuthor), string.Empty);
            Comments = element.ReadElement(nameof(Comments), string.Empty);
            LastModification = element.ReadElement(nameof(LastModification), DateTime.Now);

            base.LoadFromXml(element);
        }


        public void ParseXmlComments(string comments)
        {
            Comments = string.Empty;

            bool IsMatch(string input, string pattern, out string result)
            {
                result = null;
                var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (match.Success && match.Groups.Count > 1)
                    result = match.Groups[1].Value;
                return match.Success;
            }

            using (var sr = new StringReader(comments))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (IsMatch(line, "(?:created|updated|LDD).+?by\\s?:? (.+)", out string author))
                    {
                        Authors = author;
                    }
                    else if (IsMatch(line, "derived from\\s?:?\\s*(.+)", out string derivedFrom))
                    {
                        DerivedFrom = derivedFrom;
                    }
                    else if (IsMatch(line, "orginal author\\s?:?\\s*(.+)", out string originalAuthor))
                    {
                        OriginalAuthor = originalAuthor;
                    }
                    else if (IsMatch(line, "(\\d{4}.\\d{1,2}.\\d{1,2}|\\d{1,2}.\\d{1,2}.\\d{4})", out string dateStr))
                    {
                        
                    }
                    else if (IsMatch(line, "comments\\s?:? (.+)", out string comment) && string.IsNullOrEmpty(Comments))
                    {
                        Comments = comment;
                    }
                    else if (!string.IsNullOrEmpty(line))
                    {
                        if (!string.IsNullOrEmpty(Comments))
                            Comments += Environment.NewLine;
                        Comments += line;
                    }
                }
            }
        }

        public string GenerateXmlComments()
        {
            string comment = string.Empty;
            void AppendComment(string value)
            {
                //if (!string.IsNullOrEmpty(comment))
                comment += Environment.NewLine;
                comment += "\t" + value;
            }

            if (!string.IsNullOrEmpty(DerivedFrom))
                AppendComment($"Derived from: {DerivedFrom}");

            if (!string.IsNullOrEmpty(OriginalAuthor))
                AppendComment($"Orginal Author: {OriginalAuthor}");

            if (!string.IsNullOrEmpty(Authors))
            {
                if (string.IsNullOrEmpty(DerivedFrom))
                    AppendComment($"Created for LDD by: {Authors}");
                else
                    AppendComment($"Editted for LDD by: {Authors}");
            }

            if (LastModification != DateTime.MinValue)
                AppendComment($"Last modification: {LastModification:yyyy-MM-dd}");

            if (!string.IsNullOrEmpty(Comments))
                AppendComment($"Comments: {Comments}");

            if (!string.IsNullOrEmpty(comment))
                comment += Environment.NewLine;

            return comment;
        }
    }
}
