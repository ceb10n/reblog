using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using reblog.App.Domain;

namespace reblog.App.Repository
{
    public interface IPostRepository
    {
        List<Post> Posts();
        List<Post> FapPosts();
        List<Post> TopPosts();
        List<Owner> Owners();
        Post Get(long id);
        Post Update(Post post);
        List<User> Users();
    }
}