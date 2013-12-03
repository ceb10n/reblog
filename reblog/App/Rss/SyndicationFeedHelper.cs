using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Web.Routing;

namespace reblog.App.Rss
{
    public class SyndicationFeedHelper<TFeedItem> where TFeedItem : class
    {
        const string Id = "id";
        ControllerContext context;
        IList<TFeedItem> feedItems;
        SyndicationFeedItemMapper<TFeedItem> feedItemMapper;
        SyndicationFeedOptions feedOptions;
        SyndicationFeed syndicationFeed;
        IList<SyndicationItem> syndicationItems;
        ControllerContext Context
        {
            get
            {
                return this.context;
            }
        }
        IList<TFeedItem> FeedItems
        {
            get
            {
                return this.feedItems;
            }
        }
        SyndicationFeedItemMapper<TFeedItem> FeedItemMapper
        {
            get
            {
                return this.feedItemMapper;
            }
        }
        SyndicationFeedOptions SyndicationFeedOptions
        {
            get
            {
                return this.feedOptions;
            }
        }
        /// <summary>
        /// Constructs the SyndicationFeedHelper.
        /// </summary>
        /// <param name="context">Accepts the current controller context.</param>
        /// <param name="feedItems">Accepts a list of feed items.</param>
        /// <param name="syndicationFeedItemMapper">Accepts a <see cref="SyndicationFeedItemMapper"/>.</param>
        /// <param name="syndicationFeedOptions">Accepts a <see cref="SyndicationFeedOptions"/>.</param>
        public SyndicationFeedHelper
            (
                ControllerContext context,
                IList<TFeedItem> feedItems,
                SyndicationFeedItemMapper<TFeedItem> syndicationFeedItemMapper,
                SyndicationFeedOptions syndicationFeedOptions
            )
        {
            this.context = context;
            this.feedItems = feedItems;
            this.feedItemMapper = syndicationFeedItemMapper;
            this.feedOptions = syndicationFeedOptions;
            syndicationItems = new List<SyndicationItem>();
            SetUpFeedOptions();
        }
        /// <summary>
        /// Creates and returns a <see cref="SyndicationFeed"/>.
        /// </summary>
        /// <returns><see cref="SyndicationFeed"/></returns>
        public SyndicationFeed GetFeed()
        {
            feedItems.ToList().ForEach(feedItem =>
            {
                SyndicationItem syndicationItem = CreateSyndicationItem(feedItem);
                AddItemAuthor(feedItem, syndicationItem);
                AddPublishDate(feedItem, syndicationItem);
                syndicationItems.Add(syndicationItem);
            });
            syndicationFeed.Items = syndicationItems;
            return syndicationFeed;
        }
        private void SetUpFeedOptions()
        {
            syndicationFeed = new SyndicationFeed
                (
                    this.SyndicationFeedOptions.Title,
                    this.SyndicationFeedOptions.Description,
                    new Uri(this.SyndicationFeedOptions.Url)
                );
            AddFeedId();
            AddCopyrightStatement();
            AddLanguageIsoCode();
            AddLastUpdateDateTime();
        }
        private void AddLastUpdateDateTime()
        {
            if (this.SyndicationFeedOptions.LastUpdated != default(DateTimeOffset))
            {
                syndicationFeed.LastUpdatedTime = this.SyndicationFeedOptions.LastUpdated;
            }
        }
        private void AddLanguageIsoCode()
        {
            if (!string.IsNullOrEmpty(this.SyndicationFeedOptions.Language))
            {
                syndicationFeed.Language = this.SyndicationFeedOptions.Language;
            }
        }
        private void AddCopyrightStatement()
        {
            if (!string.IsNullOrEmpty(this.SyndicationFeedOptions.Copyright))
            {
                syndicationFeed.Copyright = new TextSyndicationContent(this.SyndicationFeedOptions.Copyright);
            }
        }
        private void AddFeedId()
        {
            if (!string.IsNullOrEmpty(this.SyndicationFeedOptions.FeedId))
            {
                syndicationFeed.Id = this.SyndicationFeedOptions.FeedId;
            }
        }
        private void AddPublishDate(TFeedItem feedItem, SyndicationItem syndicationItem)
        {
            var publishDate = this.FeedItemMapper.DatePublished.Invoke(feedItem);
            syndicationItem.PublishDate = publishDate;
        }
        private void AddItemAuthor(TFeedItem feedItem, SyndicationItem syndicationItem)
        {
            var authorEmail = this.FeedItemMapper.AuthorEmail == null ? string.Empty : this.FeedItemMapper.AuthorEmail.Invoke(feedItem);
            var authorName = this.FeedItemMapper.AuthorName == null ? string.Empty : this.FeedItemMapper.AuthorName.Invoke(feedItem);
            var authorUrl = this.FeedItemMapper.AuthorUrl == null ? string.Empty : this.FeedItemMapper.AuthorUrl.Invoke(feedItem);
            SyndicationPerson syndicationPerson = new SyndicationPerson();
            if (string.IsNullOrEmpty(authorName))
            {
                syndicationPerson.Name = authorName;
            }
            if (string.IsNullOrEmpty(authorEmail))
            {
                syndicationPerson.Email = authorEmail;
            }
            if (string.IsNullOrEmpty(authorUrl))
            {
                syndicationPerson.Uri = authorUrl;
            }
            if (!string.IsNullOrEmpty(syndicationPerson.Name) || !string.IsNullOrEmpty(syndicationPerson.Email)
                || !string.IsNullOrEmpty(syndicationPerson.Uri))
            {
                syndicationItem.Authors.Add(syndicationPerson);
            }
        }
        private SyndicationItem CreateSyndicationItem(TFeedItem feedItem)
        {
            SyndicationItem syndicationItem = new SyndicationItem
                (
                    this.FeedItemMapper.Title.Invoke(feedItem),
                    this.FeedItemMapper.Content.Invoke(feedItem),
                    this.GetUrl(feedItem)
                );
            return syndicationItem;
        }
        private Uri GetUrl(TFeedItem feedItem)
        {
            UrlHelper urlHelper = new UrlHelper(this.Context.RequestContext);
            var routeValues = new RouteValueDictionary();
            routeValues.Add(Id, this.FeedItemMapper.Id.Invoke(feedItem));
            var action = this.FeedItemMapper.Action == null ? this.FeedItemMapper.ActionString
                : this.FeedItemMapper.Action.Invoke(feedItem);
            var controller = this.FeedItemMapper.Controller == null ? this.FeedItemMapper.ControllerString
                : this.FeedItemMapper.Controller.Invoke(feedItem);
            var url = urlHelper.Action
                (
                    action,
                    controller,
                    routeValues
                );
            Uri uriPath = this.Context.RequestContext.HttpContext.Request.Url;
            var urlString = string.Format("{0}://{1}{2}", uriPath.Scheme, uriPath.Authority, url);
            return new Uri(urlString);
        }
    }
}