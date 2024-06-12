using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        [RelayCommand]
        async Task CerrarSesion()
        {
            var answer = await Shell.Current.DisplayAlert("Cerrar Sesión", "¿Estás seguro de que quieres cerrar sesión?", "Sí", "No");
            if (answer)
            {
                SecureStorage.RemoveAll();
                Preferences.Clear();
                App.ActividadesRepository.DeleteAll();
                App.DepartamentosRepository.DeleteAll();
                App.Current.MainPage = new LoginView();
            }
        }
    }
}
