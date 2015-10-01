using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;
using System.Collections.Generic;
using System.Linq;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqJoinResultTest
    {
        [TestMethod]
        public void SQLinqJoinResult_Constructor_001()
        {
            var target = new SQLinqJoinResult();
            Assert.AreEqual(0, target.Join.Count());
            Assert.AreEqual(0, target.Parameters.Count());
        }

        [TestMethod]
        public void SQLinqJoinResult_Constructor_002()
        {
            var joins = new string[] { "one", "two" };
            var target = new SQLinqJoinResult(joins);
            Assert.AreEqual(target.Join, joins);
        }

        [TestMethod]
        public void SQLinqJoinResult_Constructor_003()
        {
            var joins = new string[] { "one", "two" };
            var parameters = new Dictionary<string, object> { { "one", 1 }, { "two", "TTWWOO" } };
            var target = new SQLinqJoinResult(joins, parameters);
            Assert.AreEqual(target.Join, joins);
            Assert.AreEqual(target.Parameters, parameters);
        }
    }
}

