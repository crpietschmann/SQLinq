//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Collections.Generic;
using SQLinq.Compiler;
using SQLinq.Dynamic.Extensions;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqJoinExpression : ISQLinqJoinExpression
    {
        /// <summary>
        /// Creates a new DynamicSQLinqJoinExpression instance with the JoinOperator set to DynamicSQLinqJoinOperator.Inner ("INNER JOIN")
        /// </summary>
        /// <param name="tableName">The database Table / View to Join</param>
        /// <param name="clause">The Join clause</param>
        /// <param name="parameters">Any parameters necessary for the Join clause</param>
        public DynamicSQLinqJoinExpression(string tableName, string clause, object[] parameters)
            : this(DialectProvider.Create(), tableName, clause, parameters)
        { }

        /// <summary>
        /// Creates a new DynamicSQLinqJoinExpression instance with the JoinOperator set to DynamicSQLinqJoinOperator.Inner ("INNER JOIN")
        /// </summary>
        /// <param name="tableName">The database Table / View to Join</param>
        /// <param name="clause">The Join clause</param>
        /// <param name="parameters">Any parameters necessary for the Join clause</param>
        public DynamicSQLinqJoinExpression(ISqlDialect dialect, string tableName, string clause, object[] parameters)
            : this(dialect, tableName, DynamicSQLinqJoinOperator.Inner, clause, parameters)
        { }

        /// <summary>
        /// Creates a new DynamicSQLinqJoinExpression instance
        /// </summary>
        /// <param name="tableName">The database Table / View to Join</param>
        /// <param name="joinOperator">The JOIN operator</param>
        /// <param name="clause">The Join clause</param>
        /// <param name="parameters">Any parameter values necessary for the Join clause</param>
        public DynamicSQLinqJoinExpression(ISqlDialect dialect, string tableName, DynamicSQLinqJoinOperator joinOperator, string clause, object[] parameters)
        {
            this.Dialect = dialect;
            this.Table = tableName;
            this.JoinOperator = joinOperator;
            this.Clause = clause;
            this.Parameters = parameters;
        }

        public ISqlDialect Dialect { get; private set; }

        /// <summary>
        /// The database Table / View to Join
        /// </summary>
        public string Table { get; set; }
        
        /// <summary>
        /// The JOIN operator
        /// </summary>
        public DynamicSQLinqJoinOperator JoinOperator { get; set; }
        
        /// <summary>
        /// The Join clause
        /// </summary>
        public string Clause { get; set; }

        /// <summary>
        /// All necessary Join parameter values
        /// </summary>
        public object[] Parameters { get; set; }

        public SQLinqJoinResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            if (string.IsNullOrEmpty((parameterNamePrefix ?? string.Empty).Trim()))
            {
                throw new ArgumentException("parameterNamePrefix must be specified.", "parameterNamePrefix");
            }
            
            var parameters = new Dictionary<string, object>();
            
            var clause = this.Clause;
            for (var i = 0; i < this.Parameters.Length; i++)
            {
                existingParameterCount++;
                var key = this.Dialect.ParameterPrefix + parameterNamePrefix + existingParameterCount;
                clause = clause.Replace(this.Dialect.ParameterPrefix + i, key);
                parameters.Add(key, this.Parameters[i]);
            }

            var join = string.Format("{0} {1} ON {2}", this.JoinOperator.ToSQL(), this.Table, clause);

            return new SQLinqJoinResult(
                new string[] { join },
                parameters
                );
        }
    }
}
