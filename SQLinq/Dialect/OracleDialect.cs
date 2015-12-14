//Copyright (c) Chris Pietschmann 2015 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: https://github.com/crpietschmann/SQLinq/blob/master/LICENSE

using System.Text;

namespace SQLinq
{
    public class OracleDialect : ISqlDialect
    {
        const string _Space = " ";

        public object ConvertParameterValue(object value)
        {
            if (value is bool)
            {
                return (bool)value ? 1 : 0;
            }

            return value;
        }

        public void AssertSkip<T>(SQLinq<T> sqLinq)
        {

        }

        const string _parameterPrefix = ":";
        public string ParameterPrefix
        {
            get { return _parameterPrefix; }
        }

        public string ParseTableName(string tableName)
        {
            return tableName;
        }

        public string ParseColumnName(string columnName)
        {
            if (columnName.Contains(_Space))
            {
                return string.Format("\"\"", columnName);
            }
            return columnName;
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

            ////if (selectResult.Skip != null)
            ////{
            ////    if (sb.Length > 0)
            ////    {
            ////        sb.Append(",");
            ////    }
            ////    sb.Append(string.Format(" ROW_NUMBER() OVER (ORDER BY {0}) AS [SQLinq_row_number]", orderby));
            ////}

            sb.Append(" FROM ");

            if (selectResult.Distinct == true && selectResult.Skip != null && selectResult.Take != null)
            {
                sb.Append("(SELECT DISTINCT ");
                sb.Append(DialectProvider.ConcatFieldArray(selectResult.Select));
                sb.Append(" FROM ");
                sb.Append(selectResult.Table);
                sb.Append(") AS d");
            }
            else
            {
                sb.Append(selectResult.Table);
            }

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
            }

            if (selectResult.Skip != null)
            {
                // paging support
                var start = (selectResult.Skip + 1).ToString();
                var end = (selectResult.Skip + (selectResult.Take ?? 0)).ToString();
                if (selectResult.Take == null)
                {
                    return string.Format("SELECT * FROM (SELECT {0}) WHERE ROWNUM >= {1}", sb.ToString(), start);
                }

                return string.Format("SELECT SQLinq_Outer.* FROM (SELECT ROWNUM rn, SQLinq_Inner.* FROM (SELECT {0}) SQLinq_Inner) SQLinq_Outer WHERE SQLinq_Outer.rn >= {1} AND SQLinq_Outer.rn <= {2}",
                       sb.ToString(),
                       start,
                       end);
            }
            else if (selectResult.Take != null)
            {
                return string.Format("SELECT * FROM (SELECT {0}) WHERE ROWNUM <= {1}", sb.ToString(), selectResult.Take.ToString());
            }

            return "SELECT " + sb.ToString() + sqlOrderBy;
        }
    }
}
