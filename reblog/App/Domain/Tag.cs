using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reblog.App.Domain
{
    public class Tag
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}