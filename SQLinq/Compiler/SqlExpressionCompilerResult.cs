//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;

namespace SQLinq.Compiler
{
    public class SqlExpressionCompilerResult : ISqlExpressionResult
    {
        public SqlExpressionCompilerResult()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        public SqlExpressionCompilerResult(string sql, IDictionary<string, object> parameters)
        {
            this.SQL = sql;
            this.Parameters = parameters;
        }

        public string SQL { get; set; }
        public IDictionary<string, object> Parameters { get; set; }
    }
}
