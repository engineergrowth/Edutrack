using Plugin.LocalNotification;
using mobile.Models;
using mobile.Services;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace mobile.Views
{
    [QueryProperty("TermId", "termId")]  // Map the query parameter "termId" to TermId property
    public partial class AddCoursePage : ContentPage
    {
        private int _termId;
        private DatabaseService _databaseService;

        // Public property for QueryProperty binding
        public int TermId
        {
            get => _termId;
            set
            {
                _termId = value;
                Console.WriteLine($"TermId set to: {_termId}");  // Debugging
            }
        }

        public AddCoursePage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        // Use OnAppearing to handle receiving query parameters
        protected override void OnAppearing()
        {
            base.OnAppearing();

       

            // Initialize the DatePickers and StatusPicker as needed
            StartDatePicker.MinimumDate = DateTime.Now;
            EndDatePicker.MinimumDate = DateTime.Now;

            StatusPicker.ItemsSource = new List<string>
            {
                "Not Started",
                "In Progress",
                "Completed"
            };
        }

        // Save course, add notes, and schedule notification
        private async void OnSaveCourseClicked(object sender, EventArgs e)
        {
            // Check if there are already 6 courses for the term
            var existingCourses = await _databaseService.GetAsync<Course>(c => c.TermID == _termId);
            if (existingCourses.Count >= 6)
            {
                await DisplayAlert("Course Limit Reached", "You cannot add more than 6 courses to this term.", "OK");
                return;  // Prevent saving the new course
            }

            // Continue saving the course if the limit hasn't been reached
            var course = new Course
            {
                TermID = _termId,
                Title = TitleEntry.Text,
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date,
                Status = StatusPicker.SelectedItem?.ToString(),
                InstructorName = InstructorNameEntry.Text,
                InstructorEmail = InstructorEmailEntry.Text,
                InstructorPhone = InstructorPhoneEntry.Text,
                Notes = NotesEditor.Text,
                EnableNotifications = EnableNotificationsCheckBox.IsChecked
            };

            await _databaseService.AddAsync(course);


            // Schedule notification if enabled
            if (EnableNotificationsCheckBox.IsChecked)
            {
                ScheduleCourseNotification(course);
            }

            await DisplayAlert("Success", "Course saved successfully!", "OK");

            // Navigate back
            await Navigation.PopAsync();
        }


        // Function to schedule the notification
        private void ScheduleCourseNotification(Course course)
        {
            var notificationTime = course.StartDate.AddDays(-1);  // Notify 1 day before the course starts
            // If notification time is in the past, set it to the next minute
            if (notificationTime < DateTime.Now)
            {
                notificationTime = DateTime.Now.AddMinutes(1);
            }

            var notification = new NotificationRequest
            {
                NotificationId = course.Id,
                Title = $"Upcoming Course: {course.Title}",
                Description = $"Your course {course.Title} starts tomorrow. Don't forget!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notificationTime
                }
            };

            var notificationCenter = LocalNotificationCenter.Current;
            notificationCenter.Show(notification);
        }

     
        
    }
}
