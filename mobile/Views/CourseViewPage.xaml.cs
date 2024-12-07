using mobile.Models;
using mobile.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace mobile.Views
{
    [QueryProperty(nameof(CourseId), "courseId")]
    public partial class CourseViewPage : ContentPage
    {
        private Course _currentCourse;
        private DatabaseService _databaseService;

        public int CourseId { get; set; }

        public string Status { get; set; }

        public bool EnableNotifications { get; set; }

        public CourseViewPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCourseDetails();

            StatusPicker.ItemsSource = new List<string> { "Not Started", "In Progress", "Completed" };


            if (_currentCourse != null && !string.IsNullOrWhiteSpace(_currentCourse.Status))
            {
                StatusPicker.SelectedItem = _currentCourse.Status.Trim();
            }

           
            EnableNotificationsCheckBox.IsChecked = _currentCourse?.EnableNotifications ?? false;
        }



        private async Task LoadCourseDetails()
        {
            if (CourseId > 0)
            {
                _currentCourse = await _databaseService.GetCourseByIdAsync(CourseId);
                if (_currentCourse != null)
                {
                    TitleEntry.Text = _currentCourse.Title;
                    StartDatePicker.Date = _currentCourse.StartDate;
                    EndDatePicker.Date = _currentCourse.EndDate;

                  
                    Status = _currentCourse.Status?.Trim();
                    EnableNotifications = _currentCourse.EnableNotifications;

                    InstructorNameEntry.Text = _currentCourse.InstructorName;
                    InstructorEmailEntry.Text = _currentCourse.InstructorEmail;
                    InstructorPhoneEntry.Text = _currentCourse.InstructorPhone;
                    NotesEditor.Text = _currentCourse.Notes;

        
                    StatusPicker.ItemsSource = new List<string> { "Not Started", "In Progress", "Completed" };
                    StatusPicker.SelectedItem = Status;

             
                    EnableNotificationsCheckBox.IsChecked = EnableNotifications;

                    StartDatePicker.MinimumDate = DateTime.Now;
                    EndDatePicker.MinimumDate = DateTime.Now;
                }
                else
                {
                    await DisplayAlert("Error", "Course not found.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid Course ID.", "OK");
            }
        }





        // Save edited course details
        private async void OnSaveCourseClicked(object sender, EventArgs e)
        {
            // Form validation: Check for null or empty fields
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Validation Error", "Course title is required.", "OK");
                return;
            }

            if (StatusPicker.SelectedIndex == -1 || string.IsNullOrWhiteSpace(Status))
            {
                await DisplayAlert("Validation Error", "Course status is required. Please select a status.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstructorNameEntry.Text))
            {
                await DisplayAlert("Validation Error", "Instructor name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstructorEmailEntry.Text) || !InstructorEmailEntry.Text.Contains("@"))
            {
                await DisplayAlert("Validation Error", "A valid instructor email is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstructorPhoneEntry.Text))
            {
                await DisplayAlert("Validation Error", "Instructor phone number is required.", "OK");
                return;
            }

            // Ensure that the end date is after or equal to the start date
            if (EndDatePicker.Date < StartDatePicker.Date)
            {
                await DisplayAlert("Validation Error", "End date must be on or after the start date.", "OK");
                return;
            }

            // If validation passes, update the course details
            if (_currentCourse != null)
            {
                _currentCourse.Title = TitleEntry.Text.Trim();
                _currentCourse.StartDate = StartDatePicker.Date;
                _currentCourse.EndDate = EndDatePicker.Date;
                _currentCourse.Status = Status; 
                _currentCourse.InstructorName = InstructorNameEntry.Text.Trim();
                _currentCourse.InstructorEmail = InstructorEmailEntry.Text.Trim();
                _currentCourse.InstructorPhone = InstructorPhoneEntry.Text.Trim();
                _currentCourse.Notes = NotesEditor.Text?.Trim();
                _currentCourse.EnableNotifications = EnableNotificationsCheckBox.IsChecked; 

                await _databaseService.UpdateCourseAsync(_currentCourse);
                await DisplayAlert("Success", "Course updated successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Course not found.", "OK");
            }
        }


        // Share course notes
        private async void OnShareNotesClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_currentCourse?.Notes))
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = _currentCourse.Notes,
                    Title = "Share Course Notes"
                });
            }
            else
            {
                await DisplayAlert("Info", "No notes available to share.", "OK");
            }
        }

        // Delete the course
        private async void OnDeleteCourseClicked(object sender, EventArgs e)
        {
            if (_currentCourse != null)
            {
                bool confirm = await DisplayAlert("Delete Course", "Are you sure you want to delete this course?", "Yes", "No");
                if (confirm)
                {
                    await _databaseService.DeleteAsync(_currentCourse);
                    await DisplayAlert("Success", "Course deleted successfully!", "OK");
                    await Navigation.PopAsync();
                }
            }
        }

        // Navigate to the assessment page
        private async void OnAssessmentsClicked(object sender, EventArgs e)
        {
            if (_currentCourse != null)
            {
                await Shell.Current.GoToAsync($"AssessmentPage?courseId={_currentCourse.Id}");
            }
        }

        // Handle cancel button
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}