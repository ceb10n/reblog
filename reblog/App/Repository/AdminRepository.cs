using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Criterion;
using reblog.App.Domain;

namespace reblog.App.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ISession session;

        public AdminRepository(ISession session)
        {
            this.session = session;
        }

        public User Get(User user)
        {
            return session.CreateCriteria<User>()
                .Add(Restrictions.Or(Restrictions.Eq("Email", user.Email), Restrictions.Eq("Nick", user.Nick)))
                .UniqueResult<User>();
        }

        public Tag Get(Tag tag)
        {
            return session.CreateCriteria<Tag>()
                .Add(Restrictions.Eq("Name", tag.Name))
                .UniqueResult<Tag>();
        }

        public List<Owner> Owners()
        {
            return session.CreateCriteria<Owner>()
                .AddOrder(Order.Asc("Name"))
                .List<Owner>() as List<Owner>;
        }

        public List<Tag> Tags()
        {
            return session.CreateCriteria<Tag>()
                .AddOrder(Order.Asc("Name"))
                .List<Tag>() as List<Tag>;
        }

        public bool TagExists(Tag tag)
        {
            var tags = session.CreateCriteria<Tag>().Add(Restrictions.Eq("Name", tag.Name)).List<Tag>();
            return tags != null && tags.Count > 0;
        }

        public List<Category> Categories()
        {
            return session.CreateCriteria<Category>()
                .AddOrder(Order.Asc("Name"))
                .List<Category>() as List<Category>;
        }

        public User Save(User user)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }
            return user;
        }

        public Tag Save(Tag tag)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var newtag = session.CreateCriteria<Tag>()
                    .Add(Restrictions.Eq("Name", tag.Name))
                    .UniqueResult<Tag>();

                if (newtag == null || newtag.Id == Guid.Empty)
                {
                    session.Save(tag);
                    transaction.Commit();
                }
            }
            return tag;
        }

        public Category Save(Category category)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var newcategory = session.CreateCriteria<Category>()
                    .Add(Restrictions.Eq("Name", category.Name))
                    .UniqueResult<Category>();

                if (newcategory == null || newcategory.Id == Guid.Empty)
                {
                    session.Save(category);
                    transaction.Commit();
                }
            }
            return category;
        }

        public Owner Save(Owner owner)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var newowner = session.CreateCriteria<Owner>()
                    .Add(Restrictions.Eq("Name", owner.Name))
                    .UniqueResult<Owner>();

                if (newowner == null || newowner.Id == Guid.Empty)
                {
                    session.Save(owner);
                    transaction.Commit();
                }
            }
            return owner;
        }

        public Post Save(Post post)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(post);
                transaction.Commit();
            }
            return post;
        }
    }
}