using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U3ActRegistroDeActividadesMaui.Services;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        LoginService LoginService = new(IPlatformApplication.Current.Services.GetService<HttpClient>() ?? new HttpClient());
        [ObservableProperty]
        string error;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;
        [RelayCommand]
        void IniciarSesion()
        {
            var cons = LoginService.Login(Username, Password);
            if(!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                if (cons)
                {
                    App.Current.MainPage = new AppShell();
                    Username = "";
                    Password = "";
                    Error = "";
                }
                else
                {
                    Error = "Usuario o contraseña incorrectos";
                }
            }
            else
            {
                Error = "Por favor llene los campos";
            }
           
        }
    }
}
