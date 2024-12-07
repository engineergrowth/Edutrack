using System;
using System.Linq;
using mobile.Models;
using mobile.Services;

namespace mobile.Views;

public partial class TermListPage : ContentPage
{
    public TermListPage()
    {
        InitializeComponent();

        // Load terms from the database when the page is initialized
        LoadTerms();
    }

    // Method to load terms from the database
    private async void LoadTerms()
    {
        try
        {
            // Fetch all terms from the database
            List<Term> terms = await App.DatabaseService.GetAsync<Term>(t => true);


            // Set the ItemsSource for the CollectionView
            TermCollectionView.ItemsSource = terms;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading terms: {ex.Message}");
            await DisplayAlert("Error", "Failed to load terms from the database.", "OK");
        }
    }

    private async void OnTermSelected(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.Count > 0 && e.CurrentSelection.FirstOrDefault() is Term selectedTerm)
            {
                ;
                await Shell.Current.GoToAsync($"TermDetailPage?termId={selectedTerm.Id}");
            }
            ((CollectionView)sender).SelectedItem = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnTermSelected: {ex.Message}");
            await DisplayAlert("Error", "Failed to navigate to term details.", "OK");
        }
    }



    private async void OnAddTermClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(AddTermPage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnAddTermClicked: {ex.Message}");
            await DisplayAlert("Error", "Failed to navigate to Add Term page.", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        List<Term> terms = await App.DatabaseService.GetAsync<Term>(t => true);

        TermCollectionView.ItemsSource = terms;
    }

    private async void OnEditTermClicked(object sender, EventArgs e)
    {
        try
        {
            if (((Button)sender).BindingContext is Term selectedTerm)
            {
                await Navigation.PushAsync(new EditTermPage(selectedTerm));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnEditTermClicked: {ex.Message}");
            await DisplayAlert("Error", "Failed to navigate to Edit Term page.", "OK");
        }
    }

    private async void OnGenerateReportClicked(object sender, EventArgs e)
    {
        // Navigate to the ReportPage
        await Navigation.PushAsync(new ReportPage());
    }
}
