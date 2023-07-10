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
public partial class ChannelsTV : ContentPage
{
        ObservableCollection<ChannelsTvModel> _feeds = new ObservableCollection<ChannelsTvModel>();
		ChannelstvViewModel cvm { get; set; }
        public ChannelsTV()
		{
			InitializeComponent();
			BindingContext = cvm = new ChannelstvViewModel();

		}

		private async void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
		{
			// To prevent opening multiple pages on double tapping
			ChannelsNewslist.IsEnabled = false;
			var item = e.Item as ChannelsTvModel;
			await Navigation.PushAsync(new Views.webLoadChannelsTV(item));

			ChannelsNewslist.IsEnabled = true;
			ChannelsNewslist.SelectedItem = null;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			var rssFeeds = new CodeHollow.FeedReader.Feed();
			try
			{
				rssFeeds = await FeedReader.ReadAsync("https://www.channelstv.com/feed/");
			}
			catch (Exception ex)
			{
				_feeds.Add(new ChannelsTvModel() { Title = "Test", Author = "January 2099", Link = "www.example.com" });
				PopulateList();
				return;
			}
			foreach (var item in rssFeeds.Items)
			{
				var feed = new ChannelsTvModel()
				{
					Title = item.Title,
					DatE = item.PublishingDateString.Replace("+0000", ""),
					Link = item.Link,
					Author = item.Author

				};
				XDocument xdoc = XDocument.Parse(rssFeeds.OriginalDocument);
				XNamespace xns = xdoc.Root.GetDefaultNamespace();

				BaseFeedItem bfi = item.SpecificItem;

				if (bfi.Element.Descendants().Any(x => x.Name.LocalName == "thumbnail"))
				{
					feed.Image = bfi.Element.Descendants().First(x => x.Name.LocalName == "thumbnail").Attribute("src").Value;
				}
				_feeds.Add(feed);
			}
			PopulateList();
		}
		private void PopulateList()
		{
			ChannelsNewslist.ItemsSource = Feeds;
		}
		public ObservableCollection<ChannelsTvModel> Feeds
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