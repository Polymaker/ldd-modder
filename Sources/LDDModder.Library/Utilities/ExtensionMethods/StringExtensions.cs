using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string Tab(int amount, int length = 4)
        {
            return String.Empty.PadRight(amount * length, ' ');
        }

        public static string Tab(int amount, string tabChars)
        {
            string tabStr = string.Empty;
            for (int i = 0; i < amount; i++)
                tabStr += tabChars;
            return tabStr;
            //return Enumerable.Range(0, amount).Select(i => tabChars).Aggregate((x, y) => x + y);
        }

        public static string Indent(this string text, int amount)
        {
            return Indent(text, amount, Tab(1));
        }

        public static string Indent(this string text, int amount, int tabLength)
        {
            return Indent(text, amount, Tab(1, tabLength));
        }

        public static string Indent(this string text, int amount, string tabChars)
        {
            string identedText = string.Empty;
            text = text.Replace("\t", tabChars).Replace(Environment.NewLine, "\n").Replace("\r", "\n");
            var textLines = text.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int l = 0; l < textLines.Length; l++)
            {
                var line = textLines[l];
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (l + 1 < textLines.Length)
                        identedText += Environment.NewLine;
                    continue;
                }
                if (amount < 0)
                {
                    string fixedLine = line;
                    for (int i = 0; i < Math.Abs(amount); i++)
                    {
                        if (fixedLine.StartsWith(tabChars))
                        {
                            fixedLine = fixedLine.Substring(tabChars.Length);
                        }
                        //else if (fixedLine.StartsWith("\t"))
                        //{
                        //    fixedLine = fixedLine.Substring(1);
                        //}
                        else if (fixedLine.StartsWith(tabChars[0].ToString()))
                        {
                            fixedLine = fixedLine.TrimStart(tabChars[0], 3);
                        }
                        else
                            break;
                    }
                    identedText += fixedLine;
                    if (l + 1 < textLines.Length)
                        identedText += Environment.NewLine;
                }
                else
                {
                    identedText += Tab(amount, tabChars) + line;
                    if (l + 1 < textLines.Length)
                        identedText += Environment.NewLine;
                }
            }


            return identedText;
        }

        public static string TrimStart(this string text, char trimChar, int maxCount)
        {
            if (text.Length > 0 && text[0] == trimChar)
            {
                int i = 0;
                for (; i < Math.Min(maxCount, text.Length); i++)
                {
                    if (text[i] != trimChar)
                        break;
                }
                text.Substring(i);
            }
            return text;
        }

        public static string Capitalize(this string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        public static bool MatchesWildcard(this string text, string searchPattern)
        {
            string regexString = Regex.Escape(searchPattern);
            regexString = "^" + Regex.Replace(regexString, @"\\\*", ".*");
            regexString = Regex.Replace(regexString, @"\\\?", ".");

            return Regex.IsMatch(text, regexString);
        }
    }
}
