using System.Web.Mvc;
using Microsoft.Practices.Unity;
using NHibernate;
using reblog.App.Repository;
using reblog.App.Service;
using Unity.Mvc4;

namespace reblog
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IPostService, PostService>();
            container.RegisterType<IAdminService, AdminService>();

            container.RegisterType<IPostRepository, PostRepository>();
            container.RegisterType<IAdminRepository, AdminRepository>();

            container.RegisterType<ISession>(new InjectionFactory(c => { return MvcApplication.CurrentSession; }));
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}