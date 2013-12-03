using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace reblog.App.Domain.Map
{
    public class TagMap : ClassMapping<Tag>
    {
        public TagMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.GuidComb));
            Property(x => x.Name, m => m.NotNullable(true));

            Bag(t => t.Posts, bag =>
            {
                bag.Inverse(true);
                bag.Table("TagsPosts");
                bag.Cascade(Cascade.DeleteOrphans);
            }, t => t.ManyToMany(c =>
            {
                c.Column("PostId");
                c.Lazy(LazyRelation.Proxy);
            })); 
        }
    }
}