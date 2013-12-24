using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CsvHelper.Regex
{
    public class RegexParser : CsvParser
    {
        protected string pattern;
        protected System.Text.RegularExpressions.Regex regex;

//        public RegexParser( TextReader reader ) : this( reader, new RegexConfiguration() ) {}

        public RegexParser(TextReader reader, RegexConfiguration configuration)
            : base(reader, configuration)
        {
            pattern = configuration.Pattern;
            regex = new System.Text.RegularExpressions.Regex(pattern);
        }

        protected override string[] ReadLine()
        {
            string field = null;
			var fieldStartPosition = readerBufferPosition;
			var rawFieldStartPosition = readerBufferPosition;
			var inComment = false;
			var prevCharWasDelimiter = false;
			var recordPosition = 0;
			record = new string[FieldCount];
			RawRecord = string.Empty;
			currentRow++;

            while (true)
            {
                if (read)
                {
                    cPrev = c;
                }

                var fieldLength = readerBufferPosition - fieldStartPosition;
                read = GetChar(out c, ref fieldStartPosition, ref rawFieldStartPosition, ref field, prevCharWasDelimiter, ref recordPosition, ref fieldLength);
                if (!read)
                {
                    break;
                }
                readerBufferPosition++;
                CharPosition++;

                prevCharWasDelimiter = false;

                if (inComment && c != '\r' && c != '\n')
                {
                    // We are on a commented line.
                    // Ignore the character.
                }
                else if (c == '\r' || c == '\n')
                {
                    fieldLength = readerBufferPosition - fieldStartPosition - 1;
                    if (c == '\r')
                    {
                        char cNext;
                        GetChar(out cNext, ref fieldStartPosition, ref rawFieldStartPosition, ref field, prevCharWasDelimiter, ref recordPosition, ref fieldLength, true);
                        if (cNext == '\n')
                        {
                            readerBufferPosition++;
                            CharPosition++;
                        }
                    }

                    if (cPrev == '\r' || cPrev == '\n' || inComment || cPrev == null)
                    {
                        // We have hit a blank line. Ignore it.

                        UpdateBytePosition(fieldStartPosition, readerBufferPosition - fieldStartPosition);

                        fieldStartPosition = readerBufferPosition;
                        inComment = false;
                        currentRow++;
                        continue;
                    }

                    // If we hit the end of the record, add 
                    // the current field and return the record.
                    AppendField(ref field, fieldStartPosition, fieldLength);
                    // Include the \r or \n in the byte count.
                    UpdateBytePosition(fieldStartPosition, readerBufferPosition - fieldStartPosition);
                    AddFieldToRecord(ref recordPosition, field);
                    break;
                }
                else if (configuration.AllowComments && c == configuration.Comment && (cPrev == '\r' || cPrev == '\n' || cPrev == null))
                {
                    inComment = true;
                }
            }

            if (record != null)
            {
                RawRecord += new string(readerBuffer, rawFieldStartPosition, readerBufferPosition - rawFieldStartPosition);

                record = ParseToRecord(RawRecord);
            }

            return record;
        }

        /// <summary>
        /// Default ParseToRecord handles a single layer, may need a custom one if the Regex is complex
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        protected virtual string[] ParseToRecord(string raw)
        {
            string[] record = null;
            
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
                record = results.ToArray();
            }

            return record;
        }
    }
}