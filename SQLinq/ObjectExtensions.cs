//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

namespace SQLinq
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Creates an instance of SQLinq that is based off the specified Object Type.
        /// </summary>
        /// <typeparam name="T">The Type to use for creating the SQLinq instance.</typeparam>
        /// <param name="obj">The Object Type to base the SQLinq instance off of.</param>
        /// <param name="tableName">Optional. The database table name to use for generated SQL code. If specified, this will override the Objects name and/or SQLinqTable attribute usage.</param>
        /// <returns>A SQLinq instance.</returns>
        public static SQLinq<T> ToSQLinq<T>(this T obj, string tableName = null, ISqlDialect dialect = null)
        {
            if (dialect == null)
            {
                dialect = DialectProvider.Create();
            }
            return SQLinq.Create(obj, tableName, dialect);
        }

        public static SQLinqInsert<T> ToSQLinqInsert<T>(this T obj, string tableName = null, ISqlDialect dialect = null)
        {
            if (dialect == null)
            {
                dialect = DialectProvider.Create();
            }
            return SQLinq.Insert(obj, tableName, dialect);
        }

        public static SQLinqUpdate<T> ToSQLinqUpdate<T>(this T obj, string tableName = null, ISqlDialect dialect = null)
        {
            if (dialect == null)
            {
                dialect = DialectProvider.Create();
            }
            return SQLinq.Update(obj, tableName, dialect);
        }
    }
}
