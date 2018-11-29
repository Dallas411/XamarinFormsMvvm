using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsMvvm
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();

            BindingContext = vm;
        }

        //Senza Command ho bisogno di un hendler nel code-behind
        private void ButtonClicked(object sender,
                               EventArgs e)
        {
            //WelcomeTextLabel.Text = "Ciao a tutti!";
            vm.WelcomeText = "Ciao a tutti!";
        }

    }
}
