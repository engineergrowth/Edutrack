using System;
using mobile.Models;
using mobile.Services;

namespace mobile.Views
{
    public partial class AddTermPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public AddTermPage()
        {
            InitializeComponent();
            _databaseService = App.DatabaseService;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            {
                await DisplayAlert("Validation Error", "Term title cannot be empty.", "OK");
                return;
            }

            if (EndDatePicker.Date <= StartDatePicker.Date)
            {
                await DisplayAlert("Validation Error", "End date must be after start date.", "OK");
                return;
            }

            // Create a new term
            var newTerm = new Term
            {
                Title = TitleEntry.Text.Trim(),
                StartDate = StartDatePicker.Date,
                EndDate = EndDatePicker.Date
            };

            // Save to database
            try
            {
                await _databaseService.AddAsync(newTerm);

                await DisplayAlert("Success", "Term added successfully.", "OK");
                await Navigation.PopAsync(); 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save term: {ex.Message}", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
