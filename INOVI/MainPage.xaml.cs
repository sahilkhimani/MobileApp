using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace INOVI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnSignup(object sender, EventArgs e)
        {
            startLoad();
            await Navigation.PushAsync(new Signup());
            stopLoad();
        }
        private async void OnLoginPage(object sender, EventArgs e)
        {
            startLoad();
            await Navigation.PushAsync(new LoginPage());
            stopLoad();
        }
        public void startLoad()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsEnabled = true;
            activityIndicator.IsVisible = true;
        }

        public void stopLoad()
        {
            activityIndicator.IsRunning = false;
            activityIndicator.IsEnabled = false;
            activityIndicator.IsVisible = false;
        }
    }
}