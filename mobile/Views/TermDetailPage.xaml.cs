using mobile.Models;
using mobile.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace mobile.Views
{
    [QueryProperty(nameof(TermId), "termId")]
    public partial class TermDetailPage : ContentPage
    {
        private Term _currentTerm;
        public string TermId { get; set; }

        public TermDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Log to see if TermId is correctly set
            Console.WriteLine($"OnAppearing: TermId = {TermId}");

            await LoadTermDetails();
            await LoadCourses();
        }

        private async Task LoadTermDetails()
        {
            if (int.TryParse(TermId, out int termId))
            {
                _currentTerm = await App.DatabaseService.GetTermByIdAsync(termId);
                if (_currentTerm != null)
                {
                    Title = _currentTerm.Title;
                    TermTitleLabel.Text = _currentTerm.Title;
                    TermDatesLabel.Text = $"From: {_currentTerm.StartDate:MM/dd/yyyy} To: {_currentTerm.EndDate:MM/dd/yyyy}";
                }
                else
                {
                    await DisplayAlert("Error", "No term found.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No term selected.", "OK");
            }
        }

        private async Task LoadCourses()
        {
            if (int.TryParse(TermId, out int termId))
            {
    

                // Fetch courses for the given TermId
                List<Course> courses = await App.DatabaseService.GetAsync<Course>(c => c.TermID == termId);

                // Set the data source for the CollectionView
                CoursesCollectionView.ItemsSource = courses;
            }
            else
            {
                // Log if TermId is invalid
                Console.WriteLine("Invalid TermId. Could not load courses.");
            }
        }



        private async void OnCourseSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Course selectedCourse)
            {
                await Shell.Current.GoToAsync($"CourseViewPage?courseId={selectedCourse.Id}");
                ((CollectionView)sender).SelectedItem = null;
            }
        }


        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnAddCourseButtonClicked(object sender, EventArgs e)
        {
            if (int.TryParse(TermId, out int termId))
            {
                // Passing termId as a query parameter in the URL
                await Shell.Current.GoToAsync($"AddCoursePage?termId={termId}");
            }
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }



    }
}
