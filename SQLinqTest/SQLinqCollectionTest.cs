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
    public class SQLinqCollectionTest
    {
        [TestMethod]
        public void Constructor_001()
        {
            var target = new SQLinqCollection();
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(ISQLinq));
            Assert.IsInstanceOfType(target, typeof(List<ISQLinq>));
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void Constructor_002()
        {
            var target = new SQLinqCollection(
                new SQLinq<Person>()
                );
            Assert.AreEqual(1, target.Count);
        }

        [TestMethod]
        public void Constructor_003()
        {
            var target = new SQLinqCollection(
                new SQLinq<Person>(),
                new SQLinq<ICar>()
                );
            Assert.AreEqual(2, target.Count);
        }

        [TestMethod]
        public void Constructor_004()
        {
            var target = new SQLinqCollection(
                new ISQLinq[] { new SQLinq<Person>() }
                );
            Assert.AreEqual(1, target.Count);
        }

        [TestMethod]
        public void Constructor_005()
        {
            var target = new SQLinqCollection(
                new ISQLinq[] { new SQLinq<Person>(), new SQLinq<ICar>() }
                );
            Assert.AreEqual(2, target.Count);
        }

        [TestMethod]
        public void Constructor_006()
        {
            var list = new List<ISQLinq> { new SQLinq<Person>(), new SQLinq<ICar>() };
            var target = new SQLinqCollection(list);
            Assert.AreEqual(2, target.Count);
        }

        [TestMethod]
        public void Constructor_007()
        {
            IEnumerable<ISQLinq> list = new List<ISQLinq> { new SQLinq<Person>(), new SQLinq<ICar>() };
            var target = new SQLinqCollection(list);
            Assert.AreEqual(2, target.Count);
        }

        [TestMethod]
        public void Add_001()
        {
            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>());
            Assert.AreEqual(1, target.Count);
        }

        [TestMethod]
        public void ToSQL_001()
        {
            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>());

            var actual = (SQLinqCollectionResult)target.ToSQL();

            Assert.AreEqual(1, actual.Queries.Count);
            Assert.AreEqual("SELECT * FROM [ICar]", actual.Queries[0]);
        }

        [TestMethod]
        public void ToSQL_Oracle_001()
        {
            var dialect = new OracleDialect();

            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>(dialect));

            var actual = (SQLinqCollectionResult)target.ToSQL();

            Assert.AreEqual(1, actual.Queries.Count);
            Assert.AreEqual("SELECT * FROM ICar", actual.Queries[0]);
        }

        [TestMethod]
        public void ToSQL_002()
        {
            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 21));
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 14));

            var actual = (SQLinqCollectionResult)target.ToSQL();

            Assert.AreEqual(2, actual.Queries.Count);
            Assert.AreEqual("SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_1", actual.Queries[0]);
            Assert.AreEqual("SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_2", actual.Queries[1]);

            Assert.AreEqual(2, actual.Parameters.Count);
            Assert.AreEqual(21, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(14, actual.Parameters["@sqlinq_2"]);
        }

        [TestMethod]
        public void ToSQL_Oracle_002()
        {
            var dialect = new OracleDialect();

            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>(dialect).Where(d => d.WheelDiameter == 21));
            target.Add(new SQLinq<ICar>(dialect).Where(d => d.WheelDiameter == 14));

            var actual = (SQLinqCollectionResult)target.ToSQL();

            Assert.AreEqual(2, actual.Queries.Count);
            Assert.AreEqual("SELECT * FROM ICar WHERE WheelDiameter = :sqlinq_1", actual.Queries[0]);
            Assert.AreEqual("SELECT * FROM ICar WHERE WheelDiameter = :sqlinq_2", actual.Queries[1]);

            Assert.AreEqual(2, actual.Parameters.Count);
            Assert.AreEqual(21, actual.Parameters[":sqlinq_1"]);
            Assert.AreEqual(14, actual.Parameters[":sqlinq_2"]);
        }

        [TestMethod]
        public void ToSQL_003()
        {
            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 21));
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 14));

            var actual = target.ToSQL().ToQuery();

            Assert.AreEqual(@"SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_1
SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_2
", actual);
        }

        [TestMethod]
        public void ToSQL_004()
        {
            var guid = Guid.NewGuid();

            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 21));
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 14));
            target.Add(new SQLinqUpdate<Person>(new Person()).Where(d => d.ID == guid));
            target.Add(new SQLinqInsert<Person>(new Person()));

            var actual = target.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_1
SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_2
UPDATE [Person] SET [ID] = @sqlinq_3, [FirstName] = @sqlinq_4, [LastName] = @sqlinq_5, [Age] = @sqlinq_6, [Is_Employed] = @sqlinq_7, [ParentID] = @sqlinq_8, [Column With Spaces] = @sqlinq_9 WHERE [ID] = @sqlinq_10
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_11, @sqlinq_12, @sqlinq_13, @sqlinq_14, @sqlinq_15, @sqlinq_16, @sqlinq_17)
";

            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(17, actual.Parameters.Count);
            Assert.AreEqual(21, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(14, actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_7"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_8"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_9"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_10"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_11"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_12"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_13"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_14"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_15"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_16"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_17"]);
        }

        [TestMethod]
        public void ToSQL_005()
        {
            var guid = Guid.NewGuid();

            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 21));
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 14));
            target.Add(new SQLinqUpdate<Person>(new Person()).Where(d => d.ID == guid));
            target.Add(new SQLinqInsert<Person>(new Person()));

            var @if = new SQLinqIf(SQLinqIfOperator.Exists, from d in new SQLinq<Person>()
                                                            where d.ID == guid
                                                            select 1);
            @if.Then = from d in new SQLinq<Person>()
                       where d.ID == guid
                       select d;
            target.Add(@if);




            var actual = target.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_1
SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_2
UPDATE [Person] SET [ID] = @sqlinq_3, [FirstName] = @sqlinq_4, [LastName] = @sqlinq_5, [Age] = @sqlinq_6, [Is_Employed] = @sqlinq_7, [ParentID] = @sqlinq_8, [Column With Spaces] = @sqlinq_9 WHERE [ID] = @sqlinq_10
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_11, @sqlinq_12, @sqlinq_13, @sqlinq_14, @sqlinq_15, @sqlinq_16, @sqlinq_17)
IF (EXISTS(SELECT @sqlinq_18 FROM [Person] WHERE [ID] = @sqlinq_19))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [ID] = @sqlinq_20
END

";

            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(20, actual.Parameters.Count);
            Assert.AreEqual(21, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(14, actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_7"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_8"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_9"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_10"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_11"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_12"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_13"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_14"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_15"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_16"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_17"]);
            Assert.AreEqual(1, actual.Parameters["@sqlinq_18"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_19"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_20"]);
        }

        [TestMethod]
        public void ToSQL_006()
        {
            var guid = Guid.NewGuid();

            var target = new SQLinqCollection();
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 21));
            target.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 14));
            target.Add(new SQLinqUpdate<Person>(new Person()).Where(d => d.ID == guid));
            target.Add(new SQLinqInsert<Person>(new Person()));

            var @if = new SQLinqIf(SQLinqIfOperator.Exists, from d in new SQLinq<Person>()
                                                            where d.ID == guid
                                                            select 1);
            @if.Then = from d in new SQLinq<Person>()
                       where d.ID == guid
                       select d;
            target.Add(@if);

            target.Add(((Func<SQLinqCollection>)(() =>
            {
                var target2 = new SQLinqCollection();
                target2.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 21));
                target2.Add(new SQLinq<ICar>().Where(d => d.WheelDiameter == 14));
                target2.Add(new SQLinqUpdate<Person>(new Person()).Where(d => d.ID == guid));
                target2.Add(new SQLinqInsert<Person>(new Person()));

                var @if2 = new SQLinqIf(SQLinqIfOperator.Exists, from d in new SQLinq<Person>()
                                                                 where d.ID == guid
                                                                 select 1);
                @if2.Then = from d in new SQLinq<Person>()
                            where d.ID == guid
                            select d;
                target2.Add(@if2);

                return target2;
            }))());
            



            var actual = target.ToSQL();

            var actualQuery = actual.ToQuery();
            var expectedQuery = @"SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_1
SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_2
UPDATE [Person] SET [ID] = @sqlinq_3, [FirstName] = @sqlinq_4, [LastName] = @sqlinq_5, [Age] = @sqlinq_6, [Is_Employed] = @sqlinq_7, [ParentID] = @sqlinq_8, [Column With Spaces] = @sqlinq_9 WHERE [ID] = @sqlinq_10
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_11, @sqlinq_12, @sqlinq_13, @sqlinq_14, @sqlinq_15, @sqlinq_16, @sqlinq_17)
IF (EXISTS(SELECT @sqlinq_18 FROM [Person] WHERE [ID] = @sqlinq_19))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [ID] = @sqlinq_20
END

SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_21
SELECT * FROM [ICar] WHERE [WheelDiameter] = @sqlinq_22
UPDATE [Person] SET [ID] = @sqlinq_23, [FirstName] = @sqlinq_24, [LastName] = @sqlinq_25, [Age] = @sqlinq_26, [Is_Employed] = @sqlinq_27, [ParentID] = @sqlinq_28, [Column With Spaces] = @sqlinq_29 WHERE [ID] = @sqlinq_30
INSERT [Person] ([ID], [FirstName], [LastName], [Age], [Is_Employed], [ParentID], [Column With Spaces]) VALUES (@sqlinq_31, @sqlinq_32, @sqlinq_33, @sqlinq_34, @sqlinq_35, @sqlinq_36, @sqlinq_37)
IF (EXISTS(SELECT @sqlinq_38 FROM [Person] WHERE [ID] = @sqlinq_39))
BEGIN
SELECT [ID], [FirstName], [LastName], [Age], [Is_Employed] AS [IsEmployed], [ParentID], [Column With Spaces] AS [ColumnWithSpaces] FROM [Person] WHERE [ID] = @sqlinq_40
END


";

            Assert.AreEqual(expectedQuery, actualQuery);

            Assert.AreEqual(40, actual.Parameters.Count);
            Assert.AreEqual(21, actual.Parameters["@sqlinq_1"]);
            Assert.AreEqual(14, actual.Parameters["@sqlinq_2"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_3"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_4"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_5"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_6"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_7"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_8"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_9"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_10"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_11"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_12"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_13"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_14"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_15"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_16"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_17"]);
            Assert.AreEqual(1, actual.Parameters["@sqlinq_18"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_19"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_20"]);

            Assert.AreEqual(21, actual.Parameters["@sqlinq_21"]);
            Assert.AreEqual(14, actual.Parameters["@sqlinq_22"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_23"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_24"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_25"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_26"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_27"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_28"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_29"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_30"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_31"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_32"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_33"]);
            Assert.AreEqual(0, actual.Parameters["@sqlinq_34"]);
            Assert.AreEqual(false, actual.Parameters["@sqlinq_35"]);
            Assert.AreEqual(Guid.Empty, actual.Parameters["@sqlinq_36"]);
            Assert.AreEqual(null, actual.Parameters["@sqlinq_37"]);
            Assert.AreEqual(1, actual.Parameters["@sqlinq_38"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_39"]);
            Assert.AreEqual(guid, actual.Parameters["@sqlinq_40"]);
        }
    }
}
