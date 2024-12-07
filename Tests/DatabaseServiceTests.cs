using NUnit.Framework;
using mobile.Models; 
using mobile.Services;  
using System;
using System.Threading.Tasks;

namespace mobile.Tests
{
    [TestFixture]
    public class DatabaseServiceTests
    {
        private DatabaseService _databaseService;

        [SetUp]
        public void SetUp()
        {
            _databaseService = new DatabaseService(); 
        }

        [Test]
        public async Task AddCourse_ValidData_CourseIsAdded()
        {
            var course = new Course
            {
                Title = "Software Engineering I",
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

            var result = await _databaseService.AddAsync(course);

            Assert.AreEqual(1, result); 
        }
    }
}
