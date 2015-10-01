//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System.Collections.Generic;
using System.Data;
using DapperDotNet = Dapper;

namespace SQLinq.Dapper
{
    public class DictionaryParameterObject : DapperDotNet.SqlMapper.IDynamicParameters
    {
        public DictionaryParameterObject(IDictionary<string, object> dictionary)
        {
            this.Dictionary = dictionary;
        }

        public IDictionary<string, object> Dictionary { get; private set; }

        public void AddParameters(IDbCommand command, DapperDotNet.SqlMapper.Identity identity)
        {
            foreach (var item in this.Dictionary)
            {
                var p = command.CreateParameter();
                p.ParameterName = item.Key;
                p.Value = item.Value ?? System.DBNull.Value;
                command.Parameters.Add(p);
            }
        }
    }
}
