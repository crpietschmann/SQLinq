//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;
using System.Text;

namespace SQLinq
{
    public class SQLinqIfResult : ISQLinqResult
    {
        public SQLinqIfResult()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        public SQLinqIfOperator Operator { get; set; }
        public string If { get; set; }
        public string Then { get; set; }
        public string Else { get; set; }
        public IDictionary<string, object> Parameters { get; set; }

        public string ToQuery()
        {
            var isThenDefined = !string.IsNullOrWhiteSpace(this.Then);
            var isElseDefined = !string.IsNullOrWhiteSpace(this.Else);

            var sql = new StringBuilder("IF (");

            if (!isThenDefined && isElseDefined)
            {
                sql.Append("NOT(");
            }

            var isOperatorDefined = true;
            switch (this.Operator)
            {
                case SQLinqIfOperator.Exists:
                    sql.Append("EXISTS(");
                    break;

                case SQLinqIfOperator.Not:
                    sql.Append("NOT(");
                    break;

                case SQLinqIfOperator.None:
                default:
                    isOperatorDefined = false;
                    break;
            }

            sql.Append(this.If);

            if (isOperatorDefined)
            {
                sql.Append(")");
            }

            if (!isThenDefined && isElseDefined)
            {
                sql.Append(")");
            }

            sql.AppendLine(")");

            if (isThenDefined)
            {
                sql.AppendLine("BEGIN");
                sql.AppendLine(this.Then);
                sql.AppendLine("END");

                if (isElseDefined)
                {
                    sql.AppendLine("ELSE");
                }
            }

            if (isElseDefined)
            {
                sql.AppendLine("BEGIN");
                sql.AppendLine(this.Else);
                sql.AppendLine("END");
            }

            return sql.ToString();
        }
    }
}
