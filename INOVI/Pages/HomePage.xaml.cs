namespace INOVI.Pages;

using Helper;
using INOVI.DTO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Drawing;

public partial class HomePage : ContentPage
{
    CallApi api = new CallApi();

    [Obsolete]
    public HomePage()
    {
        InitializeComponent();

        NavigationPage.SetHasBackButton(this, false);
        GetUserInfo();
        GetApplicationsQuery();
        //AppShell _appShell = new AppShell();
        

        var icon1 = new FileImageSource { File = "add.svg" };
        var toolbarItem1 = new ToolbarItem { IconImageSource = icon1 };

        var icon2 = new FileImageSource { File = "reload.svg" };
        var toolbarItem2 = new ToolbarItem { IconImageSource = icon2 };

        if (Device.RuntimePlatform == Device.Android)
        {
            Application.Current.MainPage.ToolbarItems.Clear();
            Application.Current.MainPage.ToolbarItems.Add(toolbarItem1);
            Application.Current.MainPage.ToolbarItems.Add(toolbarItem2);
        }
        else if (Device.RuntimePlatform == Device.iOS)
        {
            NavigationPage.SetTitleView(this, new Label());
        }
        else if (Device.RuntimePlatform == Device.UWP)
        {
            Application.Current.MainPage.ToolbarItems.Clear();
            Application.Current.MainPage.ToolbarItems.Add(toolbarItem1);
            Application.Current.MainPage.ToolbarItems.Add(toolbarItem2);
        }

        toolbarItem1.Clicked += OnToolbarItem1Clicked;
        toolbarItem2.Clicked += OnToolbarItem2Clicked;
    }

    private async void GetApplicationsQuery()
    {
        string response = await api.consumeapi("", "query/getquerylist");
        var responseList = JsonConvert.DeserializeObject<List<GetQueryDTO>>(response);

        responseList = responseList.OrderByDescending(x => x.QueryId).ToList();

        for (int i = 0; i < responseList.Count; i++)
        {
            responseList[i].ShortDescription = LimitWords(responseList[i].Description, 15);
            if (responseList[i].CurrentStatus.ToLower() == ("Approved").ToLower())
            {
                responseList[i].ColorCode = "Green";
            }
            else if (responseList[i].CurrentStatus.ToLower() == ("Rejected").ToLower())
            {
                responseList[i].ColorCode = "Red";
            }
            else
            {
                responseList[i].ColorCode = "Yellow";
            }
        }
        if(responseList.Count > 0)
        {
            listview.ItemsSource = responseList;
        }

        else
        {
            noApplicationLabel.IsVisible = true;
        }
    }
    private async void OnToolbarItem1Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddApplication());
    }

    private void OnToolbarItem2Clicked(object sender, EventArgs e)
    {
        startLoad();
        GetApplicationsQuery();
        stopLoad();
    }

    private string LimitWords(string text, int wordLimit)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        string[] words = text.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(" ", words.Take(wordLimit));
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


    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null && e.SelectedItem is GetQueryDTO req)
        {
            Navigation.PushAsync(new MyRequestsPage(req));
            listview.SelectedItem = null;
        }

    }

    public async void GetUserInfo()
    {
        string response = await api.consumeapi("", "user/getuserinfo");
        var responseList = JsonConvert.DeserializeObject<GetUserDTO>(response);
        Preferences.Set("userId", (responseList.UserID).ToString());
        Preferences.Set("name", responseList.Name);
        Preferences.Set("email", responseList.Email);
        if (responseList.RoleID == 1)
        {
            Preferences.Set("roleId", "1");
            Preferences.Set("role", "Admin");
        }
        else if (responseList.RoleID == 2)
        {
            Preferences.Set("roleId", "2");
            Preferences.Set("role", "HR");
        }
        else if (responseList.RoleID == 4)
        {
            Preferences.Set("roleId", "4");
            Preferences.Set("role", "Approver");
        }
        else
        {
            Preferences.Set("roleId", "3");
            Preferences.Set("role", "Applicant");
        }
    }

}

