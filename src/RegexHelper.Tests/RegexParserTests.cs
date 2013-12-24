using System.IO;
using System.Text;
using RegexHelper;
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using CsvHelper.Regex;

namespace RegexHelper.Tests
{
    [TestClass]
    public class RegexParserTests
    {
        // See http://msdn.microsoft.com/en-us/library/h5181w5w(v=vs.110).aspx - end with \r?$
        private const string CEDictPattern = @"([^\s]+)\s+([^\s]+)\s+\[([^\]]+)\]\s+([^\r^\n]+)\r?$";
        
        [TestMethod]
        public void CEDictParseTest()
        {
            var config = new RegexConfiguration(CEDictPattern);


            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(stream))
            using (var parser = new RegexParser(reader, config))
            {
                writer.Write("中國 中国 [Zhong1 guo2] /China/Middle Kingdom/\r\n");
                writer.Write("珍・奧斯汀 珍・奥斯汀 [Zhen1 · Ao4 si1 ting1] /Jane Austen (1775-1817), English novelist/\r\n");
                writer.Write("人為財死，鳥為食亡 人为财死，鸟为食亡 [ren2 wei4 cai2 si3 , niao3 wei4 shi2 wang2] /Human beings die in pursuit of wealth, and birds die in pursuit of food/…/\r\n");
                writer.Write("女兒 女儿 [nu:3 er2] /daughter/\r\n");
                writer.Write("頭兒 头儿 [tou2 r5] /leader/\r\n");
                writer.Write("花兒 花儿 [hua1 r5] /erhua variant of 花/flower/\r\n");
                writer.Flush();
                stream.Position = 0;

                var row = parser.Read();            // there is a problem here where the /r is picked up
                Assert.IsNotNull(row);
                Assert.AreEqual(4, row.Length);
                Assert.AreEqual("中國", row[0]);
                Assert.AreEqual("中国", row[1]);
                Assert.AreEqual("Zhong1 guo2", row[2]);
                Assert.AreEqual("/China/Middle Kingdom/", row[3]);

                row = parser.Read();
                Assert.IsNotNull(row);
                Assert.AreEqual(4, row.Length);
                Assert.AreEqual("珍・奧斯汀", row[0]);
                Assert.AreEqual("珍・奥斯汀", row[1]);
                Assert.AreEqual("Zhen1 · Ao4 si1 ting1", row[2]);
                Assert.AreEqual("/Jane Austen (1775-1817), English novelist/", row[3]);

                row = parser.Read();
                Assert.IsNotNull(row);
                Assert.AreEqual(4, row.Length);
                Assert.AreEqual("人為財死，鳥為食亡", row[0]);
                Assert.AreEqual("人为财死，鸟为食亡", row[1]);
                Assert.AreEqual("ren2 wei4 cai2 si3 , niao3 wei4 shi2 wang2", row[2]);
                Assert.AreEqual("/Human beings die in pursuit of wealth, and birds die in pursuit of food/…/", row[3]);

                row = parser.Read();
                Assert.IsNotNull(row);
                Assert.AreEqual(4, row.Length);

                row = parser.Read();
                Assert.IsNotNull(row);
                Assert.AreEqual(4, row.Length);

                row = parser.Read();
                Assert.IsNotNull(row);
                Assert.AreEqual(4, row.Length);

                Assert.IsNull(parser.Read());

            }
        }
    }
}
