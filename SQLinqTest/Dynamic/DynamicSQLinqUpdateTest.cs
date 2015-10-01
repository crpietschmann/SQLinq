//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;
using SQLinq.Dynamic;
using System.Collections.Generic;

namespace SQLinqTest.Dynamic
{
    [TestClass]
    public class DynamicSQLinqUpdateTest
    {
        [TestMethod]
        public void ToSQL_001()
        {
            var data = new Dictionary<string, object>
            {
                { "ID", 1 },
                { "FirstName", "Chris" },
                { "LastName", "Pietschmann"},
                { "Age", 0 }
            };

            var target = new DynamicSQLinqUpdate(data, "tblPerson");
            var actual = (SQLinqUpdateResult)target.ToSQL();

            Assert.AreEqual("[tblPerson]", actual.Table);

            Assert.AreEqual(4, actual.Fields.Count);
            Assert.AreEqual("@sqlinq_1", actual.Fields["ID"]);
            Assert.AreEqual("@sqlinq_2", actual.Fields["FirstName"]);
            Assert.AreEqual("@sqlinq_3", actual.Fields["LastName"]);
            Assert.AreEqual("@sqlinq_4", actual.Fields["Age"]);

            Assert.AreEqual(4, actual.Parameters.Count);
            Assert.AreEqual(1, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Chris", actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual("Pietschmann", actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void ToSQL_002()
        {
            var data = new Dictionary<string, object>
            {
                { "ID", 1 },
                { "FirstName", "Chris" },
                { "LastName", "Pietschmann"},
                { "Age", 0 }
            };

            var target = new DynamicSQLinqUpdate(data, "[tblPerson]");
            target.Where<int>("ID", d => d == 42);
            var actual = (SQLinqUpdateResult)target.ToSQL();

            Assert.AreEqual("[tblPerson]", actual.Table);

            Assert.AreEqual(4, actual.Fields.Count);
            Assert.AreEqual("@sqlinq_1", actual.Fields["ID"]);
            Assert.AreEqual("@sqlinq_2", actual.Fields["FirstName"]);
            Assert.AreEqual("@sqlinq_3", actual.Fields["LastName"]);
            Assert.AreEqual("@sqlinq_4", actual.Fields["Age"]);

            Assert.AreEqual(5, actual.Parameters.Count);
            Assert.AreEqual(1, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Chris", actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual("Pietschmann", actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(42, actual.Parameters["@sqlinq_5"]);
        }

        [TestMethod]
        public void ToSQL_003()
        {
            var data = new Dictionary<string, object>
            {
                { "ID", 1 },
                { "FirstName", "Chris" },
                { "LastName", "Pietschmann"},
                { "Age", 0 }
            };

            var target = new DynamicSQLinqUpdate(data, "tblPerson");

            var actual = target.ToSQL().ToQuery();

            var expected = "UPDATE [tblPerson] SET [ID] = @sqlinq_1, [FirstName] = @sqlinq_2, [LastName] = @sqlinq_3, [Age] = @sqlinq_4";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToSQL_004()
        {
            var data = new Dictionary<string, object>
            {
                { "ID", 1 },
                { "FirstName", "Chris" },
                { "LastName", "Pietschmann"},
                { "Age", 0 }
            };

            var target = new DynamicSQLinqUpdate(data, "tblPerson");
            target.Where<int>("ID", d => d == 42);

            var actual = target.ToSQL().ToQuery();
            var expected = "UPDATE [tblPerson] SET [ID] = @sqlinq_1, [FirstName] = @sqlinq_2, [LastName] = @sqlinq_3, [Age] = @sqlinq_4 WHERE [ID] = @sqlinq_5";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToSQL_005()
        {
            var data = new Dictionary<string, object>
            {
                { "ID", 1 },
                { "FirstName", "Chris" },
                { "LastName", "Pietschmann"},
                { "Age", 0 }
            };

            var target = new DynamicSQLinqUpdate(data, "tblPerson");
            target.Where<int>("ID", d => d == 42);

            var actual = target.ToSQL(15).ToQuery();
            var expected = "UPDATE [tblPerson] SET [ID] = @sqlinq_15, [FirstName] = @sqlinq_16, [LastName] = @sqlinq_17, [Age] = @sqlinq_18 WHERE [ID] = @sqlinq_19";

            Assert.AreEqual(expected, actual);
        }
    }
}
