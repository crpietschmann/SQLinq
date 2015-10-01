//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;
using System.Data;
using DapperDotNet = Dapper;

namespace SQLinq.Dapper
{
    public static class IDbConnectionExtensions
    {
        public static IEnumerable<T> Query<T>(this IDbConnection dbconnection, SQLinq<T> query,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
            where T : new()
        {
            var result = query.ToSQL();

            var sql = result.ToQuery();
            var parameters = new DictionaryParameterObject(result.Parameters);

            return DapperDotNet.SqlMapper.Query<T>(dbconnection, sql, parameters, transaction, buffered, commandTimeout, commandType);
        }

        public static IEnumerable<dynamic> Query(this IDbConnection dbconnection, ISQLinq query,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = query.ToSQL();

            var sql = result.ToQuery();
            var parameters = new DictionaryParameterObject(result.Parameters);

            return DapperDotNet.SqlMapper.Query(dbconnection, sql, parameters, transaction, buffered, commandTimeout, commandType);
        }

        public static int Execute(this IDbConnection dbconnection, ISQLinq query,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var result = query.ToSQL();

            var sql = result.ToQuery();
            var parameters = new DictionaryParameterObject(result.Parameters);
            
            return DapperDotNet.SqlMapper.Execute(dbconnection, sql, parameters, transaction, commandTimeout, commandType);
        }
    }
}
