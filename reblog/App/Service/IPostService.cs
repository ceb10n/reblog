using System.Collections.Generic;
using reblog.App.Domain;

namespace reblog.App.Service
{
    public interface IPostService
    {
        List<Post> Posts();
        List<Post> TopPosts();
        List<Owner> Owners();
        List<User> Users();
        Post View(long id);
    }
}