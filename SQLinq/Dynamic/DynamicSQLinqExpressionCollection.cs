//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;
using System.Text;
using SQLinq.Compiler;
using System;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqExpressionCollection : List<IDynamicSQLinqExpression>, IDynamicSQLinqExpression
    {
        public DynamicSQLinqExpressionCollection()
            : this(DialectProvider.Create())
        { }

        public DynamicSQLinqExpressionCollection(ISqlDialect dialect)
            : this(dialect, DynamicSQLinqWhereOperator.And)
        { }

        public DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator whereOperator)
            : this(DialectProvider.Create(), whereOperator)
        { }

        public DynamicSQLinqExpressionCollection(ISqlDialect dialect, DynamicSQLinqWhereOperator whereOperator)
        {
            this.Dialect = dialect;
            this.WhereOperator = whereOperator;
        }

        public ISqlDialect Dialect { get; private set; }

        public DynamicSQLinqWhereOperator WhereOperator { get; set; }

        public SqlExpressionCompilerResult Compile(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            if (string.IsNullOrEmpty(parameterNamePrefix))
            {
                throw new ArgumentException("parameterNamePrefix must be specified.", "parameterNamePrefix");
            }

            var sb = new StringBuilder();
            var parameters = new Dictionary<string, object>();
            var newParameterCount = existingParameterCount;

            var count = this.Count;
            for (var i = 0; i < count; i++)
            {
                var compiled = this[i].Compile(newParameterCount, parameterNamePrefix);

                if (i > 0)
                {
                    if (this.WhereOperator == DynamicSQLinqWhereOperator.Or)
                    {
                        sb.Append(" OR ");
                    }
                    else
                    {
                        sb.Append(" AND ");
                    }
                }
                if (count > 1)
                {
                    sb.Append("(");
                }

                sb.Append(compiled.SQL);

                if (count > 1)
                {
                    sb.Append(")");
                }

                foreach (var p in compiled.Parameters)
                {
                    parameters.Add(p.Key, p.Value);
                }

                newParameterCount = existingParameterCount + parameters.Count;
            }

            return new SqlExpressionCompilerResult(sb.ToString(), parameters);
        }

        public DynamicSQLinqExpressionCollection Add(string clause, params object[] parameters)
        {
            this.Add(new DynamicSQLinqExpression(this.Dialect, clause, parameters));
            return this;
        }
    }
}
