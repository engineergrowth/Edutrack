using NUnit.Framework;
using mobile.Models;
using mobile.Services;
using System;
using System.Threading.Tasks; 

namespace mobile.Tests
{
    [TestFixture]
    public class DatabaseServiceInvalidTests
    {
        private DatabaseService _databaseService;

        [SetUp]
        public void SetUp()
        {
            _databaseService = new DatabaseService();
        }

        [Test]
        public async Task AddCourse_EmptyTitle_ThrowsArgumentException() 
        {
            var course = new Course
            {
                Title = "", // Empty title
                TermID = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Status = "In Progress",
                InstructorName = "Anika Patel",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                InstructorPhone = "555-123-4567",
                Notes = "This course focuses on software engineering basics.",
                EnableNotifications = true
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _databaseService.AddAsync(course)); 
            Assert.That(ex.Message, Is.EqualTo("Course title cannot be empty."));
        }
    }
}
