using _9jaNews.Models;
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
public partial class PremiumTimes : ContentPage
{
        ObservableCollection<PremiumTimesModel> _PMnewsFeeds = new ObservableCollection<PremiumTimesModel>();
        public PremiumTimes()
    {
        InitializeComponent();
            if (_PMnewsFeeds != null)
            {
                PopulateList();
            }
            else
            {
                //label.Text = "Either the feed is empty or the URL is incorrect.";
            }
        }
        private async void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            // To prevent opening multiple pages on double tapping
            premiumList.IsEnabled = false;
            var item = e.Item as PremiumTimesModel;
            await Navigation.PushAsync(new Views.webLoadPremium(item));

            premiumList.IsEnabled = true;
            premiumList.SelectedItem = null;
        }
   
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var rssFeeds = new CodeHollow.FeedReader.Feed();
            try
            {
                rssFeeds = await FeedReader.ReadAsync("https://www.premiumtimesng.com/feed");
            }
            catch (Exception ex)
            {
                _PMnewsFeeds.Add(new PremiumTimesModel() { Title = "Test", Description = "January 2099", Link = "www.example.com" });
                PopulateList();
                return;
            }
            foreach (var item in rssFeeds.Items)
            {
                var feed = new PremiumTimesModel()
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

                _PMnewsFeeds.Add(feed);
            }
            PopulateList();
        }
        private void PopulateList()
        {
            premiumList.ItemsSource = PMnewsFeeds;
        }
        public ObservableCollection<PremiumTimesModel> PMnewsFeeds
        {
            get { return _PMnewsFeeds; }
            set
            {
                SetProperty(ref _PMnewsFeeds, value);
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