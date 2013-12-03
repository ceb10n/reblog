using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace reblog.App.Domain
{
    public class Owner
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
    }
}
