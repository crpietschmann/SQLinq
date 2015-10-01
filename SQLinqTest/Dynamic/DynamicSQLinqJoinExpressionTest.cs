//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Dynamic;
using System;

namespace SQLinqTest.Dynamic
{
    [TestClass]
    public class DynamicSQLinqJoinExpressionTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ToSQL_001()
        {
            var target = new DynamicSQLinqJoinExpression(null, null, null);
            target.ToSQL(parameterNamePrefix: null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ToSQL_002()
        {
            var target = new DynamicSQLinqJoinExpression(null, null, null);
            target.ToSQL(parameterNamePrefix: string.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ToSQL_003()
        {
            var target = new DynamicSQLinqJoinExpression(null, null, null);
            target.ToSQL(parameterNamePrefix: " ");
        }
    }
}
