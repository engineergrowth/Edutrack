using System;
using mobile.Models;
using mobile.Services;

namespace mobile.Views
{
    public partial class EditTermPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        private Term _currentTerm;

        public EditTermPage(Term term)
        {
            InitializeComponent();
            _databaseService = App.DatabaseService;
            _currentTerm = term;

            TitleEntry.Text = _currentTerm.Title;
            StartDatePicker.Date = _currentTerm.StartDate;
            EndDatePicker.Date = _currentTerm.EndDate;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
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

            _currentTerm.Title = TitleEntry.Text.Trim();
            _currentTerm.StartDate = StartDatePicker.Date;
            _currentTerm.EndDate = EndDatePicker.Date;

            try
            {
                await _databaseService.UpdateAsync(_currentTerm);
                await DisplayAlert("Success", "Term updated successfully.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update term: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteTermClicked(object sender, EventArgs e)
        {
            try
            {
                // Get the term object from the database using the Id
                var termToDelete = await _databaseService.GetTermByIdAsync(_currentTerm.Id);

                if (termToDelete != null)
                {
                    // Delete the term
                    var result = await _databaseService.DeleteAsync(termToDelete);

                    if (result > 0) // Check if deletion was successful
                    {
                        await DisplayAlert("Success", "Term deleted successfully.", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to delete term. Please try again.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Term not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete term: {ex.Message}", "OK");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
