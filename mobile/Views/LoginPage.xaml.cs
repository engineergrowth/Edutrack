using mobile.Models;
using mobile.Services;

namespace mobile.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public LoginPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService(); 
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Hide the error label initially
            ErrorLabel.IsVisible = false;

            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            // Await the authentication method
            var user = await _databaseService.AuthenticateUserAsync(username, password);

            if (user != null)
            {
                // Navigate to the TermListPage
                await Shell.Current.GoToAsync("//TermListPage"); 
            }
            else
            {
                // Display an error message
                ErrorLabel.Text = "Invalid username or password.";
                ErrorLabel.IsVisible = true;
            }
        }

    }
}
