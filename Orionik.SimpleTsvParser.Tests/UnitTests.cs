using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orionik.SimpleTsvParser.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void SimpleParseFirst()
        {
            var parser = new TsvParser(noneIn: "none", noneOut: "mynone");

            string[][] str = {
                new[] {"1", "2", "3", "4"},
                new[] {"test1", "test2", "none"},
                new[] {"none", "test3", "test4", ""}
            };

            var obj = parser.Parse(str);

            Assert.AreEqual("test4", obj[1, 2]);
            Assert.AreEqual("mynone", obj[1, 3]);
            Assert.AreEqual("mynone", obj[0, 2]);
            Assert.AreEqual("mynone", obj[0, 3]);
            Assert.AreEqual("2", obj.Header[1]);
            Assert.AreEqual(4, obj.ColumnCount);
            Assert.AreEqual("mynone", obj.None);
            Assert.AreEqual(2, obj.RowCount);
            Assert.AreEqual(2, obj.Rows.GetLength(0));
            Assert.AreEqual(4, obj.Rows.GetLength(1));
            Assert.AreEqual("test1", obj.GetRow(0)[0]);
            Assert.AreEqual("mynone", obj.GetColumn("1")[1]);
            Assert.AreEqual(-1, obj.GetColumnIndex("33"));
        }

        [TestMethod]
        public void SimpleParseSecond()
        {
            var parser = new TsvParser(firstIsHeader:false, noneIn:"none", noneOut:"mynone");
            var text = "test1\ttest2\test3\nnone\ttest4\ttest5";
            var obj = parser.Parse(text);

            obj.AddHeader("newHeder1", new[] {"newTest1", "newTest2"});
            obj.AddRow(new []{ "newRowItem1", "newRowItem2" });

            Assert.AreEqual("Item1", obj.Header[1]);
            Assert.AreEqual("newHeder1", obj.Header[3]);
            Assert.AreEqual(3, obj.RowCount);
            Assert.AreEqual(4, obj.ColumnCount);
            Assert.AreEqual("newRowItem2", obj.GetRow(2)[1]);
            Assert.AreEqual("mynone", obj.GetRow(2)[2]);
            Assert.AreEqual("newTest1", obj.GetRow(0)[3]);
        }

        [TestMethod]
        [DeploymentItem("Assets\\data.tsv", "Input")]
        public void FileTest()
        {
            var parser = new TsvParser(noneIn: "\\N");
            var obj = parser.ParseFile("Assets\\data.tsv");
            obj.AddHeader("TestHeader");
            var val = obj.AddHeader("TestHeader");
            obj.AddRow();
            obj[1, 1] = "TestMessage";

            Assert.AreEqual(221,obj.RowCount);
            Assert.AreEqual(7, obj.ColumnCount);
            Assert.AreEqual(val, false);
            Assert.AreEqual(obj[1, 1], "TestMessage");
        }

        [TestMethod]
        [ExpectedException(typeof(TsvException))]
        public void TsvListSecond()
        {
            var parser = new TsvParser(noneIn: "none", noneOut: "mynone");

            string[][] str = {
                new[] {"1", "1", "3", "4"},
                new[] {"test1", "test2", "none"},
                new[] {"none", "test3", "test4", ""}
            };

            var obj = parser.Parse(str);
        }
    }
}
