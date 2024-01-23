using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;
using Newtonsoft.Json;

namespace INOVI;

public partial class Signup : ContentPage
{
    CallApi api = new CallApi();

    public Signup()
    {
        InitializeComponent();
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void btnSignup_Clicked(object sender, EventArgs e)
    {
        SignupDTO req = new SignupDTO();
        CancellationTokenSource cts = new CancellationTokenSource();

        try
        {
            req.Name = nameTxt.Text;
            req.Username = usernameTxt.Text;
            req.UserEmail = emailTxt.Text;
            req.UserPassword = passwordTxt.Text;

            if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Username) ||
                string.IsNullOrWhiteSpace(req.UserEmail) || string.IsNullOrWhiteSpace(req.UserPassword))
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
            if (req.UserPassword.Length < 8)
            {
                await DisplayAlert("Error", "Your Password must be atleast 8 characters", "OK");
                return;
            }
            if (myCheckbox.IsChecked == false)
            {
                await DisplayAlert("Error", "Please accept all terms and conditions", "OK");
                return;
            }
            startLoad();
            string data = JsonConvert.SerializeObject(req);
            string response = await api.consumeapi(data, "user/signupuser");
            stopLoad();

            if (response.Contains("Signup Successfully!"))
            {
                await Navigation.PushAsync(new LoginPage());
                var toast = Toast.Make(response);
                await toast.Show(cts.Token);
            }
            else
            {
                startLoad();
                var toast = Toast.Make(response);
                await toast.Show(cts.Token);
                stopLoad();
                return;
            }
        }
        catch (Exception)
        {
            var toast = Toast.Make("Please Connect to Internet");
            await toast.Show(cts.Token);
            return;
        }
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