using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCsvExporter;

namespace SimpleCsvExporterTest
{
    [TestClass]
    public class RecordProcessorTest
    {
        private class TestRecordClass
        {
            [CsvColumn(name: "ID")]
            public int Id;

            public int hiddenField;

            [CsvColumn(name: "日本語名")]
            public string value = "";
        }

        [TestMethod]
        public void TestGetFieldNameList()
        {
            var processor = new RecordProcessor<TestRecordClass>(new List<TestRecordClass>());
            var actual = processor.GetFieldNameList();
            var expected = new List<string>()
            {
                "ID",
                "日本語名"
            };
            Assert.IsTrue(IsListEqual(actual, expected));
        }

        [TestMethod]
        public void TestRecordValueList()
        {
            var dataList = new List<TestRecordClass>()
            {
                new TestRecordClass() {Id = 1, hiddenField = 1, value = "a"},
                new TestRecordClass() {Id = 2, hiddenField = 3, value = ","},
            };

            var processor = new RecordProcessor<TestRecordClass>(dataList);
            var actualList = processor.GetRecordValueList();
            var expectedList = new List<List<string>>();
            var expected1 = new List<string>()
            {
                "1",
                "a",
            };
            expectedList.Add(expected1);
            var expected2 = new List<string>()
            {
                "2",
                "\\,"
            };
            expectedList.Add(expected2);

            for (var i = 0; i < actualList.Count; i++)
            {
                Assert.IsTrue(IsListEqual(actualList[i], expectedList[i]));
            }
        }

        private bool IsListEqual<T>(List<T> actual, List<T> expected)
        {
            // 要素数が異なっていたらfalse
            if (actual.Count != expected.Count)
            {
                return false;
            }

            var isEqual = true;
            var listCount = actual.Count;
            for (var i = 0; i < listCount; i++)
            {
                if (actual[i].Equals(expected[i]) == false)
                {
                    isEqual = false;
                    break;
                }
            }

            return isEqual;
        }
    }
}
