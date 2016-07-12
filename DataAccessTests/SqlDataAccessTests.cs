using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace DataAccess.Tests
{
    [TestClass()]
    public class SqlDataAccessTests
    {
        public SqlDataAccessTests()
        {
            

        }
        
        [TestMethod]
        public void InsertTest()
        {
            using (var dataAccess = new SqlDataAccess("data source=.;initial catalog=Peraport_Test;integrated security=true;"))
            {
                var person = new Person
                {
                    Name = "Osman",
                    LastName = "Türer",
                    Birthdate = new DateTime(1979, 09, 16),
                    Description = "Test açıklaması" };
                dataAccess.Insert(person);
                dataAccess.Commit();
                Assert.IsNotNull(person);
                Assert.IsTrue((long)person.Id > 0);
            }
        }


        [TestMethod]
        public void UpdateTest()
        {
            using (var dataAccess = new SqlDataAccess("data source=.;initial catalog=Peraport_Test;integrated security=true;"))
            {
                var person = new Person
                {
                    Id = 1,
                    Name = "Osman",
                    LastName = "Güler",
                    Birthdate = new DateTime(1977, 04, 10),
                    Description = "Test açıklaması"
                };
                dataAccess.Update(person);
                dataAccess.Commit();

            }

        }


        [TestMethod]
        public void DeleteTest()
        {
            using (var dataAccess = new SqlDataAccess("data source=.;initial catalog=Peraport_Test;integrated security=true;"))
            {
                var person = new Person
                {
                    Id = 6,
                    Name = "Osman",
                    LastName = "Güler",
                    Birthdate = new DateTime(1977, 04, 10),
                    Description = "Test açıklaması"
                };
                dataAccess.Delete(person);
                dataAccess.Commit();

            }

        }

        [TestMethod]
        public void GetallTest()
        {
            using (var dataAccess = new SqlDataAccess("data source=.;initial catalog=Peraport_Test;integrated security=true;"))
            {

                var r = dataAccess.GetAll<Person>();
                

            }

        }
    }

    public class Person: BaseEntity
    {

        [Field(DbType =DbType.String,IsNullable =true,Name ="Name")]
        public string Name { get; set; }

        [Field(DbType = DbType.String, IsNullable = true, Name = "LastName")]
        public string LastName { get; set; }

        [Field(DbType = DbType.DateTime, IsNullable = true, Name = "Birthdate")]
        public DateTime? Birthdate { get; set; }

        [Field(DbType = DbType.String, IsNullable = true, Name = "Description")]
        public string Description { get; set; }

        


    }
}