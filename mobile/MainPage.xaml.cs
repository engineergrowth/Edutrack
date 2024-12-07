using System;
using System.Collections.Generic;
using System.Linq;
using mobile.Models;
using mobile.Services;

namespace mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public MainPage()
        {
            InitializeComponent();

            // Initialize the DatabaseService
            _databaseService = new DatabaseService();

            // Run database tests
            TestDatabaseAsync();
        }

        private async void TestDatabaseAsync()
        {
            try
            {
                // Initialize sample data
                await _databaseService.InitializeSampleData();

                // Fetch all terms
                var terms = await _databaseService.GetAllAsync<Term>();
                Console.WriteLine("=== Terms ===");
                foreach (var term in terms)
                {
                    Console.WriteLine($"Term: {term.Title}, Start: {term.StartDate}, End: {term.EndDate}");

                    // Fetch courses for each term
                    var courses = await _databaseService.GetAsync<Course>(c => c.TermID == term.Id);



                    foreach (var course in courses)
                    {
                        Console.WriteLine($"  Course: {course.Title}, Status: {course.Status}");
                        Console.WriteLine($"    Instructor: {course.InstructorName}, Email: {course.InstructorEmail}, Phone: {course.InstructorPhone}");

                        // Fetch assessments for each course
                        var assessments = await _databaseService.GetAsync<Assessment>(a => a.CourseID == course.Id);

                        foreach (var assessment in assessments)
                        {
                            Console.WriteLine($"    Assessment: {assessment.Title}, Type: {assessment.Type}, Notifications: {assessment.NotificationEnabled}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error testing database: {ex.Message}");
            }
        }
    }
}

