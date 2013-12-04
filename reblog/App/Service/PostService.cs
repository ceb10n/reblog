using System.Collections.Generic;
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

        public List<Post> TopPosts()
        {
            return repository.TopPosts();
        }

        public List<Owner> Owners()
        {
            return repository.Owners();
        }

        public List<User> Users()
        {
            return repository.Users();
        }

        public Post View(long id)
        {
            var post = repository.Get(id);
            if (post != null)
                post.Hits += 1;
            return repository.Update(post);
        }
    }
}