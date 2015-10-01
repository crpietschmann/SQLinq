//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq.Compiler;
using System.Collections.Generic;

namespace SQLinq
{
    public class SQLinqCollection : List<ISQLinq>, ISQLinq
    {
        public SQLinqCollection()
        { }

        public SQLinqCollection(params ISQLinq[] queries)
        {
            this.AddRange(queries);
        }

        public SQLinqCollection(IEnumerable<ISQLinq> queries)
        {
            this.AddRange(queries);
        }

        public ISQLinqResult ToSQL(int existingParameterCount = 0, string parameterNamePrefix = SqlExpressionCompiler.DefaultParameterNamePrefix)
        {
            var paramCount = existingParameterCount;

            var result = new SQLinqCollectionResult();

            foreach (var query in this)
            {
                var r = query.ToSQL(paramCount, parameterNamePrefix);

                foreach (var p in r.Parameters)
                {
                    result.Parameters.Add(p);
                }

                result.Queries.Add(r.ToQuery());

                paramCount = existingParameterCount + result.Parameters.Count;
            }

            return result;
        }
    }
}
