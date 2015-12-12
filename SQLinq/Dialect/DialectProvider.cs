//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: https://github.com/crpietschmann/SQLinq/blob/master/LICENSE

using System;
using System.Text;

namespace SQLinq
{
    public static class DialectProvider
    {
        public static Type DefaultProviderType = typeof(SqlServerDialect);
        public static ISqlDialect Create()
        {
            return (ISqlDialect)Activator.CreateInstance(DefaultProviderType);
        }

        public static ISqlDialect Create<T>()
            where T : ISqlDialect, new()
        {
            return new T();
        }

        internal static string ConcatFieldArray(string[] fields)
        {
            if (fields == null) return string.Empty;
            if (fields.Length == 0) return string.Empty;

            var sb = new StringBuilder();
            for (var s = 0; s < fields.Length; s++)
            {
                if (s > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(fields[s]);
            }
            return sb.ToString();
        }
    }
}
