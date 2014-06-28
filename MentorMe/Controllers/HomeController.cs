using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MentorMe.Models;

namespace MentorMe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertFeedback(FeedForm feedForm)
        {
            string Result = string.Empty;
            using (var context = new UsersContext())
            {
                FeedForm oFeedForm = new FeedForm();
                oFeedForm.Comments = feedForm.Comments;
                oFeedForm.CurrentDate = DateTime.Now.ToString();
                oFeedForm.Email = feedForm.Email;
                oFeedForm.Name = feedForm.Name;
                oFeedForm.Rating = feedForm.Rating;
                context.Feedbacks.Add(oFeedForm);
                try
                {
                    context.SaveChanges();
                    Result = "Success";
                }
                catch (Exception ex)
                {
                    Result = "Failure";
                }
            }
            return Json(new { Result }, JsonRequestBehavior.AllowGet);
        }
    }
}
