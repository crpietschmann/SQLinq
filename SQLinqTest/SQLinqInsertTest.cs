//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;
using System;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqInsertTest
    {
        [TestMethod]
        public void ToSQL_001()
        {
            var data = new Person
            {
                FirstName = "Chris",
                LastName = "Pietschmann"
            };
            var target = new SQLinqInsert<Person>(data);
            var actual = (SQLinqInsertResult)target.ToSQL();

            Assert.AreEqual("[Person]", actual.Table);
            
            Assert.AreEqual(7, actual.Fields.Count);
            Assert.AreEqual("@sqlinq_1", actual.Fields["ID"]);
            Assert.AreEqual("@sqlinq_2", actual.Fields["FirstName"]);
            Assert.AreEqual("@sqlinq_3", actual.Fields["LastName"]);
            Assert.AreEqual("@sqlinq_4", actual.Fields["Age"]);
            Assert.AreEqual("@sqlinq_5", actual.Fields["[Is_Employed]"]);
            Assert.AreEqual("@sqlinq_6", actual.Fields["ParentID"]);
            Assert.AreEqual("@sqlinq_7", actual.Fields["Column With Spaces"]);

            Assert.AreEqual(7, actual.Parameters.Count);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Chris", actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual("Pietschmann", actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_7"]);
        }

        [TestMethod]
        public void ToSQL_Oracle_001()
        {
            var dialect = new OracleDialect();

            var data = new Person
            {
                FirstName = "Chris",
                LastName = "Pietschmann"
            };
            var target = new SQLinqInsert<Person>(data, dialect);
            var actual = (SQLinqInsertResult)target.ToSQL();

            Assert.AreEqual("Person", actual.Table);

            Assert.AreEqual(7, actual.Fields.Count);
            Assert.AreEqual(":sqlinq_1", actual.Fields["ID"]);
            Assert.AreEqual(":sqlinq_2", actual.Fields["FirstName"]);
            Assert.AreEqual(":sqlinq_3", actual.Fields["LastName"]);
            Assert.AreEqual(":sqlinq_4", actual.Fields["Age"]);
            Assert.AreEqual(":sqlinq_5", actual.Fields["[Is_Employed]"]);
            Assert.AreEqual(":sqlinq_6", actual.Fields["ParentID"]);
            Assert.AreEqual(":sqlinq_7", actual.Fields["Column With Spaces"]);

            Assert.AreEqual(7, actual.Parameters.Count);
            Assert.AreEqual(Guid.Empty, actual.Parameters[":sqlinq_1"]);
            Assert.AreEqual("Chris", actual.Parameters[":sqlinq_2"]);
            Assert.AreEqual("Pietschmann", actual.Parameters[":sqlinq_3"]);
            Assert.AreEqual(0, actual.Parameters[":sqlinq_4"]);
            Assert.AreEqual(false, actual.Parameters[":sqlinq_5"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters[":sqlinq_6"]);
            Assert.AreEqual(null, actual.Parameters[":sqlinq_7"]);
        }

        [TestMethod]
        public void ToSQL_002()
        {
            var data = new PersonView
            {
                FirstName = "Chris",
                LastName = "Pietschmann"
            };
            var target = new SQLinqInsert<PersonView>(data);
            var actual = (SQLinqInsertResult)target.ToSQL();

            Assert.AreEqual("[vw_Person]", actual.Table);

            Assert.AreEqual(4, actual.Fields.Count);
            Assert.AreEqual("@sqlinq_1", actual.Fields["ID"]);
            Assert.AreEqual("@sqlinq_2", actual.Fields["First_Name"]);
            Assert.AreEqual("@sqlinq_3", actual.Fields["Last_Name"]);
            Assert.AreEqual("@sqlinq_4", actual.Fields["Age"]);

            Assert.AreEqual(4, actual.Parameters.Count);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Chris", actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual("Pietschmann", actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void ToSQL_003()
        {
            var data = new
            {
                ID = 1,
                FirstName = "Chris",
                LastName = "Pietschmann",
                Age = 0
            };
            var target = SQLinq.SQLinq.Insert(data, "Person");
            var actual = (SQLinqInsertResult)target.ToSQL();

            Assert.AreEqual("[Person]", actual.Table);

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
        public void ToSQL_004()
        {
            var data = new PersonInsert
            {
                FirstName = "Chris",
                LastName = "Pietschmann"
            };
            var target = new SQLinqInsert<PersonInsert>(data);
            var actual = (SQLinqInsertResult)target.ToSQL(42, "foo");

            Assert.AreEqual("[PersonInsert]", actual.Table);

            Assert.AreEqual(6, actual.Fields.Count);
            Assert.AreEqual("@foo43", actual.Fields["FirstName"]);
            Assert.AreEqual("@foo44", actual.Fields["LastName"]);
            Assert.AreEqual("@foo45", actual.Fields["Age"]);
            Assert.AreEqual("@foo46", actual.Fields["IsEmployed"]);
            Assert.AreEqual("@foo47", actual.Fields["ParentID"]);
            Assert.AreEqual("@foo48", actual.Fields["Column With Spaces"]);

            Assert.AreEqual(6, actual.Parameters.Count);
            Assert.AreEqual("Chris", actual.Parameters["@foo43"]);
            Assert.AreEqual("Pietschmann", actual.Parameters["@foo44"]);
            Assert.AreEqual(0, actual.Parameters["@foo45"]);
            Assert.AreEqual(false, actual.Parameters["@foo46"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@foo47"]);
            Assert.AreEqual(null, actual.Parameters["@foo48"]);
        }

        [TestMethod]
        public void ToSQL_005()
        {
            var data = new
            {
                ID = 1,
                FirstName = "Chris",
                LastName = "Pietschmann",
                Age = 0
            };
            var target = data.ToSQLinqInsert("Person");
            var actual = (SQLinqInsertResult)target.ToSQL();

            Assert.AreEqual("[Person]", actual.Table);

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
        public void ToSQL_006()
        {
            var data = new PersonInsert
            {
                FirstName = "Chris",
                LastName = "Pietschmann"
            };
            var target = data.ToSQLinqInsert();
            var actual = (SQLinqInsertResult)target.ToSQL(42, "foo");

            Assert.AreEqual("[PersonInsert]", actual.Table);

            Assert.AreEqual(6, actual.Fields.Count);
            Assert.AreEqual("@foo43", actual.Fields["FirstName"]);
            Assert.AreEqual("@foo44", actual.Fields["LastName"]);
            Assert.AreEqual("@foo45", actual.Fields["Age"]);
            Assert.AreEqual("@foo46", actual.Fields["IsEmployed"]);
            Assert.AreEqual("@foo47", actual.Fields["ParentID"]);
            Assert.AreEqual("@foo48", actual.Fields["Column With Spaces"]);

            Assert.AreEqual(6, actual.Parameters.Count);
            Assert.AreEqual("Chris", actual.Parameters["@foo43"]);
            Assert.AreEqual("Pietschmann", actual.Parameters["@foo44"]);
            Assert.AreEqual(0, actual.Parameters["@foo45"]);
            Assert.AreEqual(false, actual.Parameters["@foo46"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@foo47"]);
            Assert.AreEqual(null, actual.Parameters["@foo48"]);
        }

        [TestMethod]
        public void ToSQL_007()
        {
            var data = new ToSQL_007_Class
            {
                ID = 33,
                Name = "Chris",
                DotNotInsert = "Some Value"
            };
            var target = data.ToSQLinqInsert();
            var actual = (SQLinqInsertResult)target.ToSQL();

            Assert.AreEqual("[MyTable]", actual.Table);

            Assert.AreEqual(2, actual.Fields.Count);
            Assert.AreEqual("@sqlinq_1", actual.Fields["ID"]);
            Assert.AreEqual("@sqlinq_2", actual.Fields["Name"]);

            Assert.AreEqual(2, actual.Parameters.Count);
            Assert.AreEqual(33, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Chris", actual.Parameters["@sqlinq_2"]);
        }

        [SQLinqTable("MyTable")]
        private class ToSQL_007_Class
        {
            public int ID { get; set; }
            public string Name { get; set; }

            [SQLinqColumn(insert: false)]
            public string DotNotInsert { get; set; }
        }
    }
}
