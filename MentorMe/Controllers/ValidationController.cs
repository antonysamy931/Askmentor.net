using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MentorMe.Models;

namespace MentorMe.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : Controller
    {
        public static string GetAltName(string username)
        {
            string sugectionUID = String.Format(CultureInfo.InvariantCulture,
                "{0} is not available. ", username);

            for (int i = 1; i < 100; i++)
            {
                var ouser = username.Split('@');
                string newusername = ouser[0] + i.ToString() + "@" + ouser[1];
                var db=new UsersContext();
                if (!db.UserProfiles.Any(x => x.UserName == newusername))
                {
                    sugectionUID = String.Format(CultureInfo.InvariantCulture,
                        "{0} is not available. Try {1}", username, newusername);
                    break;
                }
            }
            return sugectionUID;
        }

        public JsonResult IsUserNameAvailable(string username)
        {
            var db=new UsersContext();
            if (!db.UserProfiles.Any(x => x.UserName == username))
                return Json(true, JsonRequestBehavior.AllowGet);
            string SugectionUsername = GetAltName(username);
            return Json(SugectionUsername, JsonRequestBehavior.AllowGet);
        }

    }
}
