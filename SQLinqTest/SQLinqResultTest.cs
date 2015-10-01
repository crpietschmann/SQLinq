//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqResultTest
    {
        #region ToQuery
        
        [TestMethod]
        public void SQLinqResultTest_ToQuery_001()
        {
            var query = from d in new SQLinq<Person>()
                        where d.Age == 12
                        select d;

            var result = query.ToSQL();

            var sql = result.ToQuery();

            Assert.AreEqual("SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] = @sqlinq_1", sql);
        }

        [TestMethod]
        public void SQLinqResultTest_ToQuery_002()
        {
            var query = from d in new SQLinq<Person>()
                        where d.Age == 12
                        select new {
                            FN = d.FirstName,
                            LN = d.LastName,
                            Age = d.Age
                        };

            var result = query.ToSQL();

            var sql = result.ToQuery();

            Assert.AreEqual("SELECT [FirstName] AS [FN], [LastName] AS [LN], [Age] FROM [Person] WHERE [Age] = @sqlinq_1", sql);
        }

        #endregion
    }
}
