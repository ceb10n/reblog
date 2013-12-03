using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace reblog.App.Domain.Map
{
    public class PostMap : ClassMapping<Post>
    {
        public PostMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Name, m => m.NotNullable(true));
            Property(x => x.Summary, m =>
            {
                m.Length(350);
                m.NotNullable(true);
            });
            Property(x => x.Date, m => m.NotNullable(true));
            Property(x => x.Hits);
            Property(x => x.Url);
            Property(x => x.Priority);
            ManyToOne(x => x.Owner, m =>
            {
                m.Cascade(Cascade.None);
                m.Column("OwnerId");
                m.Class(typeof(Owner));
            });

            ManyToOne(x => x.WhoPosted, m =>
            {
                m.Cascade(Cascade.None);
                m.Column("UserId");
                m.Class(typeof(User));
            });

            ManyToOne(x => x.Category, m =>
            {
                m.Cascade(Cascade.None);
                m.Column("CategoryId");
                m.Class(typeof(Category));
            });

            Bag(t => t.Tags, bag =>
            {
                bag.Table("TagsPosts");
                bag.Cascade(Cascade.DeleteOrphans);
            }, t => t.ManyToMany(c =>
            {
                c.Column("TagId");
                c.Lazy(LazyRelation.Proxy);
            }));
        }
    }
}