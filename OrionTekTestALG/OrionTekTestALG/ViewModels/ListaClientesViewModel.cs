using Acr.UserDialogs;
using OrionTekTestALG.Model;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrionTekTestALG.ViewModels
{
    public class ListaClientesViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Cliente> ListadoCliente { get; set; }
        public ICommand BorrarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand ClienteSeleccionado { get; set; }
        #endregion
        public ListaClientesViewModel(INavigationService navigationService, IUserDialogs userDialogs)
          : base(navigationService, userDialogs)
        {
            Title = "Listado de Clientes";
            BorrarCommand = new Command<Cliente>(BorrarCliente);
            EditarCommand = new Command<Cliente>(EditarCliente);
            ClienteSeleccionado = new Command<Cliente>(clienteSelected);
        }

        #region Methods
        private void clienteSelected(Cliente obj)
        {
            var navigationParams = new NavigationParameters
                {
                    { "Cliente",obj }
                };
            NavigationService.NavigateAsync("ListadoDireccionesViewPage", navigationParams);
        }
        private void EditarCliente(Cliente obj)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                NavigationService.NavigateAsync("ClienteRegionViewPage");
                MessagingCenter.Send(this, "EditarCliente", obj);
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
        private async void BorrarCliente(Cliente obj)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                if (await UserDialogsGlobal.ConfirmAsync("Seguro que desea Eliminar este cliente?", "Atencion!", "Si", "No"))
                {
                    if ((await App.clienteDb.DeleteCliente(obj)) > 0)
                    {
                        Init();
                        UserDialogsGlobal.Alert("Cliente Eliminado!", "Atencion!", "Ok");
                    }
                }
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
        public async void Init()
        {
            using (UserDialogsGlobal.Loading("Cargando..."))
            {
                ListadoCliente = new ObservableCollection<Cliente>(await App.clienteDb.GetClientes());
            }
        }
        #endregion
    }
}
