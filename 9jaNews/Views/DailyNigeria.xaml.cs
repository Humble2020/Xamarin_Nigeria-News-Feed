using _9jaNews.Models;
using _9jaNews.ViewModels;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _9jaNews.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DailyNigeria : ContentPage
{
		ObservableCollection<DailyNigeriaModel> _feeds = new ObservableCollection<DailyNigeriaModel>();
		DailyNigeriaViewModel dvm;
		public DailyNigeria()
    {
        InitializeComponent();
		BindingContext = dvm = new DailyNigeriaViewModel();

		}
		private async void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
		{
			// To prevent opening multiple pages on double tapping
			dailypostlist.IsEnabled = false;
			var item = e.Item as DailyNigeriaModel;
			await Navigation.PushAsync(new Views.webLoadDailyNigeria(item));

			dailypostlist.IsEnabled = true;
			dailypostlist.SelectedItem = null;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			var rssFeeds = new CodeHollow.FeedReader.Feed();
			try
			{
				rssFeeds = await FeedReader.ReadAsync("https://dailynigerian.com/feed/");
			}
			catch (Exception ex)
			{
				_feeds.Add(new DailyNigeriaModel() { Title = "Test", Description = "January 2099", Link = "www.example.com" });
				PopulateList();
				return;
			}
			foreach (var item in rssFeeds.Items)
			{
				var feed = new DailyNigeriaModel()
				{
					Title = item.Title,
					DatE = item.PublishingDateString.Replace("+0000", ""),
					Link = item.Link
				};



				XDocument xdoc = XDocument.Parse(rssFeeds.OriginalDocument);
				XNamespace xns = xdoc.Root.GetDefaultNamespace();

				BaseFeedItem bfi = item.SpecificItem;

				if (bfi.Element.Descendants().Any(x => x.Name.LocalName == "thumbnail"))
				{
					feed.Image = bfi.Element.Descendants().First(x => x.Name.LocalName == "thumbnail").Attribute("url").Value;
				}
				if (bfi.Element.Descendants().Any(x => x.Name.LocalName == "description"))
				{
					feed.Description = bfi.Element.Descendants().First(x => x.Name.LocalName == "description").Value;
				}

				_feeds.Add(feed);
			}
			PopulateList();
		}
		private void PopulateList()
		{
			dailypostlist.ItemsSource = Feeds;
		}
		public ObservableCollection<DailyNigeriaModel> Feeds
		{
			get { return _feeds; }
			set
			{
				SetProperty(ref _feeds, value);
				OnPropertyChanged();
			}
		}

		protected bool SetProperty<T>(ref T backingStore, T value,
			[CallerMemberName] string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;
		}

		#region INotifyPropertyChanged
		public new event PropertyChangedEventHandler PropertyChanged;
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			var changed = PropertyChanged;
			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}