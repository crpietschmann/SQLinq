using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqTableTest
    {
        [TestMethod]
        public void SQLinqTable_001()
        {
            var query = new SQLinq<SQLinqTable_001_Class>();
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[TableName]", result.Table);
        }

        [SQLinqTable("TableName")]
        private class SQLinqTable_001_Class
        {
            public int ID { get; set; }
        }

        [TestMethod]
        public void SQLinqTable_002()
        {
            var query = new SQLinq<SQLinqTable_002_Class>();
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[TableName]", result.Table);
        }

        [SQLinqTable("[TableName]")]
        private class SQLinqTable_002_Class
        {
            public int ID { get; set; }
        }

        [TestMethod]
        public void SQLinqTable_003()
        {
            var query = new SQLinq<SQLinqTable_003_Class>();
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[DatabaseName].[TableName]", result.Table);
        }

        [SQLinqTable("[DatabaseName].[TableName]")]
        private class SQLinqTable_003_Class
        {
            public int ID { get; set; }
        }


        [TestMethod]
        public void SQLinqTable_NameOverride_001()
        {
            var query = new SQLinq<SQLinqTable_NameOverride_001_Class>("NewTableName");
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[NewTableName]", result.Table);
        }

        [SQLinqTable("TableName")]
        private class SQLinqTable_NameOverride_001_Class
        {
            public int ID { get; set; }
        }

        [TestMethod]
        public void SQLinqTable_NameOverride_002()
        {
            var query = new SQLinq<SQLinqTable_NameOverride_002_Class>("[NewTableName]");
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[NewTableName]", result.Table);
        }

        [SQLinqTable("[TableName]")]
        private class SQLinqTable_NameOverride_002_Class
        {
            public int ID { get; set; }
        }

        [TestMethod]
        public void SQLinqTable_NameOverride_003()
        {
            var query = new SQLinq<SQLinqTable_NameOverride_003_Class>("[DatabaseName].[NewTableName]");
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[DatabaseName].[NewTableName]", result.Table);
        }

        [SQLinqTable("[DatabaseName].[TableName]")]
        private class SQLinqTable_NameOverride_003_Class
        {
            public int ID { get; set; }
        }

        [TestMethod]
        public void SQLinqTable_NameOverride_004()
        {
            var query = new SQLinq<SQLinqTable_NameOverride_004_Class>("[DatabaseName].[NewTableName]");
            var result = (SQLinqSelectResult)query.ToSQL();
            Assert.AreEqual("[DatabaseName].[NewTableName]", result.Table);
        }

        private class SQLinqTable_NameOverride_004_Class
        {
            public int ID { get; set; }
        }
      
    }
}
