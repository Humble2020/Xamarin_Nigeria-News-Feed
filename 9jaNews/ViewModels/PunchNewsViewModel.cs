﻿using _9jaNews.Models;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;

namespace _9jaNews.ViewModels
{
    public class PunchNewsViewModel : BaseViewModel
{
		ObservableCollection<PunchNewsModel> _feeds = new ObservableCollection<PunchNewsModel>();
		public ICommand RefreshCommand { get; }
		public INavigation Navigation;
		public PunchNewsViewModel()
        {
			RefreshCommand = new Command(ExecuteRefreshCommand);
		}
		private ICommand _selectWalletCommand;
		public ICommand SelectWalletCommand => _selectWalletCommand = new Command<PunchNewsModel>(async (value) => await Task.WhenAll(OnWalletTapped(value)));
		private async Task OnWalletTapped(PunchNewsModel item)
		{
			await Navigation.PushAsync(new Views.webLoadpunchNews(item));
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
		private void PopulateList()
		{
			
		}
		public async void ExecuteRefreshCommand()
        {
			var rssFeeds = new CodeHollow.FeedReader.Feed();
			try
			{
				rssFeeds = await FeedReader.ReadAsync("https://punchng.com/feed/");
			}
			catch (Exception ex)
			{
				_feeds.Add(new PunchNewsModel() { Title = "Test", Description = "January 2099", Link = "www.example.com" });
				PopulateList();
				return;
			}
			foreach (var item in rssFeeds.Items)
			{
				var feed = new PunchNewsModel()
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
			// Stop refreshing
			IsRefreshing = false;
		}
	}
}
