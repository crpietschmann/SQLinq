using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Dynamic;
using System;

namespace SQLinqTest.Dynamic
{
    [TestClass]
    public class DynamicSQLinqSubQueryJoinExpressionTest
    {
        [TestMethod]
        public void DynamicSQLinqSubQueryJoinExpression_Constructor_001()
        {
            var query = SQLinq.SQLinq.Create("test");
            var alias = "name";
            var clause = "1 = 1";
            var param1 = "test";
            var param2 = 2;
            
            var target = new DynamicSQLinqSubQueryJoinExpression(query, alias, clause, param1, param2);

            Assert.AreEqual(DynamicSQLinqJoinOperator.Inner, target.JoinOperator);

            Assert.AreEqual(query, target.Query);
            Assert.AreEqual(alias, target.Alias);
            Assert.AreEqual(clause, target.Clause);
            Assert.AreEqual(param1, target.Parameters[0]);
            Assert.AreEqual(param2, target.Parameters[1]);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DynamicSQLinqSubQueryJoinExpression_ToSQL_001()
        {
            var query = SQLinq.SQLinq.Create("test");
            var alias = "name";
            var clause = "1 = 1";
            var param1 = "test";
            var param2 = 2;

            var target = new DynamicSQLinqSubQueryJoinExpression(query, alias, clause, param1, param2);

            target.ToSQL(parameterNamePrefix: null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DynamicSQLinqSubQueryJoinExpression_ToSQL_002()
        {
            var query = SQLinq.SQLinq.Create("test");
            var alias = "name";
            var clause = "1 = 1";
            var param1 = "test";
            var param2 = 2;

            var target = new DynamicSQLinqSubQueryJoinExpression(query, alias, clause, param1, param2);

            target.ToSQL(parameterNamePrefix: string.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DynamicSQLinqSubQueryJoinExpression_ToSQL_003()
        {
            var query = SQLinq.SQLinq.Create("test");
            var alias = "name";
            var clause = "1 = 1";
            var param1 = "test";
            var param2 = 2;

            var target = new DynamicSQLinqSubQueryJoinExpression(query, alias, clause, param1, param2);

            target.ToSQL(parameterNamePrefix: " ");
        }
    }
}
