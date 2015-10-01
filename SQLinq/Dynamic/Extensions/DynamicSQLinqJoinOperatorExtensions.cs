//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;

namespace SQLinq.Dynamic.Extensions
{
    public static class DynamicSQLinqJoinOperatorExtensions
    {
        public static string ToSQL(this DynamicSQLinqJoinOperator op)
        {
            switch (op)
            {
                case DynamicSQLinqJoinOperator.Inner:
                    return "JOIN";
                case DynamicSQLinqJoinOperator.Left:
                    return "LEFT JOIN";
                case DynamicSQLinqJoinOperator.Right:
                    return "RIGHT JOIN";
                case DynamicSQLinqJoinOperator.Full:
                    return "FULL JOIN";
                default:
                    throw new Exception(string.Format("Unsupported DynamicSQLinqJoinOperator value: {0}", op.ToString()));
            }
        }
    }
}
