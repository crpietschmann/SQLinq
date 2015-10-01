//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Collections.Generic;

namespace SQLinq
{
    /// <summary>
    /// Allows for SQL sub-queries to be specified using SQLinq.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SQLinqSubQueryAttribute : Attribute
    {
        /// <summary>
        /// The SQL code to use for the sub-query.
        /// </summary>
        public string SQL { get; set; }

        /// <summary>
        /// The Type that contains the static Method to call for generating the sub-query.
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// The static Method of the Type that contains the method to call for generating the sub-query.
        /// </summary>
        public string Method { get; set; }

        public string GetQuery(Dictionary<string, object> parameters)
        {
            if (!string.IsNullOrEmpty(this.SQL))
            {
                return this.SQL;
            }

            // Invoke static method that returns the sub-query to use
            var methods = this.Type.GetMethods();
            var method = this.Type.GetMethod(this.Method);
            var query = (ISQLinq)method.Invoke(null, null);

            var result = query.ToSQL();

            // Add parameters
            foreach (var p in result.Parameters)
            {
                parameters.Add(p.Key, p.Value);
            }

            // return SQL
            return result.ToQuery();
        }
    }
}
