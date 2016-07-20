using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database_course_design.Models;
using Database_course_design.Models.ItemModel;
using Database_course_design.Models.WorkModel;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private static string user_id = "";

        public ActionResult Login()
        {
            return View();
        }

        public void Save()
        {
            var userOp = new AboutUser();
            string username = Request["email"];
            user_id = username;
            string passwd = Request["password"];
            var spiderOp = new PythonOperation();
            var userInfo = spiderOp.spiderUserInfo(username, passwd);
            if (userInfo.name == "")
            {
                Response.Redirect("/Login/Login");
            }
            else
            {
                short iden = 0;
                if (userInfo.identity == "本科生" || userInfo.identity == "研究生")
                {
                    iden = 0;
                }
                else
                {
                    iden = 1;
                }
                userOp.addUserInfo(username,passwd,userInfo.name,userInfo.department,"",iden,0);
                Response.Redirect("/Home/Index");
            }
            return;
        }

        public string getUser_id()
        {
            return user_id;
        }

    }
}