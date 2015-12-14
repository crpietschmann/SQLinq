using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqColumnTest
    {
        [TestMethod]
        public void UseWithSQLinqSubQuery_001()
        {
            var query = new SQLinq<MyTable>();

            var result = query.ToSQL();
            var actual = result.ToQuery();

            var expected = "SELECT [Identifier] AS [ID], [FullName] AS [Name] FROM (SELECT [Identifier], [FullName] FROM [tblSomeTable]) AS [MyTable]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UseWithSQLinqSubQuery_Oracle_001()
        {
            var dialect = new OracleDialect();
            var query = new SQLinq<MyTable>(dialect);

            var result = query.ToSQL();
            var actual = result.ToQuery();

            var expected = "SELECT Identifier AS ID, FullName AS Name FROM (SELECT [Identifier], [FullName] FROM [tblSomeTable]) AS MyTable";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UseWithSQLinqSubQuery_002()
        {
            var query = from item in new SQLinq<MyTable>()
                        where item.Name == "Chris"
                        select item;

            var result = query.ToSQL();
            var actual = result.ToQuery();

            var expected = "SELECT [Identifier] AS [ID], [FullName] AS [Name] FROM (SELECT [Identifier], [FullName] FROM [tblSomeTable]) AS [MyTable] WHERE [FullName] = @sqlinq_1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UseWithSQLinqSubQuery_Oracle_002()
        {
            var dialect = new OracleDialect();
            var query = from item in new SQLinq<MyTable>(dialect)
                        where item.Name == "Chris"
                        select item;

            var result = query.ToSQL();
            var actual = result.ToQuery();

            var expected = "SELECT Identifier AS ID, FullName AS Name FROM (SELECT [Identifier], [FullName] FROM [tblSomeTable]) AS MyTable WHERE FullName = :sqlinq_1";

            Assert.AreEqual(expected, actual);
        }

        [SQLinqSubQuery(SQL = @"SELECT [Identifier], [FullName] FROM [tblSomeTable]")]
        private class MyTable
        {
            [SQLinqColumn("Identifier")]
            public int ID { get; set; }

            [SQLinqColumn("FullName")]
            public string Name { get; set; }
        }
    }
}
