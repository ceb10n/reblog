using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Criterion;
using reblog.App.Domain;

namespace reblog.App.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ISession session;

        public PostRepository(ISession session)
        {
            this.session = session;
        }

        public List<Post> Posts()
        {
            return session.CreateCriteria<Post>()
                .AddOrder(Order.Desc("Date"))
                .AddOrder(Order.Desc("Priority"))
                .AddOrder(Order.Desc("Hits"))
                .SetMaxResults(64)
                .List<Post>() as List<Post>;
        }

        public List<Post> TopPosts()
        {
            return session.CreateCriteria<Post>()
                .AddOrder(Order.Desc("Hits"))
                .SetMaxResults(64)
                .List<Post>() as List<Post>;
        }

        public List<Owner> Owners()
        {
            var query = "SELECT  {owner.*}, " +
                        "COUNT(post.Id) as count " +
                        "FROM re.Owner as owner " +
                        "LEFT JOIN re.Post as post ON post.OwnerId = owner.Id " +
                        "GROUP BY owner.Id " +
                        "ORDER BY count DESC; ";
            return session.CreateSQLQuery(query)
                .AddEntity("owner", typeof(Owner))
                .List<Owner>() as List<Owner>;
        }

        public Post Get(long id)
        {
            return session.Get<Post>(id);
        }

        public Post Update(Post post)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(post);
                transaction.Commit();
            }
            return post;
        }

        public List<User> Users()
        {
            return session.CreateCriteria<User>().List<User>() as List<User>;
        }
    }
}