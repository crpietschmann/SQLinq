//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;

namespace SQLinq
{
    /// <summary>
    /// Used to explicitly specify the database table or view name if it doesn't match the object name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class SQLinqTableAttribute : Attribute
    {
        /// <summary>
        /// SQLinqTableAttribute constructor
        /// </summary>
        /// <param name="tableName">The database table/view name to use for this object with SQLinq queries.</param>
        public SQLinqTableAttribute(string tableName)
        {
            this.Table = tableName;
        }

        /// <summary>
        /// The database table/view name to use for this object with SQLinq queries.
        /// </summary>
        public string Table { get; private set; }
    }
}
