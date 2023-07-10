using System;
using System.Collections.Generic;
using System.Text;

namespace _9jaNews.Services
{
    public static class Constants
    {
		//DailyPostFeedItem Confs
		public static string dailypost_ADDRESS = $"{RSSconvert}" + "https://dailypost.ng/feed/";

		//Misc
		public static uint FADDING_TIME = 500;
		public static string RSSconvert = "https://api.rss2json.com/v1/api.json?rss_url=";
	}
}
