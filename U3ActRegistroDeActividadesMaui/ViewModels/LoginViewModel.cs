using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using U3ActRegistroDeActividadesMaui.Services;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly LoginService LoginService = new(
                                                     
            new HttpClient()                                                    
            {                                                         
                BaseAddress = new Uri("https://u3eqpo1actapi.labsystec.net/api")                                                     
            });

        [ObservableProperty]
        string error = "";
        [ObservableProperty]
        string username = null!;
        [ObservableProperty]
        string password = null!;

        [RelayCommand]
        void IniciarSesion()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {

            }
            else
            {
                var cons = LoginService.Login(Username, Password);
                if (cons)
                {
                    if (App.Current != null)
                    {
                        App.Current.MainPage = new AppShell();
                    }
                }
                else
                {
                    Error = "Usuario o contra incorrectos";
                }
            }
        }
    }
}
