using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reblog.App.Rss;
using reblog.App.Service;

namespace reblog.Controllers
{
    public class RssController : Controller
    {
        readonly IPostService service;

        public RssController(IPostService service)
        {
            this.service = service;
        }

        public ActionResult Rss()
        {
            SyndicationFeedItemMapper<MyFeedItem> mapper = SetUpFeedMapper();
            SyndicationFeedOptions options = SetUpFeedOptions();

            List<MyFeedItem> feedItems = new List<MyFeedItem>();
            var feeds = service.Posts();

            foreach (var feed in feeds)
                feedItems.Add(new MyFeedItem
                {
                    CreatedBy = feed.WhoPosted.Nick,
                    DateAdded = feed.Date,
                    Description = feed.Summary,
                    Id = feed.Id,
                    Title = feed.Name
                });
            SyndicationFeedHelper<MyFeedItem> feedHelper = SetUpFeedHelper(mapper, options, feedItems);
            return new RssActionResult(feedHelper.GetFeed());
        }

        private SyndicationFeedHelper<MyFeedItem> SetUpFeedHelper(SyndicationFeedItemMapper<MyFeedItem> mapper,
            SyndicationFeedOptions options, List<MyFeedItem> feedItems)
        {
            SyndicationFeedHelper<MyFeedItem> feedHelper = new SyndicationFeedHelper<MyFeedItem>
                (
                    this.ControllerContext,
                    feedItems,
                    mapper,
                    options
                );
            return feedHelper;
        }

        private static SyndicationFeedItemMapper<MyFeedItem> SetUpFeedMapper()
        {
            SyndicationFeedItemMapper<MyFeedItem> mapper = new SyndicationFeedItemMapper<MyFeedItem>
                (
                    f => f.Title,
                    f => f.Description,
                    "Home",
                    "Articles",
                    f => f.Id.ToString(),
                    f => f.DateAdded
                );
            return mapper;
        }

        private static SyndicationFeedOptions SetUpFeedOptions()
        {
            SyndicationFeedOptions options = new SyndicationFeedOptions
                (
                    "Reblog",
                    "reblogs do reblog",
                    "http://re.blog.br"
                );
            return options;
        }
    }
}
