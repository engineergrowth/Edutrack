using mobile.Services;
using mobile.Views;

namespace mobile;

public partial class App : Application
{
    public static DatabaseService DatabaseService { get; private set; }

    public App()
    {
        InitializeComponent();

        // Initialize the DatabaseService
        DatabaseService = new DatabaseService();

        // Run database tests and initialize sample data
        InitializeDatabase();

        // Set the starting page to AppShell
        MainPage = new AppShell(); // Use AppShell as the main container for navigation
    }

    private async void InitializeDatabase()
    {
        try
        {
            // Initialize sample data
            await DatabaseService.InitializeSampleData();
            Console.WriteLine("Database initialized successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
        }
    }
}
