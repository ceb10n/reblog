using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reblog.App.Domain;
using reblog.App.Repository;

namespace reblog.App.Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository repository;

        public PostService(IPostRepository repository)
        {
            this.repository = repository;
        }

        public List<Post> Posts()
        {
            return repository.Posts();
        }

        public List<Post> FapPosts()
        {
            return repository.FapPosts();
        }

        public List<Post> TopPosts()
        {
            return repository.TopPosts();
        }

        public List<Owner> Owners()
        {
            return repository.Owners();
        }

        public Post Get(long id)
        {
            return repository.Get(id);
        }

        public Post View(long id)
        {
            var post = repository.Get(id);
            if (post != null)
                post.Hits += 1;
            return repository.Update(post);
        }

        public void Update(Post post)
        {
            repository.Update(post);
        }

        public List<User> Users()
        {
            return repository.Users();
        }
    }
}