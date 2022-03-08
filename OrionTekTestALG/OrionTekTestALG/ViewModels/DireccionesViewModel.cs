using Acr.UserDialogs;
using OrionTekTestALG.Model;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;
using Prism.Commands;
using System.Windows.Input;
using System.Linq;

namespace OrionTekTestALG.ViewModels
{
    public class DireccionesViewModel : ViewModelBase
    {
        #region Properties
        private ObservableCollection<Municipios> Municipios { get; set; }
        public ObservableCollection<Municipios> Municipios1 { get; set; }
        public ObservableCollection<Provincias> Provincias { get; set; }
        public Direcciones DireccionesModel { get; set; }
        public Provincias ProvinciaSelected { get; set; }
        public Municipios MunicipiosSelected { get; set; }
        public int IdCliente { get; set; }
        public bool AtrasEnable { get; set; }
        public ICommand ProvinciaSeleccionadaCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AtrasCommand { get; set; }
        #endregion
        public DireccionesViewModel(INavigationService navigationService, IUserDialogs userDialogs)
            : base(navigationService, userDialogs)
        {
            LoadData();
            ProvinciaSeleccionadaCommand = new Command(SelectedProvidence);
            SaveCommand = new Command(Save);
            AtrasCommand = new Command(Atras);
            MunicipiosSelected = new Municipios();
            ProvinciaSelected = new Provincias();

            MessagingCenter.Subscribe<ClienteViewModel, int>(this, "ClienteID", (a, b) =>
            {
                DireccionesModel = new Direcciones();
                IdCliente = b;
                DireccionesModel.ClienteID = b;
                AtrasEnable = true;
            });

            MessagingCenter.Subscribe<ListadoDireccionesViewModel, int>(this, "ClienteID", (a, b) =>
            {
                DireccionesModel = new Direcciones();
                IdCliente = b;
                DireccionesModel.ClienteID = b;
            });

            MessagingCenter.Subscribe<ListadoDireccionesViewModel, Direcciones>(this, "EditarDireccion", (a, b) =>
            {
                DireccionesModel = new Direcciones();
                DireccionesModel = b;
                ProvinciaSelected = Provincias.FirstOrDefault(x => x.provincia == b.Provincia);
                MunicipiosSelected = Municipios.FirstOrDefault(x => x.minicipio == b.Municipio);
            });

            MessagingCenter.Subscribe<ListadoDireccionesViewModel, int>(this, "AgregarDireccion", (a, b) =>
            {
                DireccionesModel = new Direcciones();
                IdCliente = b;
                DireccionesModel.ClienteID = b;
            });
        }


        #region Methods
        private  void Atras()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                DireccionesModel = new Direcciones();
                MessagingCenter.Send(this, "VentanaCliente");
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
        private async void Save()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (DireccionesModel != null)
                {
                    DireccionesModel.Municipio = MunicipiosSelected.minicipio;
                    DireccionesModel.Provincia = ProvinciaSelected.provincia;
                    if (Validar())
                    {
                        using (UserDialogsGlobal.Loading("Enviando..."))
                        {
                            var res = await App.DireccionesDb.Save(DireccionesModel);
                            if (res > 0)
                            {
                                UserDialogsGlobal.Alert("Accion Exitosa!. Puede seguir agregando otra direccion.", "Atencion!", "Ok");
                                DireccionesModel = new Direcciones();
                                DireccionesModel.ClienteID = IdCliente;
                                MunicipiosSelected = null;
                                ProvinciaSelected = null;
                            }
                            else
                                UserDialogsGlobal.Alert("Error al intentar guardar la direccion!", "Atencion!", "Ok");
                        }
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
            if (string.IsNullOrEmpty(DireccionesModel.Alias))
            {
                paso = false;
            }
            if (Municipios.Where(x => x.minicipio == DireccionesModel.Municipio).Count() == 0)
            {
                paso = false;
            }
            if (Provincias.Where(x => x.provincia == DireccionesModel.Provincia).Count() == 0)
            {
                paso = false;
            }
            if (string.IsNullOrEmpty(DireccionesModel.Direccion))
            {
                paso = false;
            }

            return paso;
        }
        private void SelectedProvidence()
        {
            try
            {
                if (ProvinciaSelected != null)
                    Municipios1 = new ObservableCollection<Municipios>(Municipios.Where(x => x.provincia_id == ProvinciaSelected.provincia_id).ToList());
            }
            catch (Exception ex)
            {
                UserDialogsGlobal.Alert(ex.Message, "Error!", "Ok");
            }
        }
        private void LoadData()
        {
            try
            {
                using (UserDialogsGlobal.Loading("Cargando.."))
                {
                    #region Cargando Data de municipios y de provincias de los archivos
                    var assembly = IntrospectionExtensions.GetTypeInfo(typeof(DireccionesViewModel)).Assembly;
                    Stream stream = assembly.GetManifestResourceStream("OrionTekTestALG.Resources.Municipios.txt");
                    Stream stream2 = assembly.GetManifestResourceStream("OrionTekTestALG.Resources.Provincias.txt");

                    using (var reader = new StreamReader(stream))
                    {
                        Municipios = new ObservableCollection<Municipios>(JsonConvert.DeserializeObject<List<Municipios>>(reader.ReadToEnd()).OrderBy(x => x.minicipio).ToList());
                    }

                    using (var reader = new StreamReader(stream2))
                    {
                        Provincias = new ObservableCollection<Provincias>(JsonConvert.DeserializeObject<List<Provincias>>(reader.ReadToEnd()).OrderBy(x => x.provincia).ToList());
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                UserDialogsGlobal.Alert(ex.Message, "Error!", "Ok");
            }
        }
        #endregion
    }
}