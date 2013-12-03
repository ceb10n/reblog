using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reblog.App.Domain
{
    public class Post
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Url { get; set; }
        public virtual long Hits { get; set; }
        public virtual int Priority { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual User WhoPosted { get; set; }

        public virtual string Keywords()
        {
            var keywords = string.Empty;

            if (Tags == null)
                return keywords;

            foreach (var tag in Tags)
                keywords += tag.Name + ", ";

            return keywords.Remove(keywords.Length - 1);
        }
    }
}