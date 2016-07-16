using Database_course_design.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Database_course_design.Models.DBModel;

namespace Database_course_design.Controllers
{
    public class HomeController : Controller
    {
        public DBModel db = new DBModel();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Personal()
        {
            return View();
        }

        public List<FileInfo> GetFile(string resipositoryId)
        {
            List<FileInfo> res = db.showFile(resipositoryId); //name,type,size
            return res;
        }
    }
}