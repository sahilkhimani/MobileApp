using Helper;
using INOVI.Pages;

namespace INOVI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
  
            MainPage = new AppShell();

        }
    }
}