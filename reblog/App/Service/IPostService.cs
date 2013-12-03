using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reblog.App.Domain;

namespace reblog.App.Service
{
    public interface IPostService
    {
        List<Post> Posts();
        List<Post> FapPosts();
        List<Post> TopPosts();
        List<Owner> Owners();
        List<User> Users();
        Post Get(long id);
        Post View(long id);
        void Update(Post post);
    }
}