//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqInsertResultTest
    {
        [TestMethod]
        public void Constructor()
        {
            var target = new SQLinqInsertResult();
        }

        [TestMethod]
        public void Table_001()
        {
            var target = new SQLinqInsertResult();
            var expected = "test";
            target.Table = expected;
            var actual = target.Table;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Fields_001()
        {
            var target = new SQLinqInsertResult();
            var expected = new Dictionary<string, string>();
            target.Fields = expected;
            var actual = target.Fields;
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void Parameters_001()
        {
            var target = new SQLinqInsertResult();
            var expected = new Dictionary<string, object>();
            target.Parameters = expected;
            var actual = target.Parameters;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToQuery_001()
        {
            var data = new Person
            {
                FirstName = "Chris",
                LastName = "Pietschmann"
            };
            var target = new SQLinqInsert<Person>(data);
            var result = target.ToSQL();
            var actual = result.ToQuery();

            Assert.AreEqual("INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_1, @sqlinq_2, @sqlinq_3, @sqlinq_4, @sqlinq_5, @sqlinq_6, @sqlinq_7)", actual);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ToQuery_Table_001()
        {
            var target = new SQLinqInsertResult()
            {
                Table = string.Empty,
                Fields = new Dictionary<string, string>
                {
                    { "test", "test" }
                },
                Parameters = new Dictionary<string, object>
                {
                    { "test", 1 }
                }
            };
            target.ToQuery();
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ToQuery_Table_002()
        {
            var target = new SQLinqInsertResult()
            {
                Table = " ",
                Fields = new Dictionary<string, string>
                {
                    { "test", "test" }
                },
                Parameters = new Dictionary<string, object>
                {
                    { "test", 1 }
                }
            };
            target.ToQuery();
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void ToQuery_Table_003()
        {
            var target = new SQLinqInsertResult()
            {
                Table = null,
                Fields = new Dictionary<string, string>
                {
                    { "test", "test" }
                },
                Parameters = new Dictionary<string, object>
                {
                    { "test", 1 }
                }
            };
            target.ToQuery();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ToQuery_Fields_001()
        {
            var target = new SQLinqInsertResult()
            {
                Table = "test",
                Fields = new Dictionary<string, string>(),
                Parameters = new Dictionary<string, object>
                {
                    { "test", 1 }
                }
            };
            target.ToQuery();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ToQuery_Fields_002()
        {
            var target = new SQLinqInsertResult()
            {
                Table = "test",
                Fields = null,
                Parameters = new Dictionary<string, object>
                {
                    { "test", 1 }
                }
            };
            target.ToQuery();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ToQuery_Parameters_001()
        {
            var target = new SQLinqInsertResult()
            {
                Table = "test",
                Fields = new Dictionary<string, string>
                {
                    { "test", "test" }
                },
                Parameters = new Dictionary<string, object>()
            };
            target.ToQuery();
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void ToQuery_Parameters_002()
        {
            var target = new SQLinqInsertResult()
            {
                Table = "test",
                Fields = new Dictionary<string, string>
                {
                    { "test", "test" }
                },
                Parameters = null
            };
            target.ToQuery();
        }
    }
}
