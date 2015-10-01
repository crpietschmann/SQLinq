//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Dynamic;
using System;

namespace SQLinqTest.Dynamic
{
    [TestClass]
    public class DynamicSQLinqExpressionTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Compile_001()
        {
            var target = new DynamicSQLinqExpression(null);
            target.Compile(parameterNamePrefix: string.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Compile_002()
        {
            var target = new DynamicSQLinqExpression(null);
            target.Compile(parameterNamePrefix: null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Compile_003()
        {
            var target = new DynamicSQLinqExpression(null);
            target.Compile(parameterNamePrefix: " ");
        }
    }
}
