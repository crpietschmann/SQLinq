//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
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
        {
            this.Clause = clause;
            this.Parameters = args;
        }

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
                var paramName = string.Format("@{0}{1}", parameterNamePrefix, existingParameterCount.ToString());

                sql = sql.Replace("@" + i, paramName);
                parameters.Add(paramName, this.Parameters[i]);
            }

            return new SqlExpressionCompilerResult(sql, parameters);
        }
    }
}
