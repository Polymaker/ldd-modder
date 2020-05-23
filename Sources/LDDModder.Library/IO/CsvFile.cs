using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LDDModder.IO
{
    public class CsvFile
    {
        private static Regex QuoteCleaner = new Regex("(?<=^|[,;\t])\"((\"\"|[^\"])+)\"(?=[,;\t]|$)", RegexOptions.Compiled);

        public string CsvSeparator { get; private set; } = ";";

        public IFormatProvider ParsingFormatProvider { get; set; }

        #region Classes & enums

        public enum Separator
        {
            AutoDetect,
            Comma = ',',
            Tab = '\t',
            SemiColon = ';'
        }

        private interface ICsvItem
        {
            CsvFile File { get; set; }
        }

        public class CsvRow : ICsvItem
        {
            public CsvFile File { get; private set; }

            CsvFile ICsvItem.File { get => File; set => File = value; }

            public int Index => File != null ? File.Rows.IndexOf(this) : -1;

            public List<string> Values { get; } = new List<string>();

            public int ColumnCount => Values.Count;

            public string this[int index]
            {
                get => index < Values.Count ? Values[index] : null;
                set
                {
                    while (index >= Values.Count)
                        Values.Add(null);
                    Values[index] = value;
                }
            }

            public string this[string columnName]
            {
                get
                {
                    if (IsHeader)
                        throw new InvalidOperationException("An header row cannot be referenced by column name");

                    var header = File?.Rows.LastOrDefault(r => r.Index < Index);
                    if (header != null)
                    {
                        for (int i = 0; i < header.Values.Count; i++)
                        {
                            if (header[i].Trim().ToLower() == columnName.Trim().ToLower())
                                return this[i];
                        }
                    }
                    return null;
                }
                set
                {
                    if (IsHeader)
                        throw new InvalidOperationException("An header row cannot be referenced by column name");

                    var header = File?.Rows.LastOrDefault(r => r.Index < Index);
                    if (header != null)
                    {
                        for (int i = 0; i < header.Values.Count; i++)
                        {
                            if (header[i].Trim().ToLower() == columnName.Trim().ToLower())
                                this[i] = value;
                        }
                    }
                }
            }

            public bool IsHeader { get; set; }

            public CsvRow() { }

            public CsvRow(params string[] values)
            {
                Values.AddRange(values);
            }

            public override string ToString()
            {
                return "[" + string.Join(File?.CsvSeparator ?? ",", Values) + "]";
            }
        }

        public class CsvRowCollection : IList<CsvRow>
        {
            public CsvFile File { get; private set; }
            private List<CsvRow> _Rows;

            public CsvRowCollection(CsvFile file)
            {
                File = file;
                _Rows = new List<CsvRow>();
            }

            public CsvRow this[int index] { get => ((IList<CsvRow>)_Rows)[index]; set => ((IList<CsvRow>)_Rows)[index] = value; }

            public int Count => ((IList<CsvRow>)_Rows).Count;

            public bool IsReadOnly => ((IList<CsvRow>)_Rows).IsReadOnly;

            public void Add(CsvRow item)
            {
                SetParent(item as ICsvItem, File);
                ((IList<CsvRow>)_Rows).Add(item);
            }

            public void Clear()
            {
                _Rows.ForEach(r => SetParent(r as ICsvItem, null));
                ((IList<CsvRow>)_Rows).Clear();
            }

            public bool Contains(CsvRow item)
            {
                return ((IList<CsvRow>)_Rows).Contains(item);
            }

            public void CopyTo(CsvRow[] array, int arrayIndex)
            {
                ((IList<CsvRow>)_Rows).CopyTo(array, arrayIndex);
            }

            public IEnumerator<CsvRow> GetEnumerator()
            {
                return ((IList<CsvRow>)_Rows).GetEnumerator();
            }

            public int IndexOf(CsvRow item)
            {
                return ((IList<CsvRow>)_Rows).IndexOf(item);
            }

            public void Insert(int index, CsvRow item)
            {
                SetParent(item as ICsvItem, File);
                ((IList<CsvRow>)_Rows).Insert(index, item);
            }

            public bool Remove(CsvRow item)
            {
                SetParent(item as ICsvItem, null);
                return ((IList<CsvRow>)_Rows).Remove(item);
            }

            public void RemoveAt(int index)
            {
                SetParent(this[index] as ICsvItem, null);
                ((IList<CsvRow>)_Rows).RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IList<CsvRow>)_Rows).GetEnumerator();
            }

            public CsvRow Add(params string[] values)
            {
                var newRow = new CsvRow(values);
                Add(newRow);
                return newRow;
            }

            public CsvRow AddRow()
            {
                var newRow = new CsvRow();
                Add(newRow);
                return newRow;
            }

            private void SetParent(ICsvItem item, CsvFile parent)
            {
                if (item != null)
                    item.File = parent;
            }
        }

        //private class 

        #endregion

        public CsvRowCollection Rows { get; }

        public string this[int row, int col]
        {
            get { return Rows[row][col]; }
        }

        public CsvFile()
        {
            Rows = new CsvRowCollection(this);
            ParsingFormatProvider = CultureInfo.CurrentCulture;
        }

        public CsvFile(Separator separator)
            : this((char)separator)
        {

        }

        public CsvFile(char separator)
        {
            if (!(separator == ',' || separator == '\t' || separator == ';'))
                throw new NotSupportedException($"Invalid CSV separator: '{separator}'");

            Rows = new CsvRowCollection(this);
            CsvSeparator = separator.ToString();
        }

        public static CsvFile Read(string filepath, Separator separator = Separator.AutoDetect)
        {
            using (var fs = File.OpenRead(filepath))
            {
                return Read(fs, separator);
            }
        }

        public static CsvFile Read(Stream stream, Separator separator = Separator.AutoDetect)
        {
            var csv = new CsvFile();

            using (var sr = new StreamReader(stream))
            {

                var allLines = new List<string>();
                while (sr.Peek() != -1)
                    allLines.Add(sr.ReadLine());

                char separatorChar = ' ';

                if (separator == Separator.AutoDetect)
                {
                    var cleanedLines = allLines.Select(l => QuoteCleaner.Replace(l, "TEXT")).ToList();
                    var avgComma = cleanedLines.Average(l => l.Count(c => c == ','));
                    var avgTab = cleanedLines.Average(l => l.Count(c => c == '\t'));
                    var avgSemi = cleanedLines.Average(l => l.Count(c => c == ';'));
                    if (avgComma > avgTab && avgComma > avgSemi)
                        separatorChar = ',';
                    else if (avgTab > avgComma && avgTab > avgSemi)
                        separatorChar = '\t';
                    else
                        separatorChar = ';';
                }
                else
                {
                    separatorChar = (char)separator;
                }

                csv.CsvSeparator = separatorChar.ToString();

                for (int lineIndex = 0; lineIndex < allLines.Count; lineIndex++)
                {
                    string currentLine = allLines[lineIndex];

                    var escapedValues = new List<string>();

                    var cleanedLine = QuoteCleaner.Replace(currentLine, (m) =>
                    {
                        escapedValues.Add(m.Groups[1].Value.Replace("\"\"", "\""));
                        return $"#{escapedValues.Count - 1}#";
                    });

                    var lineValues = cleanedLine.Split(separatorChar);

                    //handle new lines in escaped value
                    while (lineValues.Length > 0 && lineValues[lineValues.Length - 1].StartsWith("\""))
                    {
                        currentLine += Environment.NewLine + allLines[lineIndex + 1];
                        lineIndex++;
                        escapedValues.Clear();

                        cleanedLine = QuoteCleaner.Replace(currentLine, (m) =>
                        {
                            escapedValues.Add(m.Groups[1].Value.Replace("\"\"", "\""));
                            return $"#{escapedValues.Count - 1}#";
                        });
                        lineValues = cleanedLine.Split(separatorChar);
                    }

                    if (escapedValues.Count > lineValues.Length)
                    {
                        Debug.WriteLine("Incorrect separator or invalid CSV file.");
                        for (int i = 0; i < lineValues.Length; i++)
                        {
                            for (int j = 0; j < escapedValues.Count; j++)
                                lineValues[i] = lineValues[i].Replace($"#{j}#", escapedValues[j]);
                        }
                    }
                    else if (escapedValues.Count > 0)
                    {
                        int curText = 0;
                        for (int i = 0; i < lineValues.Length; i++)
                        {
                            if (lineValues[i] == $"#{curText}#")
                                lineValues[i] = escapedValues[curText++];
                        }
                    }

                    csv.Rows.Add(lineValues);
                }
            }

            return csv;
        }

        public void Save(string filepath, Separator separator = Separator.AutoDetect)
        {
            using (var fs = File.Open(filepath, FileMode.Create))
                Save(fs, separator);
        }

        public void Save(Stream stream, Separator separator = Separator.AutoDetect)
        {
            var csvSeparator = separator == Separator.AutoDetect ? CsvSeparator : ((char)separator).ToString();

            using (var sw = new StreamWriter(stream))
            {
                foreach (var row in Rows)
                {
                    var csvLine = string.Join(csvSeparator,
                        row.Values.Select(
                            v => v == null ? string.Empty : EscapeCsvValue(v, csvSeparator)
                            )
                        );
                    sw.WriteLine(csvLine);
                }
            }
        }

        public static string EscapeCsvValue(string value, string separator)
        {
            if (value.Contains(separator))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }
    }
}
