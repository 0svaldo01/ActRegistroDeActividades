using U3ActRegistroDeActividadesMaui.Repositories;
using U3ActRegistroDeActividadesMaui.Services;
using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui
{
    public partial class App : Application
    {
        public static ActividadesService ActividadesService { get; set; } = new();
        public static DepartamentosService DepartamentosService { get; set; } = new();
        public static ActividadesRepository ActividadesRepository { get; set; } = new();
        public static DepartamentosRepository DepartamentosRepository { get; set; } = new();

        public App()
        {
            InitializeComponent();

            //var splashPage = new U3ActRegistroDeActividadesMaui.Views.Splashpage();


            //MainPage = new ContentPage
            //{
            //    Content = splashPage
            //};
            ActividadesService = new ActividadesService();
            DepartamentosService = new DepartamentosService();

            ActividadesRepository = new ActividadesRepository();
            DepartamentosRepository = new DepartamentosRepository();

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

        //private async void Sincronizador()
        //{
        //    while (true)
        //    {
        //        await DepartamentosService.GetDepartamentos();
        //        Thread.Sleep(TimeSpan.FromSeconds(15));
        //    }
        //}

        //buscando otra manera
        //protected override void OnStart()
        //{

        //    Device.StartTimer(TimeSpan.FromSeconds(3), () =>
        //    {
        //        MainPage = new NavigationPage(new AgregarDepView());
        //        return false;
        //    });
        //}
    }
}
