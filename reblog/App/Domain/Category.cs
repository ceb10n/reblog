using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reblog.App.Domain
{
    public class Category
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}