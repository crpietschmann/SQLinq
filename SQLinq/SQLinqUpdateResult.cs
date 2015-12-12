//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace SQLinq
{
    public class SQLinqUpdateResult : ISQLinqResult
    {
        public SQLinqUpdateResult(ISqlDialect dialect)
        {
            this.Dialect = dialect;
        }

        public ISqlDialect Dialect { get; private set; }

        public string Table { get; set; }

        public IDictionary<string, string> Fields { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public string Where { get; set; }

        public string ToQuery()
        {
            if (string.IsNullOrWhiteSpace((this.Table ?? string.Empty).Trim()))
            {
                throw new ArgumentException("SQLinqInsertResult.Table is required to have a value.", "Table");
            }
            if (this.Fields == null || this.Fields.Count == 0)
            {
                throw new ArgumentNullException("Fields");
            }
            if (this.Parameters == null || this.Parameters.Count == 0)
            {
                throw new ArgumentNullException("Parameters");
            }

            var fieldParameterList = new StringBuilder();

            var isFirst = true;
            foreach (var f in this.Fields)
            {
                if (!isFirst)
                {
                    fieldParameterList.Append(", ");
                }
                else
                {
                    isFirst = false;
                }

                fieldParameterList.Append(this.Dialect.ParseColumnName(f.Key));

                fieldParameterList.Append(" = ");
                fieldParameterList.Append(f.Value);
            }

            if (string.IsNullOrWhiteSpace(this.Where))
            {
                return string.Format("UPDATE {0} SET {1}", this.Table, fieldParameterList.ToString());
            }
            else
            {
                return string.Format("UPDATE {0} SET {1} WHERE {2}", this.Table, fieldParameterList.ToString(), this.Where);
            }
        }
    }
}
