using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var tkn = SecureStorage.GetAsync("tkn").Result;
            if (tkn != null)
            {

                MainPage = new AppShell();
            }
            else
            {
                MainPage = new LoginView();
            }
        }
    }
}
