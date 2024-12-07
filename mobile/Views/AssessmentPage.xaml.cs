using Microsoft.Maui.Controls;

namespace mobile.Views
{
    [QueryProperty(nameof(CourseId), "courseId")]
    public partial class AssessmentPage : ContentPage
    {
        private int _courseId;

        public int CourseId
        {
            get => _courseId;
            set => _courseId = value;
        }

       
        public AssessmentPage()
        {
            InitializeComponent();
        }

    
        private async void OnPerformanceAssessmentClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"AssessmentDetailPage?courseId={_courseId}&type=Performance");
        }

        private async void OnObjectiveAssessmentClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"AssessmentDetailPage?courseId={_courseId}&type=Objective");
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}