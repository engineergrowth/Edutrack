using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using mobile.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace mobile.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wgu_tracker.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // Creating tables for Term, Course, and Assessment
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

        public Task<int> AddAsync<T>(T entity) where T : BaseModel
        {
            return _database.InsertAsync(entity);
        }

        public Task<int> UpdateAsync<T>(T entity) where T : BaseModel
        {
            return _database.UpdateAsync(entity);
        }

        public Task<int> DeleteAsync<T>(T entity) where T : BaseModel
        {
            return _database.DeleteAsync(entity);
        }

        public Task<List<T>> GetAllAsync<T>() where T : BaseModel, new()
        {
            return _database.Table<T>().ToListAsync();
        }

        public Task<Term> GetTermByIdAsync(int termId) =>
            _database.Table<Term>().FirstOrDefaultAsync(t => t.Id == termId);

        public Task<Course> GetCourseByIdAsync(int courseId) =>
            _database.Table<Course>().FirstOrDefaultAsync(c => c.Id == courseId);

        public Task<Assessment> GetAssessmentByIdAsync(int assessmentId) =>
            _database.Table<Assessment>().FirstOrDefaultAsync(a => a.Id == assessmentId);

        public Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> predicate) where T : new()
        {
            return _database.Table<T>().Where(predicate).ToListAsync();
        }

        public Task<List<Assessment>> GetAssessmentsByTypeAsync(int courseId, string assessmentType)
        {
            return GetAsync<Assessment>(a => a.CourseID == courseId && a.Type == assessmentType);
        }

        public async Task SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.Id > 0)
            {
                // Existing assessment, update it
                await UpdateAsync(assessment);
            }
            else
            {
                // New assessment, add it
                await AddAsync(assessment);
            }
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return DeleteAsync(assessment);
        }

        public Task<int> UpdateCourseAsync(Course course)
        {
            return UpdateAsync(course);
        }

        // New method to search courses by instructor's name
        public Task<List<Course>> SearchCoursesByInstructorAsync(string instructorName)
        {
            return _database.Table<Course>()
                            .Where(c => c.InstructorName.ToLower().Contains(instructorName.ToLower()))
                            .ToListAsync();
        }

        // Sample Data Initialization
        public async Task InitializeSampleData()
        {
            // Insert sample term
            var term1 = new Term
            {
                Title = "Fall 2024",
                StartDate = new DateTime(2024, 8, 15),
                EndDate = new DateTime(2024, 12, 15)
            };

            // Insert term into the database
            await AddAsync(term1);  // SQLite auto-generates Id

            // Fetch the term using its Id (this is guaranteed to return the right term)
            var insertedTerm = await _database.Table<Term>().Where(t => t.Id == term1.Id).FirstOrDefaultAsync();

            if (insertedTerm == null)
            {
                // Alert if term insertion failed
                await App.Current.MainPage.DisplayAlert("Error", "Failed to insert term.", "OK");
                return;
            }


            // Insert sample course linked to the term
            var course1 = new Course
            {
                TermID = insertedTerm.Id,  // Use the correct term ID
                Title = "Software Engineering I",
                StartDate = new DateTime(2024, 8, 20),
                EndDate = new DateTime(2024, 12, 10),
                Status = "In Progress",
                InstructorName = "Anika Patel",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                InstructorPhone = "555-123-4567",
                Notes = "This course focuses on the fundamentals of software engineering, including design, development, and testing."
            };

            await AddAsync(course1);  // Insert course, SQLite auto-generates Id


            // Insert sample assessments linked to the course
            var assessment1 = new Assessment
            {
                CourseID = course1.Id,  // Link assessment to the inserted course
                Title = "Midterm Exam",
                Type = "Objective",
                StartDate = new DateTime(2024, 10, 1),
                EndDate = new DateTime(2024, 10, 5),
                NotificationEnabled = true
            };
            await AddAsync(assessment1);  // Insert assessment

            var assessment2 = new Assessment
            {
                CourseID = course1.Id,  // Link assessment to the inserted course
                Title = "Project Presentation",
                Type = "Performance",
                StartDate = new DateTime(2024, 11, 15),
                EndDate = new DateTime(2024, 11, 15),
                NotificationEnabled = true
            };
            await AddAsync(assessment2);  // Insert second assessment

            var sampleUser = new User
            {
                Username = "test",
                Password = "test" 
            };

            var existingUser = await _database.Table<User>().FirstOrDefaultAsync(u => u.Username == sampleUser.Username);
            if (existingUser == null)
            {
                await AddUserAsync(sampleUser);
            }


        }
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            return await _database.Table<User>().FirstOrDefaultAsync(user =>
                user.Username == username && user.Password == password);
        }

        public async Task AddUserAsync(User user)
        {
            await _database.InsertAsync(user);
        }

        public async Task<List<Course>> GetCoursesByStatusAsync(string status)
        {
            // Ensure the status is not null or empty
            if (string.IsNullOrEmpty(status))
            {
                throw new ArgumentException("Status cannot be null or empty.", nameof(status));
            }

            // Fetch courses with the specified status from the database
            var courses = await _database.Table<Course>().Where(c => c.Status == status).ToListAsync();
            return courses;
        }

        // panopto video recording url: https://wgu.hosted.panopto.com/Panopto/Pages/Viewer.aspx?id=b64218ff-761c-459e-8cbf-b238006780b5


    }
}
