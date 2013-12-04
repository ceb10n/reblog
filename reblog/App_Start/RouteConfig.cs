using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace reblog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "topposts",
                url: "top",
                defaults: new { controller = "Home", action = "Top", nick = UrlParameter.Optional });

            routes.MapRoute(
                name: "blogs",
                url: "blogs",
                defaults: new { controller = "Home", action = "Blogs", nick = UrlParameter.Optional });

            routes.MapRoute(
                name: "userdetail",
                url: "user/{nick}",
                defaults: new { controller = "Home", action = "UserDetail", nick = UrlParameter.Optional });

            routes.MapRoute(
                name: "rss",
                url: "rss",
                defaults: new { controller = "Rss", action = "Rss" });

            routes.MapRoute(
                name: "about",
                url: "sobre",
                defaults: new { controller = "Home", action = "About" });

            routes.MapRoute(
                name: "postsfromuser",
                url: "posts/user/{user}",
                defaults: new { controller = "Home", action = "User", nick = UrlParameter.Optional });

            routes.MapRoute(
                name: "postswithcategory",
                url: "posts/category/{category}",
                defaults: new { controller = "Home", action = "Category", tag = UrlParameter.Optional });

            routes.MapRoute(
                name: "postsfromowner",
                url: "posts/owner/{owner}",
                defaults: new { controller = "Home", action = "Owner", tag = UrlParameter.Optional });

            routes.MapRoute(
                name: "postspage",
                url: "posts/{id}",
                defaults: new { controller = "Home", action = "Post", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "admin",
                url: "adminwithlasers",
                defaults: new { controller = "Admin", action = "Index", tag = UrlParameter.Optional });

            routes.MapRoute(
                name: "adminlogin",
                url: "adminwithlasers/login",
                defaults: new { controller = "Admin", action = "Login", tag = UrlParameter.Optional });

            routes.MapRoute(
                name: "admincreatepost",
                url: "adminwithlasers/createpost",
                defaults: new { controller = "Admin", action = "CreatePost", tag = UrlParameter.Optional });

            routes.MapRoute(
                name: "admincreatetag",
                url: "adminwithlasers/createtag",
                defaults: new { controller = "Admin", action = "CreateTag", tag = UrlParameter.Optional });

            routes.MapRoute(
               name: "admincreatesite",
               url: "adminwithlasers/createsite",
               defaults: new { controller = "Admin", action = "CreateSite", tag = UrlParameter.Optional });

            routes.MapRoute(
              name: "admincreateuser",
              url: "adminwithlasers/createuser",
              defaults: new { controller = "Admin", action = "CreateUser", tag = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
