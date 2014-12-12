using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CsvHelper.Regex
{
    public class RegexParser : CsvParser
    {
        protected string pattern;
        protected System.Text.RegularExpressions.Regex regex;

        public RegexParser(TextReader reader, RegexConfiguration configuration)
            : base(reader, configuration)
        {
            pattern = configuration.Pattern;
            regex = new System.Text.RegularExpressions.Regex(pattern);
        }

        protected override string[] ReadLine()
        {
            var result = base.ReadLine();

            if (result != null)
            {
                result = ParseToRecord(RawRecord);              
            }

            return result;
        }

        /// <summary>
        /// Default ParseToRecord handles a single layer, may need a custom one if the Regex is complex
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        protected virtual string[] ParseToRecord(string raw)
        {
            string[] result = null;

            var results = new List<string>();
            var match = regex.Match(RawRecord);
            if (match.Success && match.Groups.Count > 1)
            {
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    results.Add(match.Groups[i].Captures[0].Value);
                }
            }

            if (results.Count > 0)
            {
                result = results.ToArray();
            }

            return result;
        }
    }
}
