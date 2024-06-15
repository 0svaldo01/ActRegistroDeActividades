using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {

        //[RelayCommand]

        //[RelayCommand]
        //public void VerAgregarDepartamento()
        //{
        //    Departamento = new();
        //    Shell.Current.GoToAsync("//AgregarDep");
        //}
        [RelayCommand]
        static async Task CerrarSesion()
        {
            var answer = await Shell.Current.DisplayAlert("Cerrar Sesión", "¿Estás seguro de que quieres cerrar sesión?", "Sí", "No");
            if (answer)
            {
                SecureStorage.RemoveAll();
                Preferences.Clear();
                App.ActividadesRepository.DeleteAll();
                App.DepartamentosRepository.DeleteAll();
                if (App.Current != null)
                    App.Current.MainPage = new LoginView();
            }
        }
    }
}
