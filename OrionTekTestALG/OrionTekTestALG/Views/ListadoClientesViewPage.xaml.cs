using OrionTekTestALG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OrionTekTestALG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListadoClientesViewPage : ContentPage
    {
        public ListadoClientesViewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ((ListaClientesViewModel)BindingContext).Init();
        }
    }
}