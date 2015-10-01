//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq.Dynamic;
using System;

namespace SQLinqTest.Dynamic
{
    [TestClass]
    public class DynamicSQLinqTest
    {
        [TestMethod]
        public void DynamicSQLinq_Create_001()
        {
            var expected = "tblPerson";
            var target = SQLinq.SQLinq.Create(expected);
            Assert.AreEqual(expected, target.TableName);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Where("Make = @0", "Ford")
                            .Where("Color = @0 OR Color = @1", 0, 1);

            var sql = query.ToSQL();
            
            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE (Make = @sqlinq_1) AND (Color = @sqlinq_2 OR Color = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_002()
        {
            var where = new DynamicSQLinqExpressionCollection();
            where.Add("Make = @0", "Ford")
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var query = new DynamicSQLinq("Car")
                            .Where(where);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE (Make = @sqlinq_1) AND (Color = @sqlinq_2 OR Color = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_003()
        {
            var where = new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or);
            where.Add("Make = @0", "Ford")
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var query = new DynamicSQLinq("Car")
                            .Where(where);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE (Make = @sqlinq_1) OR (Color = @sqlinq_2 OR Color = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_004()
        {
            var where = new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or);
            where.Add("Make = @0", "Ford")
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var where2 = new DynamicSQLinqExpressionCollection();
            where2.Add("Make = @0", "Chevrolet");

            where.Add(where2);

            var query = new DynamicSQLinq("Car")
                            .Where(where);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE (Make = @sqlinq_1) OR (Color = @sqlinq_2 OR Color = @sqlinq_3) OR (Make = @sqlinq_4)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual("Chevrolet", sql.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_005()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID", "Make").Select("Color")
                            .Where(new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or)
                            {
                                new DynamicSQLinqExpression("Make = @0", "Ford"),
                                new DynamicSQLinqExpression("Color = @0 OR Color = @1", 0, 1),

                                new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.And)
                                {
                                    new DynamicSQLinqExpression("Make = @0", "Chevrolet"),
                                    new DynamicSQLinqExpression("Color = @0", 2),
                                }
                            })
                            .OrderBy("Make").OrderBy("Color", "ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT ID, Make, Color FROM Car WHERE (Make = @sqlinq_1) OR (Color = @sqlinq_2 OR Color = @sqlinq_3) OR ((Make = @sqlinq_4) AND (Color = @sqlinq_5)) ORDER BY Make, Color, ID", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual("Chevrolet", sql.Parameters["@sqlinq_4"]);
            Assert.AreEqual(2, sql.Parameters["@sqlinq_5"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_006()
        {
            var where = new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or);
            where.Add("(Make = @0) OR (Make <> @0 AND Color = @1)", "Ford", 15)
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var where2 = new DynamicSQLinqExpressionCollection();
            where2.Add("Make = @0", "Chevrolet");

            where.Add(where2);

            var query = new DynamicSQLinq("Car")
                            .Where(where);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE ((Make = @sqlinq_1) OR (Make <> @sqlinq_1 AND Color = @sqlinq_2)) OR (Color = @sqlinq_3 OR Color = @sqlinq_4) OR (Make = @sqlinq_5)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_4"]);
            Assert.AreEqual("Chevrolet", sql.Parameters["@sqlinq_5"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_007()
        {
            var query = new DynamicSQLinq("Car")
                            .Where("Make = @0", "Ford")
                            .Where("Color = @0 OR Color = @1", 0, 1)
                            .Where<string>("[Make]", d => d.StartsWith("F"));

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE (Make = @sqlinq_1) AND (Color = @sqlinq_2 OR Color = @sqlinq_3) AND ([Make] LIKE @sqlinq_4)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual("F%", sql.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_008()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<string>("Make", d => d == null);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE [Make] IS NULL", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_009()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<string>("Make", d => d != null);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE [Make] IS NOT NULL", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_010()
        {
            string nullVal = null;
            var query = new DynamicSQLinq("Car")
                            .Where<string>("Make", d => d == nullVal);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE [Make] IS NULL", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_011()
        {
            var query = new DynamicSQLinq("[Car]")
                            .Where("[Make] = @0", "Ford")
                            .Where("[Color] = @0 OR [Color] = @1", 0, 1);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM [Car] WHERE ([Make] = @sqlinq_1) AND ([Color] = @sqlinq_2 OR [Color] = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        #region Distinct

        [TestMethod]
        public void DynamicSQLinq_Distinct_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Distinct()
                            .Select("Make")
                            .Where("Make = @0", "Ford");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT DISTINCT Make FROM Car WHERE Make = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
        }

        #endregion

        #region SELECT Methods

        [TestMethod]
        public void DynamicSQLinq_Select_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID", "Make");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT ID, Make FROM Car", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Select_002()
        {
            var query = new DynamicSQLinq("Car")
                            .SelectTable("Car", "ID", "Make");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Car.ID, Car.Make FROM Car", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Select_003()
        {
            var query = new DynamicSQLinq("Person")
                            .Select("(FirstName + ' ' + LastName) AS FullName");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT (FirstName + ' ' + LastName) AS FullName FROM Person", code);
        }

        #endregion

        #region ORDER BY Methods

        [TestMethod]
        public void DynamicSQLinq_OrderBy_001()
        {
            var query = new DynamicSQLinq("Car")
                            .OrderByDescending("Make")
                            .OrderByDescending("Color DESC")
                            .OrderBy("ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car ORDER BY Make DESC, Color DESC, ID", code);
        }

        #endregion

        #region Lambda Expressions

        [TestMethod]
        public void DynamicSQLinq_Lambda_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<string>("[Make]", d => d.StartsWith("F"));

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE [Make] LIKE @sqlinq_1", code);

            Assert.AreEqual("F%", sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Lambda_002()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<string>("[Make]", d => d == "Ford");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE [Make] = @sqlinq_1", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Lambda_003()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<int>("[Color]", d => d > 1);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE [Color] > @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Lambda_004()
        {
            var query = new DynamicSQLinq("[Car]")
                            .Select("[Car].[ID]")
                            .Where<int>("[Color]", d => d > 1)
                            .Where<string>("[Make]", d => d == "Ford");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT [Car].[ID] FROM [Car] WHERE ([Color] > @sqlinq_1) AND ([Make] = @sqlinq_2)", code);

            Assert.AreEqual(1, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Lambda_005()
        {
            var MakeObject = new {
                Name = "Ford"
            };

            var query = new DynamicSQLinq("[Car]")
                            .Select("[Car].[ID]")
                            .Where<int>("[Color]", d => d > 1)
                            .Where<string>("[Make]", d => d == MakeObject.Name);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT [Car].[ID] FROM [Car] WHERE ([Color] > @sqlinq_1) AND ([Make] = @sqlinq_2)", code);

            Assert.AreEqual(1, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Lambda_006()
        {
            var MakeObject = new
            {
                NameObject = new
                {
                    Name = "Ford"
                }
            };

            var query = new DynamicSQLinq("[Car]")
                            .Select("[Car].[ID]")
                            .Where<int>("[Color]", d => d > 1)
                            .Where<string>("[Make]", d => d == MakeObject.NameObject.Name);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT [Car].[ID] FROM [Car] WHERE ([Color] > @sqlinq_1) AND ([Make] = @sqlinq_2)", code);

            Assert.AreEqual(1, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Lambda_007()
        {
            var query = new DynamicSQLinq("[Car]")
                            .Select("ID")
                            .Where<ICar>("[WheelDiameter]", d => d.WheelDiameter > 16);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT ID FROM [Car] WHERE [WheelDiameter] > @sqlinq_1", code);

            Assert.AreEqual(16, sql.Parameters["@sqlinq_1"]);
        }

        #endregion

        #region Group By

        [TestMethod]
        public void DynamicSQLinq_GroupBy_001()
        {
            var query = new DynamicSQLinq("Car")
                        .Select("ID")
                        .GroupBy("Make");
            var sql = query.ToSQL();
            var code = sql.ToQuery();

            Assert.AreEqual("SELECT ID FROM Car GROUP BY Make", code);
        }

        [TestMethod]
        public void DynamicSQLinq_GroupBy_002()
        {
            var query = new DynamicSQLinq("Car")
                        .Select("ID", "Name", "Make")
                        .Where("Make = @0", "Ford")
                        .GroupBy("Make")
                        .OrderByDescending("ID");
            var sql = query.ToSQL();
            var code = sql.ToQuery();

            Assert.AreEqual("SELECT ID, Name, Make FROM Car WHERE Make = @sqlinq_1 GROUP BY Make ORDER BY ID DESC", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
        }

        #endregion

        #region Having

        [TestMethod]
        public void DynamicSQLinq_Having_001()
        {
            var query = new DynamicSQLinq("Car")
                        .Select("Make").Select("SUM(Make)")
                        .GroupBy("Make")
                        .Having("Make = @0", "Ford");
            var sql = query.ToSQL();
            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Make, SUM(Make) FROM Car GROUP BY Make HAVING Make = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Having_002()
        {
            var query = new DynamicSQLinq("Car")
                        .Select("Make").Select("SUM(Make)")
                        .GroupBy("Make")
                        .Having<string>("Make", d => d == "Ford");
            var sql = query.ToSQL();
            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Make, SUM(Make) FROM Car GROUP BY Make HAVING [Make] = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
        }

        #endregion

        #region Take and Skip

        [TestMethod]
        public void DynamicSQLinq_Take_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID")
                            .Take(15);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT TOP 15 ID FROM Car", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Skip_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID")
                            .OrderBy("Make")
                            .Skip(15);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("WITH SQLinq_data_set AS (SELECT ID, ROW_NUMBER() OVER (ORDER BY Make) AS [SQLinq_row_number] FROM Car) SELECT * FROM SQLinq_data_set WHERE [SQLinq_row_number] >= 16", code);
        }

        [TestMethod]
        public void DynamicSQLinq_SkipTake_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID")
                            .OrderBy("Make")
                            .Skip(15).Take(10);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("WITH SQLinq_data_set AS (SELECT ID, ROW_NUMBER() OVER (ORDER BY Make) AS [SQLinq_row_number] FROM Car) SELECT * FROM SQLinq_data_set WHERE [SQLinq_row_number] BETWEEN 16 AND 25", code);
        }

        [TestMethod]
        public void DynamicSQLinq_SkipTake_002()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID")
                            .Where("Color = @0", 1)
                            .OrderBy("Make")
                            .Skip(15).Take(10);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("WITH SQLinq_data_set AS (SELECT ID, ROW_NUMBER() OVER (ORDER BY Make) AS [SQLinq_row_number] FROM Car WHERE Color = @sqlinq_1) SELECT * FROM SQLinq_data_set WHERE [SQLinq_row_number] BETWEEN 16 AND 25", code);
        }

        #endregion

        #region JOIN

        [TestMethod]
        public void DynamicSQLinq_Join_001()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Where<int>("Contact.ID", d => d == id)
                            .Join("Organization", "Contact.ID = Organization.ID").Select("Organization.ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact JOIN Organization ON Contact.ID = Organization.ID WHERE Contact.ID = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_002()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Join("Organization", "Contact.ID = Organization.ID AND Organization.ID = @0", 12).Select("Organization.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact JOIN Organization ON Contact.ID = Organization.ID AND Organization.ID = @sqlinq_1 WHERE Contact.ID = @sqlinq_2", code);

            Assert.AreEqual(2, sql.Parameters.Count);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_003()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Where<int>("Contact.ID", d => d == id)
                            .LeftJoin("Organization", "Contact.ID = Organization.ID").Select("Organization.ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact LEFT JOIN Organization ON Contact.ID = Organization.ID WHERE Contact.ID = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_004()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .LeftJoin("Organization", "Contact.ID = Organization.ID AND Organization.ID = @0", 12).Select("Organization.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact LEFT JOIN Organization ON Contact.ID = Organization.ID AND Organization.ID = @sqlinq_1 WHERE Contact.ID = @sqlinq_2", code);

            Assert.AreEqual(2, sql.Parameters.Count);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_005()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Where<int>("Contact.ID", d => d == id)
                            .RightJoin("Organization", "Contact.ID = Organization.ID").Select("Organization.ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact RIGHT JOIN Organization ON Contact.ID = Organization.ID WHERE Contact.ID = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_006()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .RightJoin("Organization", "Contact.ID = Organization.ID AND Organization.ID = @0", 12).Select("Organization.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact RIGHT JOIN Organization ON Contact.ID = Organization.ID AND Organization.ID = @sqlinq_1 WHERE Contact.ID = @sqlinq_2", code);

            Assert.AreEqual(2, sql.Parameters.Count);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_007()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Where<int>("Contact.ID", d => d == id)
                            .FullJoin("Organization", "Contact.ID = Organization.ID").Select("Organization.ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact FULL JOIN Organization ON Contact.ID = Organization.ID WHERE Contact.ID = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_008()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .FullJoin("Organization", "Contact.ID = Organization.ID AND Organization.ID = @0", 12).Select("Organization.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact FULL JOIN Organization ON Contact.ID = Organization.ID AND Organization.ID = @sqlinq_1 WHERE Contact.ID = @sqlinq_2", code);

            Assert.AreEqual(2, sql.Parameters.Count);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_009()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Where<int>("Contact.ID", d => d == id)
                            .Join("Organization", DynamicSQLinqJoinOperator.Full, "Contact.ID = Organization.ID").Select("Organization.ID");

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact FULL JOIN Organization ON Contact.ID = Organization.ID WHERE Contact.ID = @sqlinq_1", code);

            Assert.AreEqual(1, sql.Parameters.Count);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_1"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_010()
        {
            var id = 15;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Join("Organization", DynamicSQLinqJoinOperator.Left, "Contact.ID = Organization.ID AND Organization.ID = @0", 12).Select("Organization.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Organization.ID FROM Contact LEFT JOIN Organization ON Contact.ID = Organization.ID AND Organization.ID = @sqlinq_1 WHERE Contact.ID = @sqlinq_2", code);

            Assert.AreEqual(2, sql.Parameters.Count);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_011()
        {
            var id = 15;

            var subquery = new DynamicSQLinq("Organization")
                                .Select("ID", "Name");

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Join(subquery, "Org", DynamicSQLinqJoinOperator.Inner, "Contact.ID = Org.ID AND Org.ID = @0", 12).Select("Org.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Org.ID FROM Contact JOIN (SELECT ID, Name FROM Organization) AS Org ON Contact.ID = Org.ID AND Org.ID = @sqlinq_1 WHERE Contact.ID = @sqlinq_2", code);

            Assert.AreEqual(2, sql.Parameters.Count);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_012()
        {
            var id = 15;

            var subquery = new DynamicSQLinq("Organization")
                                .Where("Organization.ID = @0", 42)
                                .Select("ID", "Name");

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Join(subquery, "Org", DynamicSQLinqJoinOperator.Inner, "Contact.ID = Org.ID AND Org.ID = @0", 12).Select("Org.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Org.ID FROM Contact JOIN (SELECT ID, Name FROM Organization WHERE Organization.ID = @sqlinq_1) AS Org ON Contact.ID = Org.ID AND Org.ID = @sqlinq_2 WHERE Contact.ID = @sqlinq_3", code);

            Assert.AreEqual(3, sql.Parameters.Count);
            Assert.AreEqual(42, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(12, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Join_013()
        {
            var id = 15;
            var subqueryid = Guid.NewGuid();

            var subquery = from d in new SQLinq.SQLinq<Person>()
                           where d.ID == subqueryid
                           select d.ID;

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Join(subquery, "P", DynamicSQLinqJoinOperator.Inner, "Contact.ID = P.ID AND P.ID = @0", Guid.Empty).Select("P.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, P.ID FROM Contact JOIN (SELECT [ID] FROM [Person] WHERE [ID] = @sqlinq_1) AS P ON Contact.ID = P.ID AND P.ID = @sqlinq_2 WHERE Contact.ID = @sqlinq_3", code);

            Assert.AreEqual(3, sql.Parameters.Count);
            Assert.AreEqual(subqueryid, sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(Guid.Empty, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_3"]);
        }

        #endregion

        #region ParameterNamePrefix

        [TestMethod]
        public void DynamicSQLinq_ParameterNamePrefix_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Where("Make = @0", "Ford")
                            .Where("Color = @0 OR Color = @1", 0, 1);

            var sql = query.ToSQL(parameterNamePrefix: "mysql");

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT * FROM Car WHERE (Make = @mysql1) AND (Color = @mysql2 OR Color = @mysql3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@mysql1"]);
            Assert.AreEqual(0, sql.Parameters["@mysql2"]);
            Assert.AreEqual(1, sql.Parameters["@mysql3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_ParameterNamePrefix_002()
        {
            var id = 15;

            var subquery = new DynamicSQLinq("Organization")
                                .Where("Organization.ID = @0", 42)
                                .Select("ID", "Name");

            var query = new DynamicSQLinq("Contact")
                            .Select("Contact.ID", "Contact.FirstName", "Contact.LastName")
                            .Join(subquery, "Org", DynamicSQLinqJoinOperator.Inner, "Contact.ID = Org.ID AND Org.ID = @0", 12).Select("Org.ID")
                            .Where<int>("Contact.ID", d => d == id);

            var sql = query.ToSQL(parameterNamePrefix: "mysql");

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT Contact.ID, Contact.FirstName, Contact.LastName, Org.ID FROM Contact JOIN (SELECT ID, Name FROM Organization WHERE Organization.ID = @mysql1) AS Org ON Contact.ID = Org.ID AND Org.ID = @mysql2 WHERE Contact.ID = @mysql3", code);

            Assert.AreEqual(3, sql.Parameters.Count);
            Assert.AreEqual(42, sql.Parameters["@mysql1"]);
            Assert.AreEqual(12, sql.Parameters["@mysql2"]);
            Assert.AreEqual(15, sql.Parameters["@mysql3"]);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DynamicSQLinq_ParameterNamePrefix_003()
        {
            var query = new DynamicSQLinq("Car").Where("Make = @0", "Ford");
            var sql = query.ToSQL(parameterNamePrefix: "");
        }

        #endregion

        #region SELECT COUNT

        [TestMethod]
        public void DynamicSQLinq_Test_Count_001()
        {
            var query = new DynamicSQLinq("Car")
                            .Where("Make = @0", "Ford")
                            .Where("Color = @0 OR Color = @1", 0, 1)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE (Make = @sqlinq_1) AND (Color = @sqlinq_2 OR Color = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_002()
        {
            var where = new DynamicSQLinqExpressionCollection();
            where.Add("Make = @0", "Ford")
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var query = new DynamicSQLinq("Car")
                            .Where(where)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE (Make = @sqlinq_1) AND (Color = @sqlinq_2 OR Color = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_003()
        {
            var where = new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or);
            where.Add("Make = @0", "Ford")
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var query = new DynamicSQLinq("Car")
                            .Where(where)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE (Make = @sqlinq_1) OR (Color = @sqlinq_2 OR Color = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_004()
        {
            var where = new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or);
            where.Add("Make = @0", "Ford")
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var where2 = new DynamicSQLinqExpressionCollection();
            where2.Add("Make = @0", "Chevrolet");

            where.Add(where2);

            var query = new DynamicSQLinq("Car")
                            .Where(where)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE (Make = @sqlinq_1) OR (Color = @sqlinq_2 OR Color = @sqlinq_3) OR (Make = @sqlinq_4)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual("Chevrolet", sql.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_005()
        {
            var query = new DynamicSQLinq("Car")
                            .Select("ID", "Make").Select("Color")
                            .Where(new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or)
                            {
                                new DynamicSQLinqExpression("Make = @0", "Ford"),
                                new DynamicSQLinqExpression("Color = @0 OR Color = @1", 0, 1),

                                new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.And)
                                {
                                    new DynamicSQLinqExpression("Make = @0", "Chevrolet"),
                                    new DynamicSQLinqExpression("Color = @0", 2),
                                }
                            })
                            .OrderBy("Make").OrderBy("Color", "ID")
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE (Make = @sqlinq_1) OR (Color = @sqlinq_2 OR Color = @sqlinq_3) OR ((Make = @sqlinq_4) AND (Color = @sqlinq_5)) ORDER BY Make, Color, ID", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual("Chevrolet", sql.Parameters["@sqlinq_4"]);
            Assert.AreEqual(2, sql.Parameters["@sqlinq_5"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_006()
        {
            var where = new DynamicSQLinqExpressionCollection(DynamicSQLinqWhereOperator.Or);
            where.Add("(Make = @0) OR (Make <> @0 AND Color = @1)", "Ford", 15)
                 .Add("Color = @0 OR Color = @1", 0, 1);

            var where2 = new DynamicSQLinqExpressionCollection();
            where2.Add("Make = @0", "Chevrolet");

            where.Add(where2);

            var query = new DynamicSQLinq("Car")
                            .Where(where)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE ((Make = @sqlinq_1) OR (Make <> @sqlinq_1 AND Color = @sqlinq_2)) OR (Color = @sqlinq_3 OR Color = @sqlinq_4) OR (Make = @sqlinq_5)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(15, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_4"]);
            Assert.AreEqual("Chevrolet", sql.Parameters["@sqlinq_5"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_007()
        {
            var query = new DynamicSQLinq("Car")
                            .Where("Make = @0", "Ford")
                            .Where("Color = @0 OR Color = @1", 0, 1)
                            .Where<string>("[Make]", d => d.StartsWith("F"))
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE (Make = @sqlinq_1) AND (Color = @sqlinq_2 OR Color = @sqlinq_3) AND ([Make] LIKE @sqlinq_4)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
            Assert.AreEqual("F%", sql.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_008()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<string>("Make", d => d == null)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE [Make] IS NULL", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_009()
        {
            var query = new DynamicSQLinq("Car")
                            .Where<string>("Make", d => d != null)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE [Make] IS NOT NULL", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_010()
        {
            string nullVal = null;
            var query = new DynamicSQLinq("Car")
                            .Where<string>("Make", d => d == nullVal)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM Car WHERE [Make] IS NULL", code);
        }

        [TestMethod]
        public void DynamicSQLinq_Test_Count_011()
        {
            var query = new DynamicSQLinq("[Car]")
                            .Where("[Make] = @0", "Ford")
                            .Where("[Color] = @0 OR [Color] = @1", 0, 1)
                            .Count();

            var sql = query.ToSQL();

            var code = sql.ToQuery();

            Assert.AreEqual("SELECT COUNT(1) FROM [Car] WHERE ([Make] = @sqlinq_1) AND ([Color] = @sqlinq_2 OR [Color] = @sqlinq_3)", code);

            Assert.AreEqual("Ford", sql.Parameters["@sqlinq_1"]);
            Assert.AreEqual(0, sql.Parameters["@sqlinq_2"]);
            Assert.AreEqual(1, sql.Parameters["@sqlinq_3"]);
        }

        #endregion
    }
}
