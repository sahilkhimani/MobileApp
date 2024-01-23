using Helper;
using INOVI.DTO;
using Newtonsoft.Json;
using CommunityToolkit.Maui.Alerts;

namespace INOVI;

public partial class ForgotPassword : ContentPage
{
    CallApi api = new CallApi();
    public ForgotPassword()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void sendCode(object sender, EventArgs e)
    {
        SendOTPDTO req = new SendOTPDTO();
        CancellationTokenSource cts = new CancellationTokenSource();

        req.UserEmail = txtEmail.Text;
        Preferences.Set("emailAddress", req.UserEmail);
        try
        {
            if (string.IsNullOrWhiteSpace(req.UserEmail))
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
            if (checkBox1.IsChecked == false)
            {
                await DisplayAlert("Error", "Please Click on I am not a Robot", "OK");
                return;
            }
            startLoad();
            string data = JsonConvert.SerializeObject(req);
            string response = await api.consumeapi(data, "user/sendotp");
            stopLoad();


            if (response.Contains("OTP Send Successfully! "))
            {
                Preferences.Set("_OTP", response.Split("'")[1]);
                await Navigation.PushAsync(new EmailVerification());
                var toast = Toast.Make("OTP Send Successfully");
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
            stopLoad();
            var toast = Toast.Make("Please connect to Internet");
            await toast.Show(cts.Token);
        }
    }

    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
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