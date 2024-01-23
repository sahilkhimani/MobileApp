using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;
using Microsoft.Maui.HotReload;
using Newtonsoft.Json;
using System.ComponentModel;

namespace INOVI
{
    public partial class AppShell : Shell
    {
        CallApi api = new CallApi();
        public AppShell()
        {
            InitializeComponent();
            //InitializeDrawer();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }
        //private void InitializeDrawer()
        //{
        //    userNameLabel.Text = Preferences.Get("name", "");
        //    userRoleLabel.Text = Preferences.Get("role", "");
        //}

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess.HasFlag(NetworkAccess.Internet))
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                var toast = Toast.Make("Connected to Internet");
                toast.Show(cts.Token);

            }
            else
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                var toast = Toast.Make("Disconnected from Internet");
                toast.Show(cts.Token);
            }
        }

        private async void LogOutClicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            await Navigation.PushAsync(new LoginPage());
        }

        //private async void OnProfileClicked(object sender, TappedEventArgs e)
        //{
        //    PickOptions options = new PickOptions();
        //    try
        //    {
        //        var result = await FilePicker.Default.PickAsync(options);
        //        if (result != null)
        //        {
        //            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
        //               result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
        //               result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
        //               )
        //            {
        //                var stream = await result.OpenReadAsync();
        //                var image = ImageSource.FromStream(() => stream);
        //                Img.Source = image;
        //            }
        //            else
        //            {
        //                await Shell.Current.DisplayAlert("Error!", "Invalid Picture type", "Ok");
        //            }
        //        }
        //    }
        //    catch { }
        //}


    }

}