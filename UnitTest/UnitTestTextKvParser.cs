using System.Collections.Generic;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class UnitTestTextKvParser
    {
        private void ProcessTest_MemInAndOut(Dictionary<string, string> obj, out Dictionary<string, string> obj2)
        {
            string s = TextKVParser.TextKVParser.Serialize(obj);
            obj2 = TextKVParser.TextKVParser.Deserialize<Dictionary<string, string>>(s);
        }

        private void ProcessTest_FileInAndOut(Dictionary<string, string> obj, out Dictionary<string, string> obj2)
        {
            string path = "temp.ini";
            TextKVParser.TextKVParser.SerializeToFile(obj, path);
            TextKVParser.TextKVParser.DeserializeFromFile(path, out obj2);
        }

        private static void AreEq(Dictionary<string, string> obj, Dictionary<string, string> obj2)
        {
            Assert.AreEqual(obj.Count, obj2.Count);
            foreach (var each in obj)
            {
                string o;
                var got = obj2.TryGetValue(each.Key, out o);
                Assert.True(got);
                Assert.AreEqual(o, each.Value);
            }
        }

        [Test]
        public void TestMain()
        {
            var obj = new Dictionary<string, string>();
            obj.Add("key1", "value1");
            obj.Add("key2", "value2");
            obj.Add("key3empty", "");
            obj.Add("key4spe", "a=b");

            Dictionary<string, string> obj2;

            ProcessTest_FileInAndOut(obj, out obj2);
            AreEq(obj, obj2);

            ProcessTest_MemInAndOut(obj, out obj2);
            AreEq(obj, obj2);
        }
    }
}