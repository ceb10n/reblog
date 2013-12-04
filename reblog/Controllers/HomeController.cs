using System.Collections.Generic;
using System.Web.Mvc;
using reblog.App.Domain;
using reblog.App.Service;

namespace reblog.Controllers
{
    public class HomeController : Controller
    {
        readonly IPostService service;

        public HomeController(IPostService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            DefineViewbagForIndex(service.Posts());
            return View();
        }

        public ActionResult Post(long id)
        {
            var post = service.View(id);
            DefineViewbagForPost(post);
            return View(post);
        }

        public ActionResult Top()
        {
            DefineViewbagForIndex(service.TopPosts());
            return View("Index");
        }

        public ActionResult Blogs()
        {
            return View(service.Owners());
        }

        public ActionResult About()
        {
            ViewBag.Users = service.Users();
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult GenericError()
        {
            return View();
        }

        private void DefineViewbagForIndex(List<Post> posts)
        {
            ViewBag.Today = posts;
            ViewBag.Title = "reblog - Links interessantes por pessoas não tão interessantes";
            ViewBag.Description = "reblog - Links interessantes por pessoas não tão interessantes";
            ViewBag.Keywords = "reblog, reblogbr, curiosidades, diversao, fotos, musica, videos, games, gatas, imagens, link, links, agregador, internet, tecnologia, links interessantes, agregador de links, blogs";
            ViewBag.OgUrl = "http://re.blog.br";
            ViewBag.OgTitle = "reblog - inks interessantes por pessoas não tão interessantes";
            ViewBag.OgImg = Url.Content("~/Images/reblog.jpg");
            ViewBag.OgDescription = "reblog - Links interessantes por pessoas não tão interessantes";
        }

        private void DefineViewbagForPost(Post post)
        {
            ViewBag.Description = "reblog - " + post.Summary;
            ViewBag.Title = "reblog - " + post.Name;
            ViewBag.Keywords = post.Keywords();
            ViewBag.OgUrl = post.Url;
            ViewBag.OgTitle = "reblog - " + post.Name;
            ViewBag.OgImg = @Url.Content("~/Uploads/" + post.Date.ToString("dd-MM-yyyy") + "/" + post.Id + ".jpg");
            ViewBag.OgDescription = "reblog - " + post.Name;
        }
    }
}
