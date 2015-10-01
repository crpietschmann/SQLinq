//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;

namespace SQLinq
{
    public interface ISQLinqJoinExpression
    {
        /// <summary>
        /// Generates a DynamicSQLinqJoinResult object from the query join details specified for the IDynamicSQLinqJoinExpression object.
        /// </summary>
        /// <param name="existingParameterCount">Optional. The number of ISQLinq generated parameters that have already been generated using an ISQLinq instance that will be joined with this method calls results into a single query.</param>
        /// <returns></returns>
        SQLinqJoinResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix);
    }
}
