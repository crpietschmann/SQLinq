//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;

namespace SQLinq
{
    public class SQLinqJoinResult
    {
        public SQLinqJoinResult()
            : this(new string[0])
        { }

        public SQLinqJoinResult(IEnumerable<string> join)
            : this(join, new Dictionary<string, object>())
        { }

        public SQLinqJoinResult(IEnumerable<string> join, IDictionary<string, object> parameters)
        {
            this.Join = join;
            this.Parameters = parameters;
        }

        public IEnumerable<string> Join { get; set; }
        public IDictionary<string, object> Parameters { get; set; }
    }
}
