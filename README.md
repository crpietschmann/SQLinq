# SQLinq
SQLinq allows yout to use LINQ (Language INtegrated Query) features of .NET to generate Ad-Hoc SQL Queries at runtime.

This allows for easy generation of ad-hoc SQL code using strongly typed LINQ code in .NET in a similar fashion to writing queries with Entity Framework. However, SQLinq is NOT an ORM. SQLinq is a library for generating ad-hoc, strongly typed SQL code at runtime. The generated SQL code can then be executed against a SQL database using either ADO.NET directly, or with Dapper.NET using the SQLinq Dapper nuget package. One of the benefits of SQLinq is that you can write ad-hoc SQL code at runtime that isn't just strongly typed, but benefits from compile time validation as well.

## Nuget Package

[http://nuget.org/packages/sqlinq](http://nuget.org/packages/sqlinq)  

[![Install SQLinq via Nuget](http://sqlinq.codeplex.com/Download?ProjectName=sqlinq&DownloadId=357830 "Install SQLinq via Nuget")](http://nuget.org/packages/sqlinq)  

## SQLinq Usage

**Step 1:** Create your data object in code (like the following examples) that matches the database table or view you want to select from. It can either be a class or interface. You can also name the object and/or its properties differently than the database by using the SQLinqTable and SQLinqColumn attributes to specify their name in the database.  

```c#
[SQLinqTable("PersonTable")]
public class Person
{
    public Guid ID { get; set; }

    [SQLinqColumn("First_Name")]
    public string FirstName { get; set; }

    [SQLinqColumn("Last_Name")]
    public string LastName { get; set; }

    public int Age { get; set; }
}
```

**Step 2:** Use LINQ to generate the ad-hoc SQL query necessary.  

```c#
var query = from d in new SQLinq<Person>()
            where d.FirstName.StartsWith("C")
                 && d.Age > 18
            orderby d.FirstName
            select new {
                id = d.ID,
                firstName = d.FirstName
            };
```

**Step 3:** Generate the SQL code and necessary query parameter key/value pairs.  

```c#
var queryResult = query.ToSQL();

// get the full SQL code
var sqlCode = queryResult.ToQuery();

// get the query parameters necessary to execute the above query
var sqlParameters = queryResult.Parameters;
```

**Step 4:** Create SqlCommand and set the SQL code and Query Parameters  

```c#
var cmd = new SqlCommand(dbconnection, sqlCode);
foreach(var p in sqlParameters)
{
    cmd.Parameters.AddWithValue(p.Key, p.Value);
}
// now execute the command and get the results from the database
```

## SQLinq.Dapper

SQLinq.Dapper is a small helper library that bridges the gap between SQLinq and [Dapper dot net](http://code.google.com/p/dapper-dot-net/) to allow for queries to be performed more easily.  

**SQLinq.Dapper Usage:**  
Here's a simple example of using SQLinq.Dapper:  

```c#
IEnumerable<Person> data = null;
using(IDbConnection con = GetDbConnection())
{
    con.Open();
    data = con.Query(
        from p in SQLinq<Person>()
        where p.FirstName.StartsWith("C") && p.Age > 21
        orderby p.FirstName
        select p
    );
    con.Close();
}

// do somthing with the data that was returned
```

**Install SQLinq.Dapper via Nuget**  
SQLinq.Dapper can also be installed into your project via Nuget!  
[http://nuget.org/packages/SQLinq.Dapper](http://nuget.org/packages/SQLinq.Dapper)  

[![Install SQLinq via Nuget](http://download.codeplex.com/Download?ProjectName=sqlinq&DownloadId=358422 "Install SQLinq via Nuget")](http://nuget.org/packages/SQLinq.Dapper)  
