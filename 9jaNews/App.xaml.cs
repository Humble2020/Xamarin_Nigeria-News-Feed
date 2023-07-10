using _9jaNews.Services;
using _9jaNews.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _9jaNews
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
