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
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _9jaNews.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Dailypost : ContentPage
{
    ObservableCollection<DailyPostFeedItem> _feeds = new ObservableCollection<DailyPostFeedItem>();
		public ICommand RefreshCommand { get; }
		DailypostViewModel dpvm;
		public Dailypost()
    {
        InitializeComponent();
			BindingContext = dpvm = new DailypostViewModel();
			RefreshCommand = new Command(ExecuteRefreshCommand);
			if (_feeds != null)
			{
				PopulateList();
			}
			else
			{
				//label.Text = "Either the feed is empty or the URL is incorrect.";
			}

		}
		private ICommand _selectWalletCommand;
		public ICommand SelectWalletCommand => _selectWalletCommand = new Command<DailyPostFeedItem>(async (value) => await Task.WhenAll(OnWalletTapped(value)));
		private async Task OnWalletTapped(DailyPostFeedItem item)
		{
			await Navigation.PushAsync(new Views.webLoad(item));
		}
		private async void OnItemTapped(object sender, EventArgs args)
		{

			await Device.InvokeOnMainThreadAsync(async () =>
			{
				Frame selectedItem = (Frame)sender;
				selectedItem.IsEnabled = false;
				Color initColor = selectedItem.BackgroundColor;
				selectedItem.BackgroundColor = Color.FromHex("#6A2C8B");
				await Task.Delay(500);
				selectedItem.BackgroundColor = initColor;
				selectedItem.IsEnabled = true;

			});
		}
		private async void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
		{
            // To prevent opening multiple pages on double tapping
            dailypostlist.IsEnabled = false;
            var item = e.Item as DailyPostFeedItem;
            await Navigation.PushAsync(new Views.webLoad(item));

            dailypostlist.IsEnabled = true;
            dailypostlist.SelectedItem = null;
		}
		
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			var rssFeeds = new CodeHollow.FeedReader.Feed();
			try
			{
				rssFeeds = await FeedReader.ReadAsync("https://dailypost.ng/feed/");
			}
			catch (Exception ex)
			{
				_feeds.Add(new DailyPostFeedItem() { Title = "Test", Author = "January 2099", Link = "www.example.com" });
				PopulateList();
				return;
			}
			foreach (var item in rssFeeds.Items)
			{
				var feed = new DailyPostFeedItem()
				{
					Title = item.Title,
					DatE = item.PublishingDateString.Replace("+0000",""),
					Link = item.Link,
					Author = item.Author
					
				};



				XDocument xdoc = XDocument.Parse(rssFeeds.OriginalDocument);
				XNamespace xns = xdoc.Root.GetDefaultNamespace();

				BaseFeedItem bfi = item.SpecificItem;

				if (bfi.Element.Descendants().Any(x => x.Name.LocalName == "thumbnail"))
				{
					feed.Image = bfi.Element.Descendants().First(x => x.Name.LocalName == "thumbnail").Attribute("url").Value;
				}


				_feeds.Add(feed);
			}
			PopulateList();
		}
		async void ExecuteRefreshCommand()
		{
			_feeds.Clear();
			var rssFeeds = new CodeHollow.FeedReader.Feed();
			try
			{
				rssFeeds = await FeedReader.ReadAsync("https://dailypost.ng/feed/");
			}
			catch (Exception ex)
			{
				_feeds.Add(new DailyPostFeedItem() { Title = "Test", Author = "January 2099", Link = "www.example.com" });
				PopulateList();
				return;
			}
			foreach (var item in rssFeeds.Items)
			{
				var feed = new DailyPostFeedItem()
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
					feed.Image = bfi.Element.Descendants().First(x => x.Name.LocalName == "thumbnail").Attribute("url").Value;
				}


				_feeds.Add(feed);
			}
			PopulateList();

			// Stop refreshing
			IsRefreshing = false;
		}
		private void PopulateList()
		{
            dailypostlist.ItemsSource = Feeds;
        }
		public ObservableCollection<DailyPostFeedItem> Feeds
		{
			get { return _feeds; }
			set
			{
				SetProperty(ref _feeds, value);
				OnPropertyChanged();
			}
		}





		bool isRefreshing;
		public bool IsRefreshing
		{
			get => isRefreshing;
			set
			{
				isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
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