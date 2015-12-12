//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;
using System.Collections.Generic;

namespace SQLinq.Dynamic
{
    public class DynamicSQLinqInsert : ISQLinqInsert
    {
        public DynamicSQLinqInsert(IDictionary<string, object> data, string tableName)
        {
            this.Table = tableName;
            this.Data = data;
        }

        public string Table { get; set; }
        public IDictionary<string, object> Data { get; set; }

        public ISQLinqResult ToSQL(int existingParameterCount = 1, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            int _parameterNumber = existingParameterCount;

            var fields = new Dictionary<string, string>();
            var parameters = new Dictionary<string, object>();

            foreach (var item in this.Data)
            {
                var fieldName = item.Key;
                var parameterValue = item.Value;
                var parameterName = DialectProvider.Dialect.ParameterPrefix + parameterNamePrefix + _parameterNumber.ToString();

                fields.Add(fieldName, parameterName);
                parameters.Add(parameterName, parameterValue);

                _parameterNumber++;
            }

            return new SQLinqInsertResult
            {
                Table = this.Table,
                Fields = fields,
                Parameters = parameters
            };
        }
    }
}
