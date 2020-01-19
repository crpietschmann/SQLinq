//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace SQLinq.Dapper.Test
{
    [TestClass]
    public class IDbConnectionExtensionsTest
    {
        private IDbConnection GetDBConnection()
        {
            //var folder = AppDomain.CurrentDomain.BaseDirectory.ToLower();
            //folder = folder.Substring(0, folder.IndexOf("testresults") - 1) + "\\DapperExtensions\\SQLinq.Dapper.Test\\bin\\Debug\\App_Data\\";
            //return new SqlConnection("Data Source=.\\SQLExpress;AttachDbFilename=" + folder + "Database1.mdf;Integrated Security=True");
            //return new SqlConnection("Data Source=.\\SQLEXPRESS;AttachDbFilename=" + folder + "Database1.mdf;Integrated Security=True;User Instance=True");
            //return new SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=" + folder + "Database1.mdf;Integrated Security=True;Connect Timeout=15");
            return new SqlConnection("Data Source=.\\SQLEXPRESS01;Database=DATABASE1;Integrated Security=True;");
        }

        #region Query
        
        [TestMethod]
        public void Query_001()
        {
            var query = from p in new SQLinq<Person>()
                        where p.FirstName.StartsWith("C")
                        select p;

            IEnumerable<Person> people;

            using (var con = GetDBConnection())
            {
                con.Open();
                people = con.Query(query);
                con.Close();
            }
            Assert.AreEqual(2, people.Count());
        }

        [TestMethod]
        public void Query_002()
        {
            IEnumerable<Person> people;

            using (var con = GetDBConnection())
            {
                con.Open();
                people = con.Query(from p in new SQLinq<Person>()
                                   where p.FirstName == "Bill"
                                   select p);
                con.Close();
            }
            Assert.AreEqual(1, people.Count());
        }

        [TestMethod]
        public void Query_003()
        {
            var query = from p in new SQLinq<IPerson>()
                        where p.FirstName.StartsWith("C")
                        select p;

            IEnumerable<dynamic> people;

            using (var con = GetDBConnection())
            {
                con.Open();
                people = con.Query((ISQLinq)query);
                con.Close();
            }
            Assert.AreEqual(2, people.Count());
        }

        [TestMethod]
        public void Query_004()
        {
            var query = from p in new SQLinq<IPerson>()
                        where p.Age == null
                        select p;

            IEnumerable<dynamic> people;

            using (var con = GetDBConnection())
            {
                con.Open();
                people = con.Query((ISQLinq)query);
                con.Close();
            }
            Assert.AreEqual(1, people.Count());
        }

        #endregion

        #region Execute

        [TestMethod]
        public void Execute_001()
        {
            var expected = new Person
            {
                ID = Guid.NewGuid(),
                FirstName = "Steven",
                LastName = "Johnston",
                Age = 1065
            };

            using (var con = GetDBConnection())
            {
                con.Open();
                var trans = con.BeginTransaction();

                try
                {
                    var query = new SQLinqInsert<Person>(expected);

                    con.Execute(query, trans);


                    var actual = con.Query(from p in new SQLinq<Person>()
                                           where p.ID == expected.ID
                                           select p, trans).FirstOrDefault();

                    Assert.IsNotNull(actual);
                    Assert.AreEqual(expected.ID, actual.ID);
                    Assert.AreEqual(expected.FirstName, actual.FirstName);
                    Assert.AreEqual(expected.LastName, actual.LastName);
                    Assert.AreEqual(expected.Age, actual.Age);
                }
                finally
                {
                    trans.Rollback();
                    con.Close();
                }
            }
        }

        [TestMethod]
        public void Execute_002()
        {
            var expected = new Person
            {
                ID = Guid.Parse("9d12a0f4-6494-4d58-bee4-c7a99a287c1c"),
                FirstName = "Chris",
                LastName = "Pietschmann",
                Age = 1065
            };

            using (var con = GetDBConnection())
            {
                con.Open();
                var trans = con.BeginTransaction();

                try
                {
                    var query = new SQLinqUpdate<Person>(expected);

                    con.Execute(query, trans);


                    var actual = con.Query(from p in new SQLinq<Person>()
                                           where p.ID == expected.ID
                                           select p, trans).FirstOrDefault();

                    Assert.IsNotNull(actual);
                    Assert.AreEqual(expected.ID, actual.ID);
                    Assert.AreEqual(expected.FirstName, actual.FirstName);
                    Assert.AreEqual(expected.LastName, actual.LastName);
                    Assert.AreEqual(expected.Age, actual.Age);
                }
                finally
                {
                    trans.Rollback();
                    con.Close();
                }
            }
        }

        [TestMethod]
        public void Execute_003()
        {
            var expected = new Person
            {
                ID = Guid.Parse("9d12a0f4-6494-4d58-bee4-c7a99a287c1c"),
                FirstName = "Chris",
                LastName = "Pietschmann",
                Age = null
            };

            using (var con = GetDBConnection())
            {
                con.Open();
                var trans = con.BeginTransaction();

                try
                {
                    var query = new SQLinqUpdate<Person>(expected);

                    con.Execute(query, trans);


                    var actual = con.Query(from p in new SQLinq<Person>()
                                           where p.ID == expected.ID
                                           select p, trans).FirstOrDefault();

                    Assert.IsNotNull(actual);
                    Assert.AreEqual(expected.ID, actual.ID);
                    Assert.AreEqual(expected.FirstName, actual.FirstName);
                    Assert.AreEqual(expected.LastName, actual.LastName);
                    Assert.IsNull(actual.Age);
                }
                finally
                {
                    trans.Rollback();
                    con.Close();
                }
            }
        }

        #endregion
    }
}
