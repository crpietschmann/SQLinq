//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SQLinq.Compiler;

namespace SQLinq.Dynamic
{
    /// <summary>
    /// Allows for dynamic ad-hoc SQL code generation with a similar API to LINQ while also allowing for LINQ expressions to be used via Lambda expressions.
    /// </summary>
    public class DynamicSQLinq : ISQLinq
    {
        public DynamicSQLinq(string tableName)
            : this(DialectProvider.Create(), tableName)
        { }

        public DynamicSQLinq(ISqlDialect dialect, string tableName)
        {
            this.TableName = tableName;
            this.SelectFields = new List<string>();
            this.JoinClauses = new List<ISQLinqJoinExpression>();
            this.WhereClauses = new DynamicSQLinqExpressionCollection(dialect);
            this.HavingClauses = new DynamicSQLinqExpressionCollection(dialect);
            this.GroupByFields = new List<string>();
            this.OrderByFields = new List<string>();

            this.Dialect = dialect;
        }

        public ISqlDialect Dialect { get; private set; }

        /// <summary>
        /// The database table/view name to use for the generated query.
        /// </summary>
        public string TableName { get; private set; }

        private int? TakeCount { get; set; }
        private int? SkipCount { get; set; }
        
        private bool? DistinctValue { get; set; }

        private List<string> SelectFields { get; set; }
        private List<string> GroupByFields { get; set; }
        private List<string> OrderByFields { get; set; }
        private DynamicSQLinqExpressionCollection WhereClauses { get; set; }
        private DynamicSQLinqExpressionCollection HavingClauses { get; set; }
        private List<ISQLinqJoinExpression> JoinClauses { get; set; }

        /// <summary>
        /// Allows for SQL 'SELECT DISTINCT' to be performed.
        /// </summary>
        /// <param name="distinct">Boolean value indicating whether 'DISTINCT' rows should be returned from the generated SQL. Default is True</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Distinct(bool distinct = true)
        {
            this.DistinctValue = distinct;
            return this;
        }

        /// <summary>
        /// Allows for SQL 'SELECT' fields to be specified.
        /// </summary>
        /// <param name="fields">The table/view fields to select.</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Select(params string[] fields)
        {
            this.SelectFields.AddRange(fields);
            return this;
        }

        /// <summary>
        /// Allows for SQL 'SELECT' fields to be specified from the specified Table/View
        /// </summary>
        /// <param name="tableName">The table/view name to select fields from.</param>
        /// <param name="fields">The fields to select.</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq SelectTable(string tableName, params string[] fields)
        {
            foreach (var f in fields)
            {
                this.SelectFields.Add(string.Format("{0}.{1}", tableName, f));
            }
            return this;
        }

        /// <summary>
        /// Allows for SQL 'GROUP BY' fields to be specified.
        /// </summary>
        /// <param name="fields">The table/view fields to 'GROUP BY'</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq GroupBy(params string[] fields)
        {
            this.GroupByFields.AddRange(fields);
            return this;
        }

        /// <summary>
        /// Specifies the number of results to return from the generated query.
        /// </summary>
        /// <param name="takeCount"></param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Take(int takeCount)
        {
            this.TakeCount = takeCount;
            return this;
        }

        /// <summary>
        /// Specifies the number of matching result from the the generated query to skip before return results.
        /// </summary>
        /// <param name="skipCount"></param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Skip(int skipCount)
        {
            this.SkipCount = skipCount;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A SQLinqCount instance that can be used for generating a Count query.</returns>
        public DynamicSQLinqCount Count()
        {
            return new DynamicSQLinqCount(this);
        }

        /// <summary>
        /// Specifies a SQL 'WHERE' clause to use with the generated query using a collection of LINQ expressions.
        /// </summary>
        /// <param name="expressionCollection"></param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Where(DynamicSQLinqExpressionCollection expressionCollection)
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
        public DynamicSQLinq Where(string clause, params object[] parameters)
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
        public DynamicSQLinq Where<T>(string fieldName, Expression<Func<T, bool>> expression)
        {
            this.WhereClauses.Add(new DynamicSQLinqLambdaExpression<T>(this.Dialect, fieldName, expression));
            return this;
        }

        /// <summary>
        /// Specifies a SQL 'HAVING' clause to use with the generated query.
        /// </summary>
        /// <param name="clause">The SQL code to use.</param>
        /// <param name="parameters">The parameters to use for the specified 'HAVING' clause.</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Having(string clause, params object[] parameters)
        {
            this.HavingClauses.Add(new DynamicSQLinqExpression(this.Dialect, clause, parameters));
            return this;
        }

        /// <summary>
        /// Specifies a SQL 'HAVING' clause to use with the generated query using a LINQ expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName">The Table/View Column name</param>
        /// <param name="expression">The LINQ / Lambda expression to use</param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq Having<T>(string fieldName, Expression<Func<T, bool>> expression)
        {
            this.HavingClauses.Add(new DynamicSQLinqLambdaExpression<T>(this.Dialect, fieldName, expression));
            return this;
        }

        /// <summary>
        /// Specifies a SQL 'JOIN' to use with the generated query.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="clause"></param>
        /// <returns></returns>
        public DynamicSQLinq Join(string tableName, string clause, params object[] parameters)
        {
            this.JoinClauses.Add(new DynamicSQLinqJoinExpression(this.Dialect, tableName, clause, parameters));
            return this;
        }

        /// <summary>
        /// Specifies a SQL join to use with the generated query.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="clause"></param>
        /// <returns></returns>
        public DynamicSQLinq Join(string tableName, DynamicSQLinqJoinOperator joinOperator, string clause, params object[] parameters)
        {
            this.JoinClauses.Add(new DynamicSQLinqJoinExpression(this.Dialect, tableName, joinOperator, clause, parameters));
            return this;
        }

        public DynamicSQLinq Join(ISQLinq subquery, string subqueryAlias, DynamicSQLinqJoinOperator joinOperator, string clause, params object[] parameters)
        {
            this.JoinClauses.Add(new DynamicSQLinqSubQueryJoinExpression(this.Dialect, subquery, subqueryAlias, joinOperator, clause, parameters));
            return this;
        }

        /// <summary>
        /// Specifies a SQL 'LEFT JOIN' to use with the generated query.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="clause"></param>
        /// <returns></returns>
        public DynamicSQLinq LeftJoin(string tableName, string clause, params object[] parameters)
        {
            return this.Join(tableName, DynamicSQLinqJoinOperator.Left, clause, parameters);
        }

        /// <summary>
        /// Specifies a SQL 'RIGHT JOIN' to use with the generated query.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="clause"></param>
        /// <returns></returns>
        public DynamicSQLinq RightJoin(string tableName, string clause, params object[] parameters)
        {
            return this.Join(tableName, DynamicSQLinqJoinOperator.Right, clause, parameters);
        }

        /// <summary>
        /// Specifies a SQL 'FULL JOIN' to use with the generated query.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="clause"></param>
        /// <returns></returns>
        public DynamicSQLinq FullJoin(string tableName, string clause, params object[] parameters)
        {
            return this.Join(tableName, DynamicSQLinqJoinOperator.Full, clause, parameters);
        }

        /// <summary>
        /// Specifies columns to sort the query result.
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <returns>The DynamicSQLinq instance to allow for method chaining.</returns>
        public DynamicSQLinq OrderBy(params string[] fieldNames)
        {
            this.OrderByFields.AddRange(fieldNames);
            return this;
        }

        /// <summary>
        /// Specifies columns to sort the query result in Descending order.
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public DynamicSQLinq OrderByDescending(params string[] fieldNames)
        {
            foreach (var f in fieldNames)
            {
                var orderByField = f;
                var lf = f.ToLowerInvariant();
                if (!(lf.EndsWith(" desc") || lf.EndsWith(" descending")))
                {
                    orderByField = f + " DESC";
                }

                this.OrderByFields.Add(orderByField);
            }
            return this;
        }

        /// <summary>
        /// Generated SQL code and query parameters from the specified query details.
        /// </summary>
        /// <param name="existingParameterCount"></param>
        /// <returns></returns>
        public ISQLinqResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            var parameterCount = existingParameterCount;

            var result = new SQLinqSelectResult(this.Dialect);
            result.Parameters = new Dictionary<string, object>();

            result.Take = this.TakeCount;
            result.Skip = this.SkipCount;

            result.Distinct = this.DistinctValue;

            // ****************************************************
            // **** FROM ******************************************
            result.Table = this.TableName;


            // ****************************************************
            // **** SELECT ****************************************
            if (this.SelectFields.Count == 0)
            {
                result.Select = new string[] { "*" };
            }
            else
            {
                result.Select = this.SelectFields.ToArray();
            }

            
            // ****************************************************
            // **** JOIN ******************************************
            var joins = new List<string>();
            foreach (var jc in this.JoinClauses)
            {
                var r = jc.ToSQL(parameterCount, parameterNamePrefix);
                foreach (var j in r.Join)
                {
                    joins.Add(j);
                }
                foreach (var i in r.Parameters)
                {
                    result.Parameters.Add(i.Key, i.Value);
                }
            }
            result.Join = joins.ToArray();

            parameterCount = existingParameterCount + result.Parameters.Count;
            
            // ****************************************************
            // **** WHERE *****************************************
            var compiledWhere = this.WhereClauses.Compile(parameterCount, parameterNamePrefix);
            result.Where = compiledWhere.SQL;
            foreach (var p in compiledWhere.Parameters)
            {
                result.Parameters.Add(p.Key, p.Value);
            }

            parameterCount = existingParameterCount + result.Parameters.Count;

            // ****************************************************
            // **** GROUP BY **************************************
            if (this.GroupByFields.Count > 0)
            {
                result.GroupBy = this.GroupByFields.ToArray();
            }

            // ****************************************************
            // **** HAVING ****************************************
            var compiledHaving = this.HavingClauses.Compile(parameterCount, parameterNamePrefix);
            result.Having = compiledHaving.SQL;
            foreach (var p in compiledHaving.Parameters)
            {
                result.Parameters.Add(p.Key, p.Value);
            }

            // ****************************************************
            // **** ORDER BY **************************************
            if (this.OrderByFields.Count > 0)
            {
                result.OrderBy = this.OrderByFields.ToArray();
            }


            return result;
        }
    }
}
