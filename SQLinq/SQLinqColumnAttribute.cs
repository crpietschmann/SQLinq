//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;

namespace SQLinq
{
    /// <summary>
    /// Used to explicitly specify the database column name if it doesn't match the object property name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SQLinqColumnAttribute : Attribute
    {
        /// <summary>
        /// SQLinqColumnAttribute constructor
        /// </summary>
        /// <param name="columnName">The database column name to use for this property with SQLinq queries.</param>
        public SQLinqColumnAttribute(string columnName = null, bool insert = true, bool update = true)
        {
            this.Column = columnName;
            this.Insert = insert;
            this.Update = update;
        }

        /// <summary>
        /// The database column name to use for this property with SQLinq queries.
        /// </summary>
        public string Column { get; private set; }

        /// <summary>
        /// Determines whether the column is used for inserting; via ISQLinqInsert.
        /// </summary>
        public bool Insert { get; set; }

        /// <summary>
        /// Determines whether the column is used for updating; via ISQLinqUpdate.
        /// </summary>
        public bool Update { get; set; }
    }
}
