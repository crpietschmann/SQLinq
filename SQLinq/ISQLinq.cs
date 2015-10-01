//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;

namespace SQLinq
{
    public interface ISQLinq
    {
        /// <summary>
        /// Generates a SQLinqResult object from the query details specified for the ISQLinq object.
        /// </summary>
        /// <param name="existingParameterCount">Optional. The number of SQLinq generated parameters that have already been generated using a different ISQLinq instance that will be joined with this method calls results into a single query.</param>
        /// <returns></returns>
        ISQLinqResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix);

        //List<ISQLinqJoinExpression> JoinExpressions { get; } // Need to remove this from this interface, it only pertains to SQLinq<T> not DynamicSQLinq
    }
}
