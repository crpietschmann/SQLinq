//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;

namespace SQLinq.Dynamic
{
    public enum DynamicSQLinqJoinOperator
    {
        /// <summary>
        /// 'JOIN' or 'INNER JOIN': Returns rows when there is at least one match in both tables.
        /// </summary>
        Inner,

        /// <summary>
        /// 'LEFT JOIN' or 'LEFT OUTER JOIN': Returns all rows from the left table, even if there are no matches in the right table.
        /// </summary>
        Left,

        /// <summary>
        /// 'RIGHT JOIN' or 'RIGHT OUTER JOIN': Returns all rows from the right table, even if there are no matches in the left table.
        /// </summary>
        Right,

        /// <summary>
        /// 'FULL JOIN' or 'FULL OUTER JOIN': Returns rows when there is a match in one of the tables.
        /// </summary>
        Full
    }
}
