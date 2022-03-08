using Acr.UserDialogs;
using OrionTekTestALG.Model;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace OrionTekTestALG.ViewModels
{
    public class ClienteViewModel : ViewModelBase
    {
        #region Properties
        public DelegateCommand SaveCommand { get; set; }
        public Cliente ClienteModel { get; set; }
        public ObservableCollection<string> Sexo { get; set; }
        public ObservableCollection<string> EstadoCivil { get; set; }

        #endregion
        public ClienteViewModel(INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            ClienteModel = new Cliente();
            Sexo = new ObservableCollection<string>();
            EstadoCivil = new ObservableCollection<string>();
            SaveCommand = new DelegateCommand(Save);
            Load();

            MessagingCenter.Subscribe<ListaClientesViewModel, Cliente>(this, "EditarCliente", (a, cliente) =>
            {
                ClienteModel = cliente;
            });

            MessagingCenter.Subscribe<DireccionesViewModel>(this, "VentanaCliente", (d) =>
            {
                ClienteModel = new Cliente();
            });
        }

        #region Methods
        private async void Save()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (ClienteModel != null)
                {
                    if (Validar())
                    {
                        bool OpenDirection = ClienteModel.Id == 0 ? true : false;
                        var res = await App.clienteDb.Save(ClienteModel);
                        if (res > 0)
                        {
                            UserDialogsGlobal.Alert("Accion Exitosa!", "Atencion!", "Ok");
                            if (OpenDirection)
                            {
                                MessagingCenter.Send(this, "DireccionesViewPage");
                                MessagingCenter.Send(this, "ClienteID", ClienteModel.Id);
                            }
                            else
                                await NavigationService.GoBackAsync();
                        }
                        else
                            UserDialogsGlobal.Alert("Error al intentar guardar el cliente!", "Atencion!", "Ok");
                    }
                    else
                        UserDialogsGlobal.Alert("Por favor llenen todo los campos del formulario!", "Atencion!", "Ok");
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
        private bool Validar()
        {
            bool paso = true;
            if (string.IsNullOrEmpty(ClienteModel.Nombre))
            {
                paso = false;
            }
            if (string.IsNullOrEmpty(ClienteModel.Apellido))
            {
                paso = false;
            }
            if (string.IsNullOrEmpty(ClienteModel.Correo))
            {
                paso = false;
            }
            if (!Regex.IsMatch(ClienteModel.Correo, @"^[a-z-A-Z_0-9'-''.']{1,}?[\@]{1}[a-z-A-Z]{1,}[.][a-z-A-Z'.']{2,}?$"))
            {
                paso = false;
            }
            if (string.IsNullOrEmpty(ClienteModel.Telefono))
            {
                paso = false;
            }
            return paso;
        }
        private void Load()
        {
            Sexo.Add("Masculino");
            Sexo.Add("Femenino");

            EstadoCivil.Add("Soltero");
            EstadoCivil.Add("Casado");
            EstadoCivil.Add("Divorseado");
            EstadoCivil.Add("Viudo");
        }
        #endregion
    }
}
