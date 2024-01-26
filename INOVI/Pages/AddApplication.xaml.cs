using CommunityToolkit.Maui.Alerts;
using Helper;
using INOVI.DTO;

using Newtonsoft.Json;
using SkiaSharp;
using ImageSource = Microsoft.Maui.Controls.ImageSource;

namespace INOVI.Pages;

public partial class AddApplication : ContentPage
{
    AddQueryDTO req = new AddQueryDTO();
    CallApi api = new CallApi();
    CancellationTokenSource cts = new CancellationTokenSource();
    List<string> lst = new List<string>();

    public AddApplication()
    {
        InitializeComponent();
    }

    private bool attachmentClickEnabled = true;
    private async void OnAddAttachmentClicked(object sender, EventArgs e)
    {
        startLoad();
        if (attachmentClickEnabled)
        {
            var results = await FilePicker.PickMultipleAsync();
            if (results == null || results.Count() == 0)
            {
                stopLoad();
                await Shell.Current.DisplayAlert("", "Please Select File", "Ok");
                return;
            }
            else
            {
                addPicture(results);
                attachmentClickEnabled = false;

                bool fileSelected = true;

                Img.IsVisible = !fileSelected;
                UploadLabel.IsVisible = !fileSelected;
                SelectFileButton.IsVisible = fileSelected;
            }

        }

        stopLoad();
    }

    private async void SelectFileButton_Clicked(object sender, EventArgs e)
    {
        startLoad();
        var results = await FilePicker.PickMultipleAsync();

        if (results == null || results.Count() == 0) 
        {
            stopLoad();
            await Shell.Current.DisplayAlert("", "Please Select File", "Ok");
            return;
        }
        else
        {
            addPicture(results);
        }
    }

    [Obsolete]
    private async void btnCancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HomePage());
    }

    private async void addPicture(dynamic results)
    {
        foreach (var result in results)
        {
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)
                )
                {
                    var stream = await result.OpenReadAsync();

                    using (var originalBitmap = SKBitmap.Decode(stream))
                    {
                        int newWidth = originalBitmap.Width / 2;
                        int newHeight = originalBitmap.Height / 2;

                        using (var resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium))
                        {
                            byte[] byteArray;
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                resizedBitmap.Encode(SKEncodedImageFormat.Jpeg, 80).SaveTo(memoryStream);
                                byteArray = memoryStream.ToArray();
                            }
                            lst.Add(Convert.ToBase64String(byteArray));
                        }
                    }

                    //byte[] byteArray;
                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    await stream.CopyToAsync(memoryStream);
                    //    byteArray = memoryStream.ToArray();

                    //}
                    //lst.Add(Convert.ToBase64String(byteArray));

                }
                else
                {
                    stopLoad();
                    await Shell.Current.DisplayAlert("Error!", "Invalid File Type", "Ok");
                }
            }

        }

        ImageContainer.Children.Clear();
        foreach (string base64String in lst)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            var image = new Image
            {
                Source = imageSource,
                WidthRequest = 50,
                HeightRequest = 50,
                Margin = 5
            };
            stopLoad();
            ImageContainer.Children.Add(image);
        }
    }   

    [Obsolete]
    private async void btnSendButton_Clicked(object sender, EventArgs e)
    {
        startLoad();
        try
        {
            req.Title = titleEntry.Text;
            req.Description = descriptionEditor.Text;
            req.Attachments = lst;
            req.UserID = Convert.ToInt32(Preferences.Get("userId", ""));

            if (string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.Description))
            {
                startLoad();
                await Shell.Current.DisplayAlert("Error", "All fields required", "Ok");
                stopLoad();
                return;
            }

            if (req == null)
            {
                await DisplayAlert("Error", "Error", "Error");
            }

            string data = JsonConvert.SerializeObject(req);
            string response = await api.consumeapi(data, "query/addquery");
            stopLoad();
            if (response.Contains("Query Added Successfully!"))
            {
                var toast = Toast.Make(response);
                await toast.Show(cts.Token);
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                var toast = Toast.Make(response);
                await toast.Show(cts.Token);
            }

        }
        catch (Exception)
        {

            throw;
        }

    }
    private async void HandleTextChanged(object sender, TextChangedEventArgs e)
    {
        // Count words
        int wordCount = e.NewTextValue.Split(new char[] { ' ', '.', ',', ';', ':', '-', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        wordCountLabel.Text = $"Word Count: {wordCount} / 1000";

        if (wordCount >= 1000)
        {
            descriptionEditor.IsReadOnly = true;
            var toast = Toast.Make("You have reached the limit");
            await toast.Show(cts.Token);
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


//var tgr = new TapGestureRecognizer();
//tgr.Tapped += (s, e) =>
// {
//     zoomImage(imageSource);
// };
//image.GestureRecognizers.Add(tgr);

//private void zoomImage(dynamic source)
//{
//    var newImage = new Image
//    {
//        Source = source,
//        WidthRequest = 100,
//        HeightRequest = 100,
//        Margin = 5
//    };

//}