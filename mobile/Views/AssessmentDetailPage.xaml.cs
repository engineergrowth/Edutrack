using Plugin.LocalNotification;
using mobile.Models;
using mobile.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace mobile.Views
{
    [QueryProperty(nameof(CourseId), "courseId")]
    [QueryProperty(nameof(AssessmentType), "type")]
    public partial class AssessmentDetailPage : ContentPage
    {
        private int _courseId;
        private string _assessmentType;
        private Assessment _currentAssessment;
        private readonly DatabaseService _databaseService;

        public int CourseId { get; set; }
        public string AssessmentType { get; set; }

        public AssessmentDetailPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAssessmentDetails();
        }

        private async Task LoadAssessmentDetails()
        {
            _courseId = CourseId;
            _assessmentType = AssessmentType;

            if (_courseId > 0 && !string.IsNullOrWhiteSpace(_assessmentType))
            {
                // Use the correct method name 'GetAssessmentsByTypeAsync' and await the result
                var assessments = await _databaseService.GetAssessmentsByTypeAsync(_courseId, _assessmentType);

                // If assessments are returned, pick the first one (or handle multiple if needed)
                _currentAssessment = assessments.FirstOrDefault();

                if (_currentAssessment != null)
                {
                    AssessmentTitleEntry.Text = _currentAssessment.Title;
                    StartDatePicker.Date = _currentAssessment.StartDate;
                    EndDatePicker.Date = _currentAssessment.EndDate;
                    NotificationCheckBox.IsChecked = _currentAssessment.NotificationEnabled;
                }
                else
                {
                    // If no assessment is found, create a new one
                    _currentAssessment = new Assessment
                    {
                        CourseID = _courseId,
                        Type = _assessmentType
                    };
                }
            }
        }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _currentAssessment.Title = AssessmentTitleEntry.Text;
            _currentAssessment.StartDate = StartDatePicker.Date;
            _currentAssessment.EndDate = EndDatePicker.Date;
            _currentAssessment.NotificationEnabled = NotificationCheckBox.IsChecked;
            _currentAssessment.Type = AssessmentType;

            await _databaseService.SaveAssessmentAsync(_currentAssessment);

            if (_currentAssessment.NotificationEnabled)
            {
                ScheduleAssessmentNotification(_currentAssessment);
            }

            await DisplayAlert("Success", "Assessment saved successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private void ScheduleAssessmentNotification(Assessment assessment)
        {
            var startNotificationTime = assessment.StartDate.AddDays(-1);
            var endNotificationTime = assessment.EndDate.AddDays(-1);

            if (startNotificationTime < DateTime.Now)
            {
                startNotificationTime = DateTime.Now.AddMinutes(1);
            }

            if (endNotificationTime < DateTime.Now)
            {
                endNotificationTime = DateTime.Now.AddMinutes(1);
            }

            // Schedule notification for assessment start
            var startNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = $"Upcoming Assessment: {assessment.Title}",
                Description = $"Your assessment '{assessment.Title}' starts tomorrow!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = startNotificationTime
                }
            };

            // Schedule notification for assessment end
            var endNotification = new NotificationRequest
            {
                NotificationId = new Random().Next(1000, 9999),
                Title = $"Assessment Ending: {assessment.Title}",
                Description = $"Your assessment '{assessment.Title}' ends tomorrow!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = endNotificationTime
                }
            };

            var notificationCenter = LocalNotificationCenter.Current;
            notificationCenter.Show(startNotification);
            notificationCenter.Show(endNotification);
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (_currentAssessment != null)
            {
                await _databaseService.DeleteAssessmentAsync(_currentAssessment);
                await DisplayAlert("Success", "Assessment deleted successfully.", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

