//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Collections.Generic;
using SQLinq.Compiler;
using SQLinq.Dynamic.Extensions;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqSubQueryJoinExpression : ISQLinqJoinExpression
    {
        /// <summary>
        /// Creates a new DynamicSQLinqSubQueryJoinExpression instance with the JoinOperator set to DynamicSQLinqJoinOperator.Inner ("INNER JOIN")
        /// </summary>
        /// <param name="query">The sub-query that will be joined</param>
        /// <param name="alias">The Alias to give the sub-query within the main query</param>
        /// <param name="clause">The Join clause</param>
        /// <param name="parameters">Any parameters necessary for the Join clause</param>
        public DynamicSQLinqSubQueryJoinExpression(ISQLinq query, string alias, string clause, params object[] parameters)
            : this(DialectProvider.Create(), query, alias, DynamicSQLinqJoinOperator.Inner, clause, parameters)
        { }

        /// <summary>
        /// Creates a new DynamicSQLinqSubQueryJoinExpression instance with the JoinOperator set to DynamicSQLinqJoinOperator.Inner ("INNER JOIN")
        /// </summary>
        /// <param name="query">The sub-query that will be joined</param>
        /// <param name="alias">The Alias to give the sub-query within the main query</param>
        /// <param name="clause">The Join clause</param>
        /// <param name="parameters">Any parameters necessary for the Join clause</param>
        public DynamicSQLinqSubQueryJoinExpression(ISqlDialect dialect, ISQLinq query, string alias, string clause, params object[] parameters)
            : this(dialect, query, alias, DynamicSQLinqJoinOperator.Inner, clause, parameters)
        { }

        /// <summary>
        /// Creates a new DynamicSQLinqSubQueryJoinExpression instance
        /// </summary>
        /// <param name="query">The sub-query that will be joined</param>
        /// <param name="alias">The Alias to give the sub-query within the main query</param>
        /// <param name="joinOperator">The JOIN operator</param>
        /// <param name="clause">The Join clause</param>
        /// <param name="parameters">Any parameters necessary for the Join clause</param>
        public DynamicSQLinqSubQueryJoinExpression(ISqlDialect dialect, ISQLinq query, string alias, DynamicSQLinqJoinOperator joinOperator, string clause, params object[] parameters)
        {
            this.Dialect = dialect;
            this.Query = query;
            this.Alias = alias;
            this.JoinOperator = joinOperator;
            this.Clause = clause;
            this.Parameters = parameters;
        }

        public ISqlDialect Dialect { get; private set; }

        public ISQLinq Query { get; set; }
        public string Alias { get; set; }
        public DynamicSQLinqJoinOperator JoinOperator { get; set; }
        public string Clause { get; set; }
        public object[] Parameters { get; set; }

        public SQLinqJoinResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            if (string.IsNullOrEmpty((parameterNamePrefix ?? string.Empty).Trim()))
            {
                throw new ArgumentException("parameterNamePrefix must be specified.", "parameterNamePrefix");
            }

            var subqueryResult = this.Query.ToSQL(existingParameterCount, parameterNamePrefix);
            var subquerySql = subqueryResult.ToQuery();

            var parameters = new Dictionary<string, object>();
            foreach (var p in subqueryResult.Parameters)
            {
                parameters.Add(p.Key, p.Value);
            }
            existingParameterCount += subqueryResult.Parameters.Count;

            var clause = this.Clause;
            for (var i = 0; i < this.Parameters.Length; i++)
            {
                existingParameterCount++;
                var key = this.Dialect.ParameterPrefix + parameterNamePrefix + existingParameterCount;
                clause = clause.Replace(this.Dialect.ParameterPrefix + i, key);
                parameters.Add(key, this.Parameters[i]);
            }

            var join = string.Format("{0} ({1}) AS {2} ON {3}", this.JoinOperator.ToSQL(), subquerySql, this.Alias, clause);

            return new SQLinqJoinResult(
                new string[] { join },
                parameters
                );
        }
    }
}
