//Copyright (c) Chris Pietschmann 2012 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using SQLinq;

namespace SQLinqTest
{
    [SQLinqSubQuery(Type = typeof(SubQueryPerson), Method = "TableQuery")]
    public class SubQueryPerson
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        //[SQLinqSubQuery(SQL = "SELECT TOP 1 [Name] FROM Company")]
        //public string CompanyName { get; set; }

        public static SQLinq<Person> TableQuery()
        {
            return from d in new SQLinq<Person>()
                   where d.Age > 30
                   select new {
                       ID = d.ID,
                       FirstName = d.FirstName,
                       LastName = d.LastName,
                       Age = d.Age
                   };
        }
    }
}
