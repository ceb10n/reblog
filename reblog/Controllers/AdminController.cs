using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using reblog.App.Domain;
using reblog.App.Service;
using reblog.Filters;
using reblog.Models;

namespace reblog.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IPostService postService;

        public AdminController(IAdminService adminService, IPostService postService)
        {
            this.adminService = adminService;
            this.postService = postService;
        }

        [AuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                var user = adminService.Login(model);
                Session["user"] = user;

                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Admin");
            }
            catch (Exception loginException)
            {
                ModelState.AddModelError("", loginException.Message);
            }

            return View(model);
        }

        [AuthenticationFilter]
        public ActionResult Details(int id)
        {
            return View();
        }

        [AuthenticationFilter]
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult CreatePost(Post post, HttpPostedFileBase image, string tags)
        {
            try
            {
                if (post == null || post.Category.Id == Guid.Empty || post.Owner.Id == Guid.Empty)
                    throw new Exception("Adicionar corretamente uma categoria e/ou site");
                post.WhoPosted = Session["user"] as User;
                var newpost = adminService.CreatePost(post, image, tags);
                return View("EditImage", newpost);
                //return RedirectToAction("Index", "Admin");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.Owners = adminService.Owners();
                ViewBag.Tags = adminService.Tags();
                return View();
            }
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult EditImage(Post post)
        {
            return View(post);
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult CropImage(long postid, DateTime postdate, int x, int y, int w, int h)
        {
            try
            {
                adminService.CropImage(postid, postdate, x, y, w, h);

                
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("Error cropping image: {0}", ex.Message);
                Response.StatusCode = 500;
                Response.Write(errorMsg);
                return Json(string.Empty);
            }
        }

        public ActionResult AllOwners(string term)
        {
            var owners = from g in adminService.Owners()
                             where g.Name.ToLower().Contains(term.ToLower())
                             select new
                             {
                                 label = g.Name,
                                 value = g.Id
                             };

            if (owners.Count() == 0)
            {
                return Json(new { label = string.Format("Adicionar {0}?", term) }, JsonRequestBehavior.AllowGet);
            }

            return Json(owners, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllCategories(string term)
        {
            var owners = from g in adminService.Categories()
                         where g.Name.ToLower().Contains(term.ToLower())
                         select new
                         {
                             label = g.Name,
                             value = g.Id
                         };

            if (owners.Count() == 0)
            {
                return Json(new { label = string.Format("Adicionar {0}?", term) }, JsonRequestBehavior.AllowGet);
            }

            return Json(owners, JsonRequestBehavior.AllowGet);
        }

        [AuthenticationFilter]
        public ActionResult CreateTag()
        {
            ViewBag.Tags = adminService.Tags();
            return View();
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult CreateTag(Tag tag)
        {
            try
            {
                adminService.CreateTag(tag);

                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return View();
            }
        }

        [AuthenticationFilter]
        public ActionResult CreateCategory()
        {
            ViewBag.Categories = adminService.Categories();
            return View();
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult CreateCategory(string category)
        {
            try
            {
                var newcategory = adminService.CreateCategory(category);


                if (Request.IsAjaxRequest() == false)
                    return RedirectToAction("Index", "Admin");
                else
                    return Json(new { label = newcategory.Name, value = newcategory.Id });
                
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateSite()
        {
            ViewBag.Owners = adminService.Owners();
            return View();
        }

        [HttpPost]
        public ActionResult CreateSite(string owner)
        {
            try
            {
                var newowner = adminService.CreateOwner(owner);

                if (Request.IsAjaxRequest() == false)
                    return RedirectToAction("Index", "Admin");
                else
                    return Json(new { label = newowner.Name, value = newowner.Id });
            }
            catch
            {
                return View();
            }
        }

        [AuthenticationFilter]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult CreateUser(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Session["user"] = adminService.Register(model);
                }
                catch (Exception mailException)
                {
                    ModelState.AddModelError("Email Duplicado", mailException.Message);
                }
            }
            return View(model);
        }
    }
}
