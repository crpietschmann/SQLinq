//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using SQLinq;
using System;

namespace SQLinqTest
{
    public class PersonInsert
    {
        [SQLinqColumn(insert: false, update: false)]
        public Guid ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool IsEmployed { get; set; }

        public Guid ParentID { get; set; }

        [SQLinqColumn("Column With Spaces")]
        public string ColumnWithSpaces { get; set; }
    }
}
