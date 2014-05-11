using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MentorMe.Models;
using System.Web.Routing;

namespace MentorMe.Controllers
{
    public class BaseController : Controller
    {
        public UsersContext model = null;
        public BaseController()
        {
            model = new UsersContext();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);
            }
            catch (Exception ex)
            {

                ErrorLog log = new ErrorLog();
                log.ErrorLogDate = DateTime.Now.ToString();
                log.ExceptionMessage = ex.Message;
                log.ExceptionStackTrace = ex.StackTrace;
                model.ErrorsLog.Add(log);
                model.SaveChanges();

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }
        }
    }
}
