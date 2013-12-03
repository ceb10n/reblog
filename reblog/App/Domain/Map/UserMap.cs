using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace reblog.App.Domain.Map
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.GuidComb));
            Property(x => x.Name, m => m.NotNullable(true));
            Property(x => x.Nick, m =>
            {
                m.NotNullable(true);
                m.Unique(true);
            });
            Property(x => x.Email, m =>
            {
                m.NotNullable(true);
                m.Unique(true);
            });
            Property(x => x.Facebook);
            Property(x => x.Tumblr);
            Property(x => x.LastFM);
            Property(x => x.Orkut);
            Property(x => x.Twitter);
            Property(x => x.Password, m => m.NotNullable(true));
        }
    }
}