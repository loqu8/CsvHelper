using CsvHelper.Configuration;

namespace CsvHelper.Regex
{
    public class RegexConfiguration : CsvConfiguration
    {
        public RegexConfiguration(string pattern)
        {
            Pattern = pattern;
        }

        #region Properties

        private string pattern;

        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        #endregion Properties
    }
}