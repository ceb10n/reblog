using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace reblog.App.Domain.Map
{
    public class OwnerMap : ClassMapping<Owner>
    {
        public OwnerMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.GuidComb));
            Property(x => x.Name, m => m.NotNullable(true));
            Property(x => x.Url, m => m.NotNullable(true));
        }
    }
}