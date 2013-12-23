using System.IO;

namespace CsvHelper.Regex
{
    public class RegexParser : CsvParser
    {
        private string pattern;
        private System.Text.RegularExpressions.Regex regex;

//        public RegexParser( TextReader reader ) : this( reader, new RegexConfiguration() ) {}

        public RegexParser(TextReader reader, RegexConfiguration configuration)
            : base(reader, configuration)
        {
            pattern = configuration.Pattern;
            regex = new System.Text.RegularExpressions.Regex(pattern);
        }
    }
}