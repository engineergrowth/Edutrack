using mobile.Models;
using mobile.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace mobile.Views
{
    public partial class SearchPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public SearchPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string query = SearchEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                await DisplayAlert("Error", "Please enter an instructor name.", "OK");
                return;
            }

            // Perform the search
            var results = await _databaseService.SearchCoursesByInstructorAsync(query);

            if (results.Any())
            {
                SearchResultsCollectionView.ItemsSource = results;
                SearchResultsCollectionView.IsVisible = true;
                NoResultsLabel.IsVisible = false;
            }
            else
            {
                SearchResultsCollectionView.ItemsSource = null;
                SearchResultsCollectionView.IsVisible = false;
                NoResultsLabel.IsVisible = true;
            }
        }


        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); 
        }



    }
}
