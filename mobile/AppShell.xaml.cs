using mobile.Views;

namespace mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register the routes for navigation
            Routing.RegisterRoute(nameof(TermDetailPage), typeof(TermDetailPage));
            Routing.RegisterRoute(nameof(AddTermPage), typeof(AddTermPage));
            Routing.RegisterRoute(nameof(AddCoursePage), typeof(AddCoursePage));
            Routing.RegisterRoute(nameof(CourseViewPage), typeof(CourseViewPage));
            Routing.RegisterRoute(nameof(AssessmentPage), typeof(AssessmentPage));
            Routing.RegisterRoute(nameof(AssessmentDetailPage), typeof(AssessmentDetailPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ReportPage), typeof(ReportPage));
        }
    }
}
