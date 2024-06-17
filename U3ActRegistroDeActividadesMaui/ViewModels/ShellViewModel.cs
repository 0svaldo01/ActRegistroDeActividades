using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        public ShellViewModel()
        {
            // Creacion de hilo para verificar que el token sea valido
            //Thread hilo = new(new ParameterizedThreadStart(VerificarToken))
            //{
            //    IsBackground = true,
            //};
            //hilo.Start();
        }

        //private void VerificarToken(object? obj)
        //{
        //Pendiente validar el token
        //    var token = SecureStorage.GetAsync("tkn");
        //    if (string.IsNullOrWhiteSpace(token.Result))
        //    {
        //        //Elimina el token
        //        SecureStorage.Remove("tkn");
        //    }
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
