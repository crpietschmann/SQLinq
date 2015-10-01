using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Dynamic;
using SQLinq.Dynamic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLinqTest.Dynamic.Extensions
{
    [TestClass]
    public class DynamicSQLinqJoinOperatorExtensionsTest
    {
        [TestMethod, ExpectedException(typeof(Exception))]
        public void ToSQL_001()
        {
            var actual = DynamicSQLinqJoinOperatorExtensions.ToSQL((DynamicSQLinqJoinOperator)4);
        }

        [TestMethod]
        public void ToSQL_002()
        {
            var actual = DynamicSQLinqJoinOperatorExtensions.ToSQL(DynamicSQLinqJoinOperator.Inner);
            Assert.AreEqual("JOIN", actual);
        }

        [TestMethod]
        public void ToSQL_003()
        {
            var actual = DynamicSQLinqJoinOperatorExtensions.ToSQL(DynamicSQLinqJoinOperator.Left);
            Assert.AreEqual("LEFT JOIN", actual);
        }

        [TestMethod]
        public void ToSQL_004()
        {
            var actual = DynamicSQLinqJoinOperatorExtensions.ToSQL(DynamicSQLinqJoinOperator.Right);
            Assert.AreEqual("RIGHT JOIN", actual);
        }

        [TestMethod]
        public void ToSQL_005()
        {
            var actual = DynamicSQLinqJoinOperatorExtensions.ToSQL(DynamicSQLinqJoinOperator.Full);
            Assert.AreEqual("FULL JOIN", actual);
        }
    }
}
