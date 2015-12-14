//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Collections.Generic;
using SQLinq.Compiler;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqExpression : IDynamicSQLinqExpression
    {
        public DynamicSQLinqExpression(string clause, params object[] args)
            : this(DialectProvider.Create(), clause, args)
        { }

        public DynamicSQLinqExpression(ISqlDialect dialect, string clause, params object[] args)
        {
            this.Clause = clause;
            this.Parameters = args;
            this.Dialect = dialect;
        }

        public ISqlDialect Dialect { get; private set; }

        public string Clause { get; set; }
        public object[] Parameters { get; set; }

        public SqlExpressionCompilerResult Compile(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            if (string.IsNullOrEmpty((parameterNamePrefix ?? string.Empty).Trim()))
            {
                throw new ArgumentException("parameterNamePrefix must be specified.", "parameterNamePrefix");
            }

            var sql = this.Clause;
            var parameters = new Dictionary<string, object>();

            for (var i = 0; i < this.Parameters.Length; i++)
            {
                existingParameterCount++;
                var paramName = string.Format("{0}{1}{2}", this.Dialect.ParameterPrefix, parameterNamePrefix, existingParameterCount.ToString());

                // replace SQL dialect specific parameter name placeholder
                sql = sql.Replace(this.Dialect.ParameterPrefix + i, paramName);

                // Replace "@0" format placeholder
                // This allows for this placeholder format, plus SQL dialect specific ones to be supported.
                // This allows for "@0" to be standardized as the supported parameter name placeholder
                // regardless of the SQL dialect being used.
                sql = sql.Replace("@" + i, paramName);

                parameters.Add(paramName, this.Parameters[i]);
            }

            return new SqlExpressionCompilerResult(sql, parameters);
        }
    }
}
