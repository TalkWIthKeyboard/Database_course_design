using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database_course_design.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private DBModel dbmodel = new DBModel();

        public ActionResult Login()
        {
            return View();
        }

        public void Save()
        {
            string username = Request["email"];
            string passwd = Request["password"];
            if(dbmodel.sureUserLoad(username,passwd))
                Response.Redirect("/Home/Index");
            return;
        }

    }
}