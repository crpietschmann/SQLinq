using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLinq.Dialect
{
    public class MySqlDialect : ISqlDialect
    {
        const string _Space = " ";
        const string Identifier = "`";

        public object ConvertParameterValue(object value)
        {
            if (value is bool)
            {
                return (bool)value ? 1 : 0;
            }

            return value;
        }

        public string ParameterPrefix => "@";
        

        public string ParseTableName(string tableName)
        {
            if (!tableName.StartsWith(Identifier))
            {
                return $"{Identifier}{tableName}{Identifier}";
            }

            return tableName;
        }

        public string ParseColumnName(string columnName)
        {
            if (!columnName.StartsWith(Identifier) && !columnName.Contains("."))
            {
                return $"{Identifier}{columnName}{Identifier}";
            }

            return columnName;
        }

        public void AssertSkip<T>(SQLinq<T> sqLinq)
        {
            if (sqLinq.OrderByExpressions.Count == 0)
            {
                throw new NotSupportedException("SQLinq: Skip can only be performed if OrderBy is specified.");
            }
        }

        public string ToQuery(SQLinqSelectResult selectResult)
        {
            var orderby = DialectProvider.ConcatFieldArray(selectResult.OrderBy);

            var groupby = DialectProvider.ConcatFieldArray(selectResult.GroupBy);

            var sb = new StringBuilder();

            if (selectResult.Distinct == true)
            {
                sb.Append("DISTINCT ");
            }

            // SELECT
            sb.Append(DialectProvider.ConcatFieldArray(selectResult.Select));

            sb.Append(" FROM ");

            sb.Append(selectResult.Table);

            if (selectResult.Join != null)
            {
                foreach (var j in selectResult.Join)
                {
                    sb.Append(_Space);
                    sb.Append(j);
                }
            }

            if (!string.IsNullOrEmpty(selectResult.Where))
            {
                sb.Append(" WHERE ");
                sb.Append(selectResult.Where);
            }

            if (!string.IsNullOrEmpty(groupby))
            {
                sb.Append(" GROUP BY ");
                sb.Append(groupby);
            }

            if (!string.IsNullOrEmpty(selectResult.Having))
            {
                sb.Append(" HAVING ");
                sb.Append(selectResult.Having);
            }

            var sqlOrderBy = string.Empty;
            if (orderby.Length > 0)
            {
                sqlOrderBy = " ORDER BY " + orderby;
                sb.Append(sqlOrderBy);
            }

            if (selectResult.Take != null)
            {
                if (selectResult.Skip != null)
                {
                    sb.Append($" LIMIT {selectResult.Take} OFFSET {selectResult.Skip}");
                }
                else
                {
                    sb.Append($" LIMIT {selectResult.Take}");
                }
            }
            else
            {
                if (selectResult.Skip != null)
                {
                    sb.Append($" LIMIT {long.MaxValue} OFFSET {selectResult.Skip}");
                }
            }

            return $"SELECT {sb}";
        }
    }
}
