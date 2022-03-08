using Acr.UserDialogs;
using OrionTekTestALG.Model;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrionTekTestALG.ViewModels
{
    public class ListadoDireccionesViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Direcciones> ListaDirecciones { get; set; }
        public Cliente Cliente { get; set; }
        public ICommand BorrarCommand { get; set; }
        public ICommand DireccionSeleccionada { get; set; }
        public ICommand AgregarNuevaDireccion { get; set; }
        #endregion

        public ListadoDireccionesViewModel(INavigationService navigationService, IUserDialogs userDialogs)
           : base(navigationService, userDialogs)
        {
            Title = "Listado de Direcciones";
            BorrarCommand = new Command<Direcciones>(Eliminar);
            DireccionSeleccionada = new Command<Direcciones>(Editar);
            AgregarNuevaDireccion = new Command(AgregarDireccion);
        }



        #region Methods

        private void AgregarDireccion()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                NavigationService.NavigateAsync("ClienteRegionViewPage");
                MessagingCenter.Send(this, "DireccionesViewPage");
                MessagingCenter.Send(this, "AgregarDireccion", Cliente.Id);
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
        private void Editar(Direcciones obj)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                NavigationService.NavigateAsync("ClienteRegionViewPage");
                MessagingCenter.Send(this, "DireccionesViewPage");
                MessagingCenter.Send(this, "EditarDireccion", obj);
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
        private async void Eliminar(Direcciones obj)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                if (await UserDialogsGlobal.ConfirmAsync("Seguro que desea Eliminar esta direccion?", "Atencion!", "Si", "No"))
                {
                    if ((await App.DireccionesDb.DeleteDirecciones(obj)) > 0)
                    {
                        Init();
                        UserDialogsGlobal.Alert("Direccion Eliminada!", "Atencion!", "Ok");
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
        public override  void OnNavigatedTo(INavigationParameters parameters)
        {
            var cliente = parameters.GetValue<Cliente>("Cliente");
            if (cliente != null)
            {
                Cliente = cliente;
            }
            Init();
        }
        private async void Init()
        {
            if (Cliente != null && Cliente.Id > 0)
                ListaDirecciones = new ObservableCollection<Direcciones>((await App.DireccionesDb.GetDirecciones(Cliente.Id)).OrderByDescending(x => x.Predeterminada).ToList());
        }
        #endregion
    }
}
