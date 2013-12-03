using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reblog.App.Rss
{
    public class SyndicationFeedOptions
    {
        string title;
        string description;
        string url;
        public SyndicationFeedOptions(string title, string description, string url)
        {
            this.title = title;
            this.description = description;
            this.url = url;
        }

        public string Title
        {
            get
            {
                return this.title;
            }
        }
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        public string Url
        {
            get
            {
                return this.url;
            }
        }
        public string FeedId { get; set; }
        public DateTimeOffset LastUpdated { get; set; }
        public string Copyright { get; set; }
        public string Language { get; set; }
    }
}