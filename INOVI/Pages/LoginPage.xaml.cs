using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using INOVI.Pages;

namespace INOVI;
public partial class LoginPage : ContentPage
{
    CallApi api = new CallApi();

    public LoginPage()
    {
        InitializeComponent();
        Preferences.Clear();
        NavigationPage.SetHasNavigationBar(this, true);
    }

    [Obsolete]
    private async void Login_Clicked(object sender, EventArgs e)
    {
        LoginDTO req = new LoginDTO();
        CancellationTokenSource cts = new CancellationTokenSource();
        try
        {
            req.UserEmail = emailTxt.Text;
            req.UserPassword = passwordTxt.Text;

            if (string.IsNullOrWhiteSpace(req.UserEmail) || string.IsNullOrWhiteSpace(req.UserPassword))
            {
                startLoad();
                await Shell.Current.DisplayAlert("Error", "All fields required", "Ok");
                stopLoad();
                return;
            }
            if (!req.UserEmail.Contains("@"))
            {
                await DisplayAlert("Error", "Invalid Email Address", "OK");
                return;
            }

            startLoad();
            string data = JsonConvert.SerializeObject(req);
            string response = await api.consumeapi(data, "user/loginuser");
            stopLoad();

            if (response.Contains("User Not Authenticate!") || response.Contains("Error Login Data!"))
            {
                startLoad();
                var toast = Toast.Make(response);
                await toast.Show(cts.Token);
                errorLabel.Text = "Email or Password is Incorrect";
                stopLoad();
                return;
            }
            else
            {
                startLoad();
                Preferences.Set("jwtKey", response);
                await Navigation.PushAsync(new HomePage());
                //await Shell.Current.GoToAsync("//HomePage");
                var toast = Toast.Make("Login Successfull");
                await toast.Show(cts.Token);
                stopLoad();
            }
        }
        catch (Exception ex)
        {
            stopLoad();
            await DisplayAlert("", ex.Message, "OK");
            var toast = Toast.Make("Please connect to Internet");
            await toast.Show(cts.Token);
            return;
        }
    }

    private async void OnSignupTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Signup());
    }

    private async void OnForgotPasswordTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ForgotPassword());
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

