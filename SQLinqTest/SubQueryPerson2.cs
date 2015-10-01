//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using SQLinq;

namespace SQLinqTest
{
    [SQLinqTable("Person"),
    SQLinqSubQuery(SQL = "SELECT ID, FirstName, LastName, Age, Desc FROM StraightSQLPerson")]
    public class SubQueryPerson2
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        [SQLinqColumn("Desc")]
        public string Description { get; set; }
    }
}
