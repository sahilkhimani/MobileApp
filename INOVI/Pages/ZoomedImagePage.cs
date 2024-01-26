using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INOVI.Pages
{
    public class ZoomedImagePage : ContentPage
    {
        public ZoomedImagePage(ImageSource imageSource)
        {

            var ZoomedImage = new Image
            {
                Source = imageSource,
                Aspect = Aspect.AspectFit,
                
                
                VerticalOptions = LayoutOptions.CenterAndExpand, 
            };
          

            var closeButton = new Button
            {
                Text = "✕",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(5),
            };

            closeButton.Clicked += OnCloseButtonClicked;

            var stackLayout = new StackLayout
            {
                Children = {closeButton, ZoomedImage }
            };


            Content = stackLayout;
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
