using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace OrionTekTestALG.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region
        private IRegionManager _regionManager { get; }
        public DelegateCommand ListaDClientesCommand { get; set; }
        public DelegateCommand RegistroClienteCommand { get; set; }
        #endregion
        public MainPageViewModel(INavigationService navigationService, IUserDialogs userDialogs, IRegionManager regionManager)
            : base(navigationService, userDialogs)
        {
            Title = "Test App";
            ListaDClientesCommand = new DelegateCommand(ListadoCliente);
            RegistroClienteCommand = new DelegateCommand(RClientes);
        }

        #region Methods
        private void RClientes()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                NavigationService.NavigateAsync("ClienteRegionViewPage");
            }
            catch (Exception ex)
            {
                UserDialogsGlobal.Alert(ex.Message, "Error!", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void ListadoCliente()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                NavigationService.NavigateAsync("ListadoClientesViewPage");
            }
            catch (Exception ex)
            {
                UserDialogsGlobal.Alert(ex.Message, "Error!", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

    }
}
