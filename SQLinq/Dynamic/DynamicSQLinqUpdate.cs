//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqUpdate : ISQLinqUpdate
    {
        public DynamicSQLinqUpdate(IDictionary<string, object> data, string tableName)
            : this(DialectProvider.Create(), data, tableName)
        { }

        public DynamicSQLinqUpdate(ISqlDialect dialect, IDictionary<string, object> data, string tableName)
        {
            this.Dialect = dialect;
            this.Table = tableName;
            this.Data = data;

            this.WhereClauses = new DynamicSQLinqExpressionCollection(dialect);
        }

        public ISqlDialect Dialect { get; private set; }

        public string Table { get; set; }
        public IDictionary<string, object> Data { get; set; }

        private DynamicSQLinqExpressionCollection WhereClauses { get; set; }

        /// <summary>
        /// Specifies a SQL 'WHERE' clause to use with the generated query using a collection of LINQ expressions.
        /// </summary>
        /// <param name="expressionCollection"></param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinqUpdate Where(DynamicSQLinqExpressionCollection expressionCollection)
        {
            this.WhereClauses.Add(expressionCollection);
            return this;
        }

        /// <summary>
        /// Specifies a SQL 'WHERE' clause to use with the generated query.
        /// </summary>
        /// <param name="clause">The SQL code to use.</param>
        /// <param name="parameters">The parameters to use for the specified 'WHERE' clause.</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinqUpdate Where(string clause, params object[] parameters)
        {
            this.WhereClauses.Add(new DynamicSQLinqExpression(this.Dialect, clause, parameters));
            return this;
        }

        /// <summary>
        /// Specifies a SQL 'WHERE' clause to use with the generated query using a LINQ expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName">The Table/View Column name</param>
        /// <param name="expression">The LINQ / Lambda expression to use</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinqUpdate Where<T>(string fieldName, Expression<Func<T, bool>> expression)
        {
            this.WhereClauses.Add(new DynamicSQLinqLambdaExpression<T>(this.Dialect, fieldName, expression));
            return this;
        }

        public ISQLinqResult ToSQL(int existingParameterCount = 1, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            int _parameterNumber = existingParameterCount;

            var fields = new Dictionary<string, string>();
            var parameters = new Dictionary<string, object>();

            foreach (var item in this.Data)
            {
                var fieldName = item.Key;
                var parameterValue = item.Value;
                var parameterName = this.Dialect.ParameterPrefix + parameterNamePrefix + _parameterNumber.ToString();

                fields.Add(fieldName, parameterName);
                parameters.Add(parameterName, parameterValue);

                _parameterNumber++;
            }
            var parameterCount = existingParameterCount + parameters.Count - 1;

            // ****************************************************
            // **** FROM ******************************************
            var tableName = this.Dialect.ParseTableName(this.Table);

            // ****************************************************
            // **** WHERE *****************************************
            var compiledWhere = this.WhereClauses.Compile(parameterCount, parameterNamePrefix);
            foreach (var p in compiledWhere.Parameters)
            {
                parameters.Add(p.Key, p.Value);
            }


            return new SQLinqUpdateResult(this.Dialect)
            {
                Table = tableName,
                Where = compiledWhere.SQL,
                Fields = fields,
                Parameters = parameters
            };
        }
    }
}
