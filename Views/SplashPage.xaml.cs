using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CameraExample.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();

            Continue();

        }

        public async void Continue(){
			await Task.Delay(3000);
			await Navigation.PushAsync(new NavigationPage(new Views.MainPage()));
        }

    }
}
