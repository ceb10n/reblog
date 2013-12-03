using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reblog.App.Domain;

namespace reblog.App.Repository
{
    public interface IAdminRepository
    {
        User Get(User user);
        User Save(User user);

        Post Save(Post post);
        
        Owner Save(Owner owner);
        List<Owner> Owners();

        Category Save(Category category);
        List<Category> Categories();

        Tag Get(Tag tag);
        bool TagExists(Tag tag);
        Tag Save(Tag tag);
        List<Tag> Tags();

    }
}