using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinFormsMvvm
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand ButtonClickedCommand { get; set; }

        public MainPageViewModel()
        {
            ButtonClickedCommand = new Command(OnButtonClicked);
        }

        private string welcomeText = "...";
        public string WelcomeText
        {
            get => welcomeText;
            set => SetValue(ref welcomeText, value);
        }

        private void OnButtonClicked()
        {
            WelcomeText = "Ciao da un Bound Command!!";
}
    }

}
