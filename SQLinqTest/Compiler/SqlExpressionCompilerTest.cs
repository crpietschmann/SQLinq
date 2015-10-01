using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Compiler;
using System;
using System.Linq.Expressions;

namespace SQLinqTest.Compiler
{
    [TestClass]
    public class SqlExpressionCompilerTest
    {
        [TestMethod, ExpectedException(typeof(Exception))]
        public void CheckRequiredProperties_001()
        {
            var target = new SqlExpressionCompiler(parameterNamePrefix: string.Empty);
            target.Compile((Expression)null);            
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void CheckRequiredProperties_002()
        {
            var target = new SqlExpressionCompiler(parameterNamePrefix: " ");
            target.Compile((Expression)null);
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void CheckRequiredProperties_003()
        {
            var target = new SqlExpressionCompiler(parameterNamePrefix: null);
            target.Compile((Expression)null);
        }
    }
}
