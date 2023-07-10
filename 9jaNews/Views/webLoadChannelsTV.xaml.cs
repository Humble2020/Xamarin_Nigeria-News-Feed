using _9jaNews.Models;
using _9jaNews.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace _9jaNews.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class webLoadChannelsTV : ContentPage
{
		ChannelsTvModel _rssFeedObject;

		public webLoadChannelsTV(ChannelsTvModel rssFeedObject)
		{
			Title = rssFeedObject.Title;
			_rssFeedObject = rssFeedObject;
			Uri uri = new Uri(_rssFeedObject.Link);
			// For iPhone X
			On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
			InitializeComponent();
			Uri hj = new Uri(_rssFeedObject.Link);
			Browserz.Source = uri;
		}
		void webOnNavigating(object sender, WebNavigatingEventArgs e)
		{
			//HideBrowser();
			Loadingz.Hide();
		}

		void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
			//ShowBrowser();
			Loadingz.Show();
		}

		private void ShowBrowser()
		{

			Browserz.Opacity = 0;
			Browserz.IsVisible = true;
			Browserz.FadeTo(1, 500);
		}

		private void HideBrowser()
		{
			Browserz.FadeTo(0, 500);
			Browserz.IsVisible = false;

		}
	}
}
