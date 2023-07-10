using _9jaNews.ViewModels;
using _9jaNews.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace _9jaNews
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private Head.HeadView header;
        public AppShell()
        {
            InitializeComponent();
            header = new Head.HeadView();
            header.HeightRequest = 190;
            FlyoutHeaderBehavior = FlyoutHeaderBehavior.CollapseOnScroll;
            FlyoutHeader = header;
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
