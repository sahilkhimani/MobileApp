using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Net.Security;

namespace INOVI;

public partial class EmailVerification : ContentPage
{
    CallApi api = new CallApi();
    CancellationTokenSource cts = new CancellationTokenSource();
    String Otp;
    string UserEmail = Preferences.Get("emailAddress", "");
    public EmailVerification()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        firstDigit.TextChanged += (sender, e) =>
        {
            secondDigit.Focus();
        };
        secondDigit.TextChanged += (sender, e) =>
        {
            thirdDigit.Focus();
        };
        thirdDigit.TextChanged += (sender, e) =>
        {
            fourthDigit.Focus();
        };

    }
    private async void VerifyClicked(object sender, EventArgs e)
    {
        Otp = (firstDigit.Text + secondDigit.Text + thirdDigit.Text + fourthDigit.Text).ToString();
        try
        {
            if (string.IsNullOrWhiteSpace(Otp))
            {
                startLoad();
                await Shell.Current.DisplayAlert("Error", "Please Enter the OTP to continue", "Ok");
                stopLoad();
                return;
            }

            if (Preferences.Get("_OTP", "") != Otp)
            {
                startLoad();
                var toast = Toast.Make("OTP is not valid");
                await toast.Show(cts.Token);
                stopLoad();
                return;
            }

            startLoad();
            await Navigation.PushAsync(new EnterNewPassword());
            var toast1 = Toast.Make("OTP Matched");
            await toast1.Show(cts.Token);
            stopLoad();
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

    private async void resendCode(object sender, TappedEventArgs e)
    {
        firstDigit.Text = string.Empty;
        secondDigit.Text = string.Empty;
        thirdDigit.Text = string.Empty;
        fourthDigit.Text = string.Empty;
        firstDigit.Focus();
        SendOTPDTO req = new SendOTPDTO();
        req.UserEmail = UserEmail;
        try
        {
            if (req.UserEmail == null)
            {
                startLoad();
                await Shell.Current.DisplayAlert("Error", "Error", "Ok");
                stopLoad();
                return;
            }
            startLoad();
            string data = JsonConvert.SerializeObject(req);
            string response = await api.consumeapi(data, "user/sendotp");
            stopLoad();
            
            if (response.Contains("OTP Send Successfully! "))
            {
                Preferences.Set("_OTP", response.Split("'")[1]);
                var toast = Toast.Make("OTP Send Succesfully");
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

            throw;
        }
        return;
    }
}