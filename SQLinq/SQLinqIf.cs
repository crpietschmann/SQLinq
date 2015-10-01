//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;

namespace SQLinq
{
    public class SQLinqIf : ISQLinqIf
    {
        /// <summary>
        /// Createa a new SQLinqIf object
        /// </summary>
        /// <param name="operator">The operator used for evaluating the IF clause</param>
        /// <param name="ifClause"></param>
        public SQLinqIf(SQLinqIfOperator @operator, ISQLinq ifClause)
        {
            this.Operator = @operator;
            this.If = ifClause;
        }

        /// <summary>
        /// Createa a new SQLinqIf object
        /// </summary>
        /// <param name="clause">The SQL code to use for the IF clause</param>
        public SQLinqIf(string clause)
            : this(SQLinqIfOperator.None, clause)
        { }

        /// <summary>
        /// Createa a new SQLinqIf object
        /// </summary>
        /// <param name="operator">The operator used for evaluating the IF clause</param>
        /// <param name="ifClause"></param>
        public SQLinqIf(SQLinqIfOperator @operator, string ifClause)
        {
            this.Operator = @operator;
            this.If = ifClause;
        }       

        /// <summary>
        /// The clause used to evaluate the IF condition
        /// </summary>
        public object If { get; protected set; }

        /// <summary>
        /// The operator to use for the evaluation of the IF clause
        /// </summary>
        public SQLinqIfOperator Operator { get; set; }

        /// <summary>
        /// Specifies the query to execute when the "IF" clause evaluates to TRUE
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ISQLinq Then { get; set; }

        /// <summary>
        /// Specifies the query to execute when the "IF" clause evaluates to FALSE
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ISQLinq Else { get; set; }

        public ISQLinqResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            var paramCount = existingParameterCount;

            var result = new SQLinqIfResult
            {
                Operator = this.Operator
            };

            if (this.If is string)
            {
                result.If = this.If as string;
            }
            else
            {
                var ifResult = ((ISQLinq)this.If).ToSQL(paramCount, parameterNamePrefix);
                result.If = ifResult.ToQuery();

                foreach (var p in ifResult.Parameters)
                {
                    result.Parameters.Add(p);
                }

                paramCount = existingParameterCount + result.Parameters.Count;
            }

            if (this.Then != null)
            {
                var thenResult = this.Then.ToSQL(paramCount, parameterNamePrefix);
                result.Then = thenResult.ToQuery();

                foreach (var p in thenResult.Parameters)
                {
                    result.Parameters.Add(p);

                }
                paramCount = existingParameterCount + result.Parameters.Count;
            }

            if (this.Else != null)
            {
                var elseResult = this.Else.ToSQL(paramCount, parameterNamePrefix);
                result.Else = elseResult.ToQuery();

                foreach (var p in elseResult.Parameters)
                {
                    result.Parameters.Add(p);
                }
            }

            return result;
        }
    }
}
