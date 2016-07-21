using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database_course_design.Models;
using Database_course_design.Models.ItemModel;
using Database_course_design.Models.WorkModel;
using Database_course_design.Models.ScriptModel;

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
            var spiderOp = new PythonOperation();
            var db = new KUXIANGDBEntities();
            string username = Request["email"];
            user_id = username; /*先设默认值*/   /*username; 获取密码*/
            string passwd = Request["password"];

            var user = db.USERTABLEs.Where(p => p.USER_ID == user_id).FirstOrDefault();
            //先查找用户账号是否在数据库中
            if (user != null)
            {
                if (user.PASSWORD == passwd)
                {
                    Response.Redirect("/Home/Index");
                }
                else
                {
                    Response.Redirect("/Login/Login");
                }
            }
            else
            {
                //不在数据库中，就进行爬虫
                var userInfo = spiderOp.spiderUserInfo(username, passwd);
                if (userInfo.name == "")
                {
                    Response.Redirect("/Login/Login");
                }
                else
                {
                    short iden = 0;
                    if (userInfo.identity == "本专科生" || userInfo.identity == "研究生")
                    {
                        iden = 0;
                    }
                    else
                    {
                        iden = 1;
                    }
                    userOp.addUserInfo(username, passwd, userInfo.name, userInfo.department, "", iden, 0);
                    Response.Redirect("/Home/Index");
                }
            }
            return;
        }

        public string getUser_id()
        {
            return user_id;
        }

    }
}