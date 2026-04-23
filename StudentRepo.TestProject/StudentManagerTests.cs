using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using StudentRepo.Models;
using System;

namespace Testing1.Tests
{
    [TestClass]
    public class StudentManagerTests
    {
        private StudentManager _manager;
        private StudentDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new StudentDbContext(options);
            _manager = new StudentManager(_context);
        }

        [TestMethod]
        public void AddStudent_NullStudent_ThrowsException()
        {
            try { _manager.AddStudent(null!); 
                Assert.Fail("Expected CustomException");
            }
            catch (Exception ex) { 
                Assert.IsTrue(ex.GetType().Name == "CustomException");
            }
        }

        [TestMethod]
        public void AddStudent_EmptyName_ThrowsException()
        {
            try { _manager.AddStudent(new Student { Name = "", Age = 20, Email = "a@a.com", PhoneNumber = "1" }); 
                Assert.Fail("Expected CustomException"); 
            }
            catch (Exception ex) { 
                Assert.IsTrue(ex.GetType().Name == "CustomException"); 
            }
        }

        [TestMethod]
        public void AddStudent_InvalidAge_ThrowsException()
        {
            try { _manager.AddStudent(new Student { Name = "John", Age = 0, Email = "a@a.com", PhoneNumber = "1" }); 
                Assert.Fail("Expected CustomException"); 
            }
            catch (Exception ex) {
                Assert.IsTrue(ex.GetType().Name == "CustomException");
            }
        }

        [TestMethod]
        public void UpdateStudent_NullStudent_ThrowsException()
        {
            try { _manager.UpdateStudent(null!); 
                Assert.Fail("Expected CustomException"); 
            }
            catch (Exception ex) {
                Assert.IsTrue(ex.GetType().Name == "CustomException"); 
            }
        }

        [TestMethod]
        public void GetStudentById_InvalidId_ThrowsException()
        {
            try { _manager.GetStudentById(0); 
                Assert.Fail("Expected CustomException"); 
            }
            catch (Exception ex) {
                Assert.IsTrue(ex.GetType().Name == "CustomException"); 
            }
        }

        [TestMethod]
        public void UpdateStudent_InvalidId_ThrowsException()
        {
            try { _manager.UpdateStudent(new Student { Id = 0, Name = "John", Age = 20, Email = "a@a.com", PhoneNumber = "1" });
                Assert.Fail("Expected CustomException");
            }
            catch (Exception ex) { 
                Assert.IsTrue(ex.GetType().Name == "CustomException");
            }
        }

        [TestMethod]
        public void DeleteStudent_InvalidId_ThrowsException()
        {
            try { _manager.DeleteStudent(0); 
                Assert.Fail("Expected CustomException"); 
            }
            catch (Exception ex) {
                Assert.IsTrue(ex.GetType().Name == "CustomException");
            }
        }
    }
}
