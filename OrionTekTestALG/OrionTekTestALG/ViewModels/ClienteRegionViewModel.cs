using Acr.UserDialogs;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OrionTekTestALG.ViewModels
{
    public class ClienteRegionViewModel : ViewModelBase
    {
        #region
        private IRegionManager _regionManager { get; }
        public bool IsClienteVisible { get; set; }
        public bool IsDireccionVisible { get; set; }
        #endregion

        public ClienteRegionViewModel(INavigationService navigationService, IUserDialogs userDialogs, IRegionManager regionManager)
            : base(navigationService, userDialogs)
        {
            Title = "Registro de Clientes";
            IsClienteVisible = true;
            _regionManager = regionManager;
        }

        #region Methods
        public override void Initialize(INavigationParameters parameters)
        {
            MessagingCenter.Subscribe<ClienteViewModel>(this, "DireccionesViewPage", (d) =>
            {
                IsClienteVisible = false;
                IsDireccionVisible = true;
            });

            MessagingCenter.Subscribe<ListadoDireccionesViewModel>(this, "DireccionesViewPage", (d) =>
            {
                IsClienteVisible = false;
                IsDireccionVisible = true;
            });
            
            MessagingCenter.Subscribe<DireccionesViewModel>(this, "VentanaCliente", (d) =>
            {
                IsClienteVisible = true;
                IsDireccionVisible = false;
            });

            _regionManager.RequestNavigate("ClienteRegion", "ClienteViewPage");
            _regionManager.RequestNavigate("DireccionRegion", "DireccionesViewPage");
        }

        #endregion
    }
}
