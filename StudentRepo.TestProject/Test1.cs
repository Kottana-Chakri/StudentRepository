

//Test1.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using StudentRepo.Models;
using StudentRepo.Interfaces;

namespace Testing1.Tests
{
    [TestClass]
    public class StudentControllerTests
    {
        private Mock<IStudentManager> _mockStudentManager;
        private StudentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockStudentManager = new Mock<IStudentManager>();
            _controller = new StudentController(_mockStudentManager.Object);
        }

        [TestMethod]
        public void Get_ReturnsOk_WithStudents()
        {
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "A" },
                new Student { Id = 2, Name = "B" }
            };

            _mockStudentManager.Setup(m => m.GetStudents()).Returns(students);

            var result = _controller.Get();
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var data = okResult.Value as List<Student>;
            Assert.IsNotNull(data);
            Assert.AreEqual(2, data.Count);
        }

        [TestMethod]
        public void GetById_ReturnsOk_WhenStudentExists()
        {
            var student = new Student { Id = 1, Name = "John" };
            _mockStudentManager.Setup(m => m.GetStudentById(1)).Returns(student);

            var result = _controller.Get(1);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var data = okResult.Value as Student;
            Assert.IsNotNull(data);
            Assert.AreEqual("John", data.Name);
        }

        [TestMethod]
        public void GetById_ReturnsNotFound_WhenStudentDoesNotExist()
        {
            _mockStudentManager.Setup(m => m.GetStudentById(1)).Returns((Student)null!);

            var result = _controller.Get(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void Post_ReturnsOk_WhenValid()
        {
            var student = new Student { Id = 1, Name = "John", Email = "j@email.com", PhoneNumber = "12345" };

            _mockStudentManager.Setup(m => m.AddStudent(student));

            var result = _controller.Post(student);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Student Added", okResult.Value);
        }

        [TestMethod]
        public void Put_ReturnsOk_WhenValid()
        {
            var student = new Student { Id = 1, Name = "John", Email = "j@email.com", PhoneNumber = "12345" };

            _mockStudentManager.Setup(m => m.UpdateStudent(student));

            var result = _controller.Put(student);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Student Updated", okResult.Value);
        }

        [TestMethod]
        public void Delete_ReturnsOk_WhenValid()
        {
            _mockStudentManager.Setup(m => m.DeleteStudent(1));

            var result = _controller.Delete(1);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Student Deleted", okResult.Value);
        }
    }
}
