//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;

namespace SQLinq
{
    public class SQLinqSelectResult : ISQLinqResult
    {
        public SQLinqSelectResult(ISqlDialect dialect)
        {
            this.Dialect = dialect;
        }

        public ISqlDialect Dialect { get; private set; }

        public string[] Select { get; set; }
        public bool? Distinct { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public string Table { get; set; }
        public string[] Join { get; set; }
        public string Where { get; set; }
        public string Having { get; set; }
        public string[] GroupBy { get; set; }
        public string[] OrderBy { get; set; }
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Returns the Full SQL statement for the specified query
        /// </summary>
        /// <returns></returns>
        public string ToQuery()
        {
            return this.Dialect.ToQuery(this);
        }
    }
}
