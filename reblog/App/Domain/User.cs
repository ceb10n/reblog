using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reblog.App.Domain
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Nick { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Facebook { get; set; }
        public virtual string Tumblr { get; set; }
        public virtual string Orkut { get; set; }
        public virtual string LastFM { get; set; }
    }
}