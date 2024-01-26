using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;
using Newtonsoft.Json;
using System.Net;

namespace INOVI.Pages;

public partial class MyRequestsPage : ContentPage
{
    CancellationTokenSource cts = new CancellationTokenSource();
    CallApi api = new CallApi();
    UpdateStatusDTO status = new UpdateStatusDTO();
    List<string> lst = new List<string>();
    public MyRequestsPage(GetQueryDTO req)
    {
        InitializeComponent();
        titleEntry.IsReadOnly = true;
        descriptionLabel.IsReadOnly = true;
        remarksEntry.IsReadOnly = true;
        titleEntry.Text = req.Title;
        descriptionLabel.Text = req.Description;
        remarksEntry.Text = req.Remarks;
        Image zoomedImage;

        foreach (byte[] arr in req.Attachmentbytes)
        {
            ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(arr));
            zoomedImage = new Image
            {
                Source = imageSource,
                WidthRequest = 50,
                HeightRequest = 50,
                Margin = 5
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnImageTapped;
            zoomedImage.GestureRecognizers.Add(tapGestureRecognizer);

            ImageContainer.Children.Add(zoomedImage);
        }

         void OnImageTapped(object sender, EventArgs e)
        {
            Image tappedImage = (Image)sender;
            Navigation.PushModalAsync(new ZoomedImagePage(tappedImage.Source));
        }

        if (Preferences.Get("roleId","") == "4" || Preferences.Get("roleId", "") == "1")
        {
            if((req.CurrentStatus).ToLower() == "pending".ToLower())
            {
                remarksEntry.IsReadOnly = false;
                approverBtns.IsVisible = true;
            }
        }
        Preferences.Set("queryId", (req.QueryId).ToString());

    }

    

    //private async void HandleTextChnaged(object sender, TextChangedEventArgs e)
    //{
    //    int wordCount = e.NewTextValue.Split(new char[] { ' ', '.', ',', ';', ':', '-', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

    //    //wordCountLabel.Text = $"Word Count: {wordCount} / 1000";

    //    if (wordCount >= 1000)
    //    {
    //        descriptionLabel.IsReadOnly = true;
    //        var toast = Toast.Make("You have reached the limit");
    //        await toast.Show(cts.Token);
    //    }
    //}


    [Obsolete]
    private void rejectClicked(object sender, EventArgs e)
    {

        status.QueryID = Convert.ToInt32(Preferences.Get("queryId", ""));
        status.UserID = Convert.ToInt32(Preferences.Get("userId", ""));
        status.Remarks = remarksEntry.Text;
        status.StatusID = (int)EStatus.Rejected;
        changeStatus(status);

    }

    [Obsolete]
    private async void changeStatus(UpdateStatusDTO status)
    {
        if(status.Remarks == null)
        {
            status.Remarks = "";
        }
        string data = JsonConvert.SerializeObject(status);
        string response = await api.consumeapi(data, "query/updatequery");

        if (response.Contains("Query Updated Successfully!"))
        {
            await Navigation.PushAsync(new HomePage());
            var toast = Toast.Make("Status Updated!");
            await toast.Show(cts.Token);
        }
        else
        {
            var toast = Toast.Make(response);
            await toast.Show(cts.Token);
        }
    }

    [Obsolete]
    private void approveClicked(object sender, EventArgs e)
    {

        status.QueryID = Convert.ToInt32(Preferences.Get("queryId", ""));
        status.UserID = Convert.ToInt32(Preferences.Get("userId", ""));
        status.Remarks = remarksEntry.Text;
        status.StatusID = (int)EStatus.Approved;
        changeStatus(status);
    }
}