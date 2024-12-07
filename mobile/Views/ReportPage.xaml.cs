using mobile.Models;
using mobile.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mobile.Views
{
    public partial class ReportPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public ReportPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        // Event handler for the "Show Courses" button click
        private async void OnShowCoursesButtonClicked(object sender, EventArgs e)
        {
            // Get selected status from the picker
            var selectedStatus = StatusPicker.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedStatus))
            {
                await DisplayAlert("Error", "Please select a status.", "OK");
                return;
            }

            // Fetch courses with the selected status from the database
            var courses = await _databaseService.GetCoursesByStatusAsync(selectedStatus);

            // Clear any previous courses in the stack layout
            CoursesStackLayout.Children.Clear();

            // If courses were found, display them
            if (courses.Any())
            {
                // Set the timestamp for the report
                TimestampLabel.Text = $"Report Generated: {DateTime.Now:MM/dd/yyyy HH:mm}";
                TimestampLabel.IsVisible = true;

                // Loop through the courses and add them to the StackLayout as cards
                foreach (var course in courses)
                {
                    var courseCard = new Frame
                    {
                        Padding = 15,
                        Margin = new Thickness(0, 10),                 
                        CornerRadius = 10,
                        Content = new StackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label { Text = course.Title, FontSize = 18, FontAttributes = FontAttributes.Bold },
                                new Label { Text = $"Instructor: {course.InstructorName}" },
                                new Label { Text = $"Start Date: {course.StartDate:MM/dd/yyyy}" },
                                new Label { Text = $"End Date: {course.EndDate:MM/dd/yyyy}" }
                            }
                        }
                    };

                    // Add the course card to the StackLayout
                    CoursesStackLayout.Children.Add(courseCard);
                }
            }
            else
            {
                // If no courses found for the selected status, show a message
                var noCoursesMessage = new Label
                {
                    Text = "No courses found for the selected status.",
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                CoursesStackLayout.Children.Add(noCoursesMessage);
            }
        }
    }
}
