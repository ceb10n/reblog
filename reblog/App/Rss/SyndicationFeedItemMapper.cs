using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reblog.App.Rss
{
    public class SyndicationFeedItemMapper<TFeedItem> where TFeedItem : class
    {
        Func<TFeedItem, string> title;
        Func<TFeedItem, string> content;
        Func<TFeedItem, string> controller;
        Func<TFeedItem, string> action;
        Func<TFeedItem, string> id;
        Func<TFeedItem, DateTimeOffset> datePublished;
        string controllerString;
        string actionString;
        public SyndicationFeedItemMapper
            (
                Func<TFeedItem, string> title,
                Func<TFeedItem, string> content,
                string controller,
                string action,
                Func<TFeedItem, string> id,
                Func<TFeedItem, DateTimeOffset> datePublished
            )
            : this(title, content, id, datePublished)
        {
            this.controllerString = controller;
            this.actionString = action;
        }
        public SyndicationFeedItemMapper
            (
                Func<TFeedItem, string> title,
                Func<TFeedItem, string> content,
                Func<TFeedItem, string> controller,
                Func<TFeedItem, string> action,
                Func<TFeedItem, string> id,
                Func<TFeedItem, DateTimeOffset> datePublished
            )
            : this(title, content, id, datePublished)
        {
            this.controller = controller;
            this.action = action;
        }
        protected SyndicationFeedItemMapper
            (
                Func<TFeedItem, string> title,
                Func<TFeedItem, string> content,
                Func<TFeedItem, string> id,
                Func<TFeedItem, DateTimeOffset> datePublished
            )
        {
            this.title = title;
            this.content = content;
            this.id = id;
            this.datePublished = datePublished;
        }
        public Func<TFeedItem, string> Title
        {
            get
            {
                return this.title;
            }
        }
        public Func<TFeedItem, string> Content
        {
            get
            {
                return this.content;
            }
        }
        public Func<TFeedItem, string> Controller
        {
            get
            {
                return this.controller;
            }
        }
        public Func<TFeedItem, string> Action
        {
            get
            {
                return this.action;
            }
        }
        public Func<TFeedItem, string> Id
        {
            get
            {
                return this.id;
            }
        }
        public Func<TFeedItem, DateTimeOffset> DatePublished
        {
            get
            {
                return this.datePublished;
            }
        }
        public string ControllerString
        {
            get
            {
                return this.controllerString;
            }
        }
        public string ActionString
        {
            get
            {
                return this.actionString;
            }
        }
        public Func<TFeedItem, string> AuthorName { get; set; }
        public Func<TFeedItem, string> AuthorEmail { get; set; }
        public Func<TFeedItem, string> AuthorUrl { get; set; }

    }
}