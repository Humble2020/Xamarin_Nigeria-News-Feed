using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace _9jaNews.Utils
{
	public class Loading : ActivityIndicator
	{
		public Loading()
		{
			VerticalOptions = LayoutOptions.FillAndExpand;
			HorizontalOptions = LayoutOptions.FillAndExpand;
		}

		public void Show()
		{
			Opacity = 0;
			IsVisible = true;
			this.FadeTo(1, 500);
		}

		public void Hide()
		{
			this.FadeTo(0, 500);
			IsVisible = false;
		}
	}
}
