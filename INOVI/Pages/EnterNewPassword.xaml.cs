using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;
using INOVI.Pages;
using Newtonsoft.Json;
namespace INOVI;

public partial class EnterNewPassword : ContentPage
{
    CallApi api = new CallApi();
	public EnterNewPassword()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void NewPasswordClicked(object sender, EventArgs e)
    {
        UpdateUserPasswordDTO req = new UpdateUserPasswordDTO();
        CancellationTokenSource cts = new CancellationTokenSource();
        req.UserEmail = Preferences.Get("emailAddress", "");
        req.Otp = Preferences.Get("_OTP", "");
        req.UserPassword = txtPassword.Text;
        req.UserConfirmPassword = txtConfirmPassword.Text;
        try
        {
            if(string.IsNullOrWhiteSpace(req.UserPassword) || string.IsNullOrWhiteSpace(req.UserConfirmPassword))
            {
                startLoad();
                await Shell.Current.DisplayAlert("Error", "All fields required", "OK");
                stopLoad();
                return;
            }

            if (req.UserPassword.Length < 8)
            {
                await DisplayAlert("Error", "Your Password must be atleast 8 characters", "OK");
                return;
            }

            if(req.UserPassword != req.UserConfirmPassword)
            {
                var toast = Toast.Make("Password and Confirm Password is not same");
                await toast.Show(cts.Token);
                return;
            }

            startLoad();
            string data = JsonConvert.SerializeObject(req);
            string response = await api.consumeapi(data, "User/UpdateUserPassword");
            stopLoad();

            if(response.Contains("Password Updated Successfully!"))
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