//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;
using System.Text;

namespace SQLinq
{
    public class SQLinqCollectionResult : ISQLinqResult
    {
        public SQLinqCollectionResult()
        {
            this.Parameters = new Dictionary<string, object>();
            this.Queries = new List<string>();
        }

        public IDictionary<string, object> Parameters { get; set; }

        public IList<string> Queries { get; set; }

        public string ToQuery()
        {
            var sql = new StringBuilder();

            foreach (var query in this.Queries)
            {
                sql.AppendLine(query);
            }

            return sql.ToString();
        }
    }
}
