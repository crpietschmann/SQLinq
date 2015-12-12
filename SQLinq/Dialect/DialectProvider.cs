using System;
using System.Text;

namespace SQLinq
{
    public static class DialectProvider
    {
        public static ISqlDialect Dialect = new SqlServerDialect();

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
