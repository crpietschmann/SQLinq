//Copyright (c) Chris Pietschmann 2014 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license
using SQLinq.Compiler;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqCount : ISQLinqCount
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">The DynamicSQLinq instance to use as the root for the Count query. This is passed by reference.</param>
        public DynamicSQLinqCount(DynamicSQLinq query)
        {
            this.Query = query;
        }

        public DynamicSQLinq Query { get; private set; }

        public ISQLinqResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            var result = (SQLinqSelectResult)this.Query.ToSQL(existingParameterCount, parameterNamePrefix);

            const string selectCount = "COUNT(1)";
            result.Select = new string[] { selectCount };

            return result;
        }
    }
}
