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
            //TODO: Validar
            var cons = LoginService.Login(Username, Password);
            if (cons)
            {
                App.Current.MainPage = new AppShell();
            }
            else
            {
                Error = "Usuario o contra incorrectos";
            }
        }
    }
}
