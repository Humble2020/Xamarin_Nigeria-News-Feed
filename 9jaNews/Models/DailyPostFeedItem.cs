using System;
using System.Collections.Generic;
using System.Text;

namespace _9jaNews.Models
{
    public class DailyPostFeedItem
    {
        private string url;
        public string URl
        {
            get { return url; }
            set { url = value; }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string link;

        public string Link
        {
            get { return link; }
            set { link = value; }
        }
        private string datE;

        public string DatE
        {
            get { return datE; }
            set { datE = value; }
        }
        private string author;

        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }
    }
 

}
