using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using reblog.App.Domain;
using reblog.App.Repository;
using reblog.Models;

namespace reblog.App.Service
{
    public interface IAdminService
    {
        User Login(LoginModel login);
        User Register(RegisterModel user);
        User Get(User user);
        List<Owner> Owners();
        List<Tag> Tags();
        List<Category> Categories();
        Post CreatePost(Post post, HttpPostedFileBase image, string tags);
        void CropImage(long postid, DateTime postdate, int x, int y, int w, int h);
        void CreateTag(Tag tag);
        void CreateOwner(Owner owner);
        Owner CreateOwner(string owner);
        void CreateCategory(Category category);
        Category CreateCategory(string category);
    }
}