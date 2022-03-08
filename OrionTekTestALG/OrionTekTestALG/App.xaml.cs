using Acr.UserDialogs;
using OrionTekTestALG.Model.DataBase;
using OrionTekTestALG.ViewModels;
using OrionTekTestALG.Views;
using Prism;
using Prism.Ioc;
using System;
using System.IO;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace OrionTekTestALG
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        #region Sqlite
        static ClientesDataBase _cliente;
        public static ClientesDataBase clienteDb
        {
            get
            {
                if (_cliente == null)
                {
                    _cliente = new ClientesDataBase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OrionTekDataBase.db3"));

                }
                return _cliente;
            }
        }

        static DireccionesDataBase _direccion;
        public static DireccionesDataBase DireccionesDb
        {
            get
            {
                if (_direccion == null)
                {
                    _direccion = new DireccionesDataBase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OrionTekDataBase.db3"));

                }
                return _direccion;
            }
        }
        #endregion

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterRegionServices();
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);
            containerRegistry.RegisterForNavigation<ClienteRegionViewPage, ClienteRegionViewModel>();
            containerRegistry.RegisterForNavigation<ListadoClientesViewPage, ListaClientesViewModel>();
            containerRegistry.RegisterForNavigation<ListadoDireccionesViewPage, ListadoDireccionesViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForRegionNavigation<ClienteViewPage, ClienteViewModel>();
            containerRegistry.RegisterForRegionNavigation<DireccionesViewPage, DireccionesViewModel>();
        }
    }
}
