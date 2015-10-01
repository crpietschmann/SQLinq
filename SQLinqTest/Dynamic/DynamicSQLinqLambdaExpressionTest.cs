using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Dynamic;
using System;

namespace SQLinqTest.Dynamic
{
    [TestClass]
    public class DynamicSQLinqLambdaExpressionTest
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Compile_001()
        {
            var target = new DynamicSQLinqLambdaExpression<string>(string.Empty, null);
            target.Compile(parameterNamePrefix: null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Compile_002()
        {
            var target = new DynamicSQLinqLambdaExpression<string>(string.Empty, null);
            target.Compile(parameterNamePrefix: string.Empty);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Compile_003()
        {
            var target = new DynamicSQLinqLambdaExpression<string>(string.Empty, null);
            target.Compile(parameterNamePrefix: " ");
        }
    }
}
