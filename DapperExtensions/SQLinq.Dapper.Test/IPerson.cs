//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;

namespace SQLinq.Dapper.Test
{
    [SQLinqTable("Person")]
    public interface IPerson
    {
        Guid ID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int? Age { get; set; }
    }
}
