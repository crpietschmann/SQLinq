//Copyright (c) Chris Pietschmann 2014 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;
using SQLinq.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SQLinq
{
    public static class SQLinq
    {
        /// <summary>
        /// Creates a new SQLinq object for the Type of the object specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object that defines the Type to use for creating the SQLinq object instance for.</param>
        /// <returns></returns>
        public static SQLinq<T> Create<T>(T obj, string tableName, ISqlDialect dialect)
        {
            return new SQLinq<T>(tableName, dialect);
        }

        /// <summary>
        /// Creates a new SQLinq object for the Type of the object specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object that defines the Type to use for creating the SQLinq object instance for.</param>
        /// <returns></returns>
        public static SQLinq<T> Create<T>(T obj, string tableName)
        {
            // initialize the Default ISqlDialect
            var dialect = DialectProvider.Create();
            return Create<T>(obj, tableName, dialect);
        }

        /// <summary>
        /// Creates a new DynamicSQLinq object for the specified table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DynamicSQLinq Create(string tableName, ISqlDialect dialect)
        {
            return new DynamicSQLinq(dialect, tableName);
        }

        /// <summary>
        /// Creates a new DynamicSQLinq object for the specified table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DynamicSQLinq Create(string tableName)
        {
            // initialize the Default ISqlDialect
            var dialect = DialectProvider.Create();
            return Create(tableName, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SQLinqInsert<T> Insert<T>(T data, ISqlDialect dialect)
        {
            return new SQLinqInsert<T>(data, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SQLinqInsert<T> Insert<T>(T data)
        {
            // initialize the Default ISqlDialect
            var dialect = DialectProvider.Create();
            return Insert<T>(data, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object and table name.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SQLinqInsert<T> Insert<T>(T data, string tableName, ISqlDialect dialect)
        {
            return new SQLinqInsert<T>(data, tableName, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object and table name.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SQLinqInsert<T> Insert<T>(T data, string tableName)
        {
            // initialize the Default ISqlDialect
            var dialect = DialectProvider.Create();
            return Insert<T>(data, tableName, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SQLinqUpdate<T> Update<T>(T data, ISqlDialect dialect)
        {
            return new SQLinqUpdate<T>(data, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SQLinqUpdate<T> Update<T>(T data)
        {
            // initialize the Default ISqlDialect
            var dialect = DialectProvider.Create();
            return Update<T>(data, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object and table name.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SQLinqUpdate<T> Update<T>(T data, string tableName, ISqlDialect dialect)
        {
            return new SQLinqUpdate<T>(data, tableName, dialect);
        }

        /// <summary>
        /// Creates a new SQLinqInsert object for the specified Object and table name.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SQLinqUpdate<T> Update<T>(T data, string tableName)
        {
            // initialize the Default ISqlDialect
            var dialect = DialectProvider.Create();
            return Update<T>(data, tableName, dialect);
        }
    }

    /// <summary>
    /// Allows for Ad-Hoc SQL queries to be generated using LINQ in a stongly type manner, while also taking advantage of compile time validation.
    /// </summary>
    /// <typeparam name="T">The Type that contains a strongly typed reference of the scheme for the database table/view to be queried.</typeparam>
    public class SQLinq<T> : ISQLinq
    {
        /// <summary>
        /// Creates a new SQLinq object
        /// </summary>
        public SQLinq()
            : this(DialectProvider.Create())
        { }

        /// <summary>
        /// Creates a new SQLinq object
        /// </summary>
        public SQLinq(ISqlDialect dialect)
        {
            this.Expressions = new List<Expression>(); //= new List<Expression<Func<T, bool>>>();
            //this.JoinExpressions = new List<ISQLinqJoinExpression>();
            this.OrderByExpressions = new List<OrderByExpression>();
            this.Dialect = dialect;
        }

        /// <summary>
        /// Creates a new SQLinq object
        /// </summary>
        /// <param name="tableNameOverride">The database table name to use. This explicitly overrides any use of the SQLinqTable attribute.</param>
        public SQLinq(string tableNameOverride)
            : this(tableNameOverride, DialectProvider.Create())
        { }

        /// <summary>
        /// Creates a new SQLinq object
        /// </summary>
        /// <param name="tableNameOverride">The database table name to use. This explicitly overrides any use of the SQLinqTable attribute.</param>
        public SQLinq(string tableNameOverride, ISqlDialect dialect)
            : this(dialect)
        {
            this.TableNameOverride = tableNameOverride;
        }

        public string TableNameOverride { get; private set; }

        public ISqlDialect Dialect { get; private set; }

        //public List<Expression<Func<T, bool>>> Expressions { get; private set; }
        public List<Expression> Expressions { get; private set; }

        public Expression<Func<T, object>> Selector { get; private set; }
        //public List<ISQLinqJoinExpression> JoinExpressions { get; private set; }
        public int? TakeRecords { get; private set; }
        public int? SkipRecords { get; private set; }
        public List<OrderByExpression> OrderByExpressions { get; private set; }

        private bool? DistinctValue { get; set; }

        public class OrderByExpression
        {
            public Expression<Func<T, object>> Expression { get; set; }
            public bool Ascending { get; set; }
        }

        private ISQLinq Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distinct">Boolean value indicating whether 'DISTINCT' rows should be returned from the generated SQL. Default is True</param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> Distinct(bool distinct = true)
        {
            this.DistinctValue = distinct;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="take"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> Take(int take)
        {
            this.TakeRecords = take;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> Skip(int skip)
        {
            this.Dialect.AssertSkip(this);

            this.SkipRecords = skip;
            return this;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A SQLinqCount instance that can be used for generating a Count query.</returns>
        public SQLinqCount<T> Count()
        {
            return new SQLinqCount<T>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> Where(Expression<Func<T, bool>> expression)
        {
            this.Expressions.Add(expression);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> Where(Expression expression)
        {
            this.Expressions.Add(expression);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> Select(Expression<Func<T, object>> selector)
        {
            this.Selector = selector;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> OrderBy(Expression<Func<T, object>> keySelector)
        {
            if (this.OrderByExpressions.Count > 0)
            {
                this.OrderByExpressions.Clear();
            }
            this.OrderByExpressions.Add(new OrderByExpression { Expression = keySelector, Ascending = true });
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            if (this.OrderByExpressions.Count > 0)
            {
                this.OrderByExpressions.Clear();
            }
            this.OrderByExpressions.Add(new OrderByExpression { Expression = keySelector, Ascending = false });
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> ThenBy(Expression<Func<T, object>> keySelector)
        {
            this.OrderByExpressions.Add(new OrderByExpression { Expression = keySelector, Ascending = true });
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySelector"></param>
        /// <returns>The SQLinq instance to allow for method chaining.</returns>
        public SQLinq<T> ThenByDescending(Expression<Func<T, object>> keySelector)
        {
            this.OrderByExpressions.Add(new OrderByExpression { Expression = keySelector, Ascending = false });
            return this;
        }

        //public SQLinq<T> Join<TInner, TKey, TResult>(
        //    //SQLinq<TOuter> outer,
        //    SQLinq<TInner> inner,
        //    Expression<Func<T, TKey>> outerKeySelector,
        //    Expression<Func<TInner, TKey>> innerKeySelector,
        //    Expression<Func<T, TInner, TResult>> resultSelector)
        //{
        //    // http://msdn.microsoft.com/en-us/library/bb738634(v=vs.90).aspx
        //    // http://byatool.com/c/linq-join-method-and-how-to-use-it/

        //    inner.Parent = this;

        //    this.JoinExpressions.Add(new SQLinqJoinExpression<T, TInner, TKey, TResult> {
        //        Outer = this,
        //        Inner = inner,
        //        OuterKeySelector = outerKeySelector,
        //        InnerKeySelector = innerKeySelector,
        //        ResultSelector = resultSelector
        //    });
        //    return this;
        //}

        private string GetTableName(bool withAs = false)
        {
            var tableName = string.Empty;
            var tableAsName = string.Empty;

            if (!string.IsNullOrEmpty(this.TableNameOverride))
            {
                tableName = tableAsName = this.TableNameOverride;
            }
            else
            {
                // Get Table / View Name
                var type = typeof(T);
                tableName = type.Name;
                var tableAttribute = type.GetCustomAttributes(typeof(SQLinqTableAttribute), false).FirstOrDefault() as SQLinqTableAttribute;
                if (tableAttribute != null)
                {
                    // Table / View name is explicitly set, use that instead
                    tableName = tableAttribute.Table;
                }

                tableAsName = tableName;
                //if (withAs)
                //{
                //    var joins = this.JoinExpressions;
                //    if (joins.Count == 0)
                //    {
                //        if (this.Parent != null)
                //        {
                //            joins = this.Parent.JoinExpressions;
                //        }
                //    }
                //    if (joins.Count > 0)
                //    {
                //        var je = joins[0];
                //        ParameterExpression p = ((dynamic)je.OuterKeySelector).Parameters[0] as ParameterExpression;
                //        if (p.Type != typeof(T))
                //        {
                //            p = ((dynamic)je.InnerKeySelector).Parameters[0] as ParameterExpression;
                //        }
                //        tableAsName = p.Name;
                //    }
                //}
            }

            tableName = this.Dialect.ParseTableName(tableName);

            //if (tableAsName != null)
            //{
            //    if (!tableAsName.StartsWith("["))
            //    {
            //        tableAsName = string.Format("[{0}]", tableAsName);
            //    }
            //}

            //if (tableName == tableAsName)
            //{
            return tableName;
            //}
            //else
            //{
            //    return string.Format("{0} AS {1}", tableName, tableAsName);
            //}
        }

        /// <summary>
        /// Returns a SQLinqResult that contains the information for the query.
        /// </summary>
        /// <param name="existingParameterCount">Used to set the unique id's of the query parameters. The first query parameter will be 'existingParameterCount' plus one.</param>
        /// <returns></returns>
        public ISQLinqResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            int _parameterNumber = existingParameterCount;

            var type = typeof(T);
            var parameters = new Dictionary<string, object>();

            // Get Table / View Name
            var tableName = this.GetTableName(true);
            var subqueryAttr = type.GetCustomAttributes(typeof(SQLinqSubQueryAttribute), false).FirstOrDefault() as SQLinqSubQueryAttribute;
            if (subqueryAttr != null)
            {
                var tableQuery = subqueryAttr.GetQuery(parameters);
                _parameterNumber = parameters.Count;
                tableName = string.Format("({0}) AS {1}", tableQuery, tableName);
            }

            //// JOIN
            //var join = new List<string>();
            //foreach (var j in this.JoinExpressions)
            //{
            //    join.Add(j.Process(parameters));
            //}

            //// SELECT
            var selectResult = this.ToSQL_Select(_parameterNumber, parameterNamePrefix, parameters);
            _parameterNumber = existingParameterCount + parameters.Count;

            // WHERE
            var whereResult = this.ToSQL_Where(_parameterNumber, parameterNamePrefix, parameters);
            _parameterNumber = existingParameterCount + parameters.Count;

            // ORDER BY
            var orderbyResult = this.ToSQL_OrderBy(_parameterNumber, parameterNamePrefix, parameters);
            _parameterNumber = existingParameterCount + parameters.Count;

            return new SQLinqSelectResult(this.Dialect)
            {
                Select = selectResult.Select.ToArray(),
                Distinct = this.DistinctValue,
                Take = this.TakeRecords,
                Skip = this.SkipRecords,
                Table = tableName,
                //Join = join.ToArray(),
                Where = whereResult == null ? null : whereResult.SQL,
                OrderBy = orderbyResult.Select.ToArray(),
                Parameters = parameters
            };
        }

        private SqlExpressionCompilerResult ToSQL_Where(int parameterNumber, string parameterNamePrefix, IDictionary<string, object> parameters)
        {
            SqlExpressionCompilerResult whereResult = null;
            if (this.Expressions.Count > 0)
            {
                whereResult = SqlExpressionCompiler.Compile(this.Dialect, parameterNumber, parameterNamePrefix, this.Expressions);
                foreach (var item in whereResult.Parameters)
                {
                    parameters.Add(item.Key, item.Value);
                }
            }
            return whereResult;
        }

        private SqlExpressionCompilerSelectorResult ToSQL_Select(int parameterNumber, string parameterNamePrefix, IDictionary<string, object> parameters)
        {
            var selectResult = SqlExpressionCompiler.CompileSelector(this.Dialect, parameterNumber, parameterNamePrefix, this.Selector);
            foreach (var item in selectResult.Parameters)
            {
                parameters.Add(item.Key, item.Value);
            }
            if (selectResult.Select.Count == 0)
            {
                var props = typeof(T).GetProperties();
                var usesSQLinqColumn = props.Where(d => d.GetCustomAttributes(typeof(SQLinqColumnAttribute), false).Length > 0).Count() > 0;
                if (usesSQLinqColumn)
                {
                    foreach (var p in props)
                    {
                        var selectName = SqlExpressionCompiler.GetMemberColumnName(p, this.Dialect);
                        var asName = this.Dialect.ParseColumnName(p.Name);
                        if (selectName == asName)
                        {
                            selectResult.Select.Add(selectName);
                        }
                        else
                        {
                            selectResult.Select.Add(string.Format("{0} AS {1}", selectName, asName));
                        }
                    }
                }
                else
                {
                    selectResult.Select.Add("*");
                }
            }
            return selectResult;
        }

        private SqlExpressionCompilerSelectorResult ToSQL_OrderBy(int parameterNumber, string parameterNamePrefix, IDictionary<string, object> parameters)
        {
            var orderbyResult = new SqlExpressionCompilerSelectorResult();

            for (var i = 0; i < this.OrderByExpressions.Count; i++)
            {
                var r = SqlExpressionCompiler.CompileSelector(this.Dialect, parameterNumber, parameterNamePrefix, this.OrderByExpressions[i].Expression);
                foreach (var s in r.Select)
                {
                    orderbyResult.Select.Add(s);
                }
                foreach (var p in r.Parameters)
                {
                    orderbyResult.Parameters.Add(p.Key, p.Value);
                }
            }
            foreach (var item in orderbyResult.Parameters)
            {
                parameters.Add(item.Key, item.Value);
            }
            for (var i = 0; i < this.OrderByExpressions.Count; i++)
            {
                if (!this.OrderByExpressions[i].Ascending)
                {
                    orderbyResult.Select[i] = orderbyResult.Select[i] + " DESC";
                }
            }

            return orderbyResult;
        }
    }
}
