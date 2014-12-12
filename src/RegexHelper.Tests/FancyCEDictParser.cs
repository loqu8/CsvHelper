using CsvHelper.Regex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexHelper.Tests
{
    public class FancyCEDictParser : RegexParser
    {
        private const string FancyCEDictPattern = @"(([^\s]+)\s+){1,2}\[([^\]]+)\]\s+([^\r^\n]+)\r?$";


        /// <summary>
        /// Try to handle when users do not add either traditional or simplified
        /// </summary>
        /// <param name="reader"></param>
        public FancyCEDictParser(TextReader reader)
            : base(reader, new RegexConfiguration(FancyCEDictPattern))
        {
        }

        protected override string[] ParseToRecord(string raw)
        {
            string[] record = null;

            var results = new List<string>();
            var match = regex.Match(RawRecord);
            if (match.Success && match.Groups.Count > 4)
            {
                // specific implementation starts at Groups[3]
                if (match.Groups[2].Captures.Count > 0)
                {
                    results.Add(match.Groups[2].Captures[0].Value);
                }

                if (match.Groups[2].Captures.Count > 1)             // the alternate
                {
                    results.Add(match.Groups[2].Captures[1].Value);
                }
                else
                {
                    results.Add("");
                }

                results.Add(match.Groups[3].Captures[0].Value);
                results.Add(match.Groups[4].Captures[0].Value);
            }

            if (results.Count > 0)
            {
                record = results.ToArray();
            }

            return record;
        }
    }
}


