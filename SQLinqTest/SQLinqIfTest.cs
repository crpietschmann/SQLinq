//Copyright (c) Chris Pietschmann 2013 (http://pietschsoft.com)
//Licensed under the GNU Library General Public License (LGPL)
//License can be found here: http://sqlinq.codeplex.com/license

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLinq;
using SQLinq.Dynamic;
using System;

namespace SQLinqTest
{
    [TestClass]
    public class SQLinqIfTest
    {
        [TestMethod]
        public void SQLinqIf_001()
        {
            var query = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, query);
            @if.Then = query;

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (EXISTS(SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_1))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_2
END
";

            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [TestMethod]
        public void SQLinqIf_002()
        {
            var query = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, query);
            @if.Else = query;

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (NOT(EXISTS(SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_1)))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_2
END
";
            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [TestMethod]
        public void SQLinqIf_003()
        {
            var query = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);
            var @if = new SQLinqIf("1 = 1");
            @if.Else = query;

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (NOT(1 = 1))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_1
END
";
            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [TestMethod]
        public void SQLinqIf_004()
        {
            var query = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);
            var @if = new SQLinqIf(SQLinqIfOperator.Not, "1 = 1");
            @if.Then = query;

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (NOT(1 = 1))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_1
END
";
            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [TestMethod]
        public void SQLinqIf_005()
        {
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, (from d in new SQLinq<Person>()
                         where d.Age >= 19
                         select d));
            @if.Then = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);

            var result = @if.ToSQL();

            Assert.AreEqual(2, result.Parameters.Count);
            Assert.AreEqual(19, result.Parameters["@sqlinq_1"]);
            Assert.AreEqual(18, result.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void SQLinqIf_006()
        {
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, (from d in new SQLinq<Person>()
                                                             where d.Age >= 19
                                                             select d));
            @if.Then = (from d in new SQLinq<Person>()
                        where d.Age >= 18
                        select d);

            var result = @if.ToSQL(5);

            Assert.AreEqual(2, result.Parameters.Count);
            Assert.AreEqual(19, result.Parameters["@sqlinq_6"]);
            Assert.AreEqual(18, result.Parameters["@sqlinq_7"]);
        }

        [TestMethod]
        public void SQLinqIf_007()
        {
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, (from d in new SQLinq<Person>()
                                                             where d.Age >= 19
                                                             select d));
            @if.Then = (from d in new SQLinq<Person>()
                        where d.Age >= 18
                        select d);

            var result = @if.ToSQL(parameterNamePrefix: "iff");

            Assert.AreEqual(2, result.Parameters.Count);
            Assert.AreEqual(19, result.Parameters["@iff1"]);
            Assert.AreEqual(18, result.Parameters["@iff2"]);
        }

        [TestMethod]
        public void SQLinqIf_008()
        {
            var query = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);
            var @if = new SQLinqIf("1 = 1");
            @if.Then = query;

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (1 = 1)
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_1
END
";
            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [TestMethod]
        public void SQLinqIf_009()
        {
            var query = (from d in new SQLinq<Person>()
                         where d.Age >= 18
                         select d);
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, query);
            @if.Then = new DynamicSQLinq("Car")
                            .Where("Make = @0", "Ford")
                            .Where("Color = @0 OR Color = @1", 0, 1);

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (EXISTS(SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [Age] >= @sqlinq_1))
BEGIN
SELECT * FROM Car WHERE (Make = @sqlinq_2) AND (Color = @sqlinq_3 OR Color = @sqlinq_4)
END
";
            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(18, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual("Ford", actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(1, actual.Parameters["@sqlinq_4"]);
        }

        [TestMethod]
        public void SQLinqIf_010()
        {
            var personId = Guid.NewGuid();
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, from p in new SQLinq<Person>()
                                                            where p.ID == personId
                                                            select p.ID);
            @if.Else = new SQLinqInsert<Person>(new Person());

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (NOT(EXISTS(SELECT [ID] FROM [Person] WHERE [ID] = @sqlinq_1)))
BEGIN
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_2, @sqlinq_3, @sqlinq_4, @sqlinq_5, @sqlinq_6, @sqlinq_7, @sqlinq_8)
END
";

            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(8, actual.Parameters.Count);
            Assert.AreEqual(personId, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_7"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_8"]);
        }

        [TestMethod]
        public void SQLinqIf_011()
        {
            var personId = Guid.NewGuid();
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, from p in new SQLinq<Person>()
                                                            where p.ID == personId
                                                            select p.ID);
            @if.Then = new SQLinqInsert<Person>(new Person());

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (EXISTS(SELECT [ID] FROM [Person] WHERE [ID] = @sqlinq_1))
BEGIN
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_2, @sqlinq_3, @sqlinq_4, @sqlinq_5, @sqlinq_6, @sqlinq_7, @sqlinq_8)
END
";

            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(8, actual.Parameters.Count);
            Assert.AreEqual(personId, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_7"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_8"]);
        }

        [TestMethod]
        public void SQLinqIf_012()
        {
            var personId = Guid.NewGuid();
            var @if = new SQLinqIf(SQLinqIfOperator.Exists, from p in new SQLinq<Person>()
                                                            where p.ID == personId
                                                            select p.ID);
            @if.Then = new SQLinqUpdate<Person>(new Person());
            @if.Else = new SQLinqInsert<Person>(new Person());

            var actual = @if.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"IF (EXISTS(SELECT [ID] FROM [Person] WHERE [ID] = @sqlinq_1))
BEGIN
UPDATE [Person] SET [ID] = @sqlinq_2, [FirstName] = @sqlinq_3, [LastName] = @sqlinq_4, [Age] = @sqlinq_5, [Is_Employed] = @sqlinq_6, [ParentID] = @sqlinq_7, [Column With Spaces] = @sqlinq_8
END
ELSE
BEGIN
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_9, @sqlinq_10, @sqlinq_11, @sqlinq_12, @sqlinq_13, @sqlinq_14, @sqlinq_15)
END
";

            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(15, actual.Parameters.Count);
            Assert.AreEqual(personId, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_7"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_8"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_9"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_10"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_11"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_12"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_13"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_14"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_15"]);
        }
    }
}
