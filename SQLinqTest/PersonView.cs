//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using SQLinq;

namespace SQLinqTest
{
    [SQLinqTable("vw_Person")]
    public class PersonView
    {
        public Guid ID { get; set; }

        [SQLinqColumn("First_Name")]
        public string FirstName { get; set; }

        [SQLinqColumn("Last_Name")]
        public string LastName { get; set; }

        [SQLinqColumn]
        public int Age { get; set; }
    }
}
