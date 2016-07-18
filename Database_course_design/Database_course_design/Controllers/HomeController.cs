using Database_course_design.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Database_course_design.Models.DBModel;
using Database_course_design.Models;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Database_course_design.Controllers
{
    public class HomeController : Controller
    {
        private DBModel dbmodel = new DBModel();

        public ActionResult Index()
        {
            string forkName = "";
            List<DBModel.FileInfo> list = dbmodel.showFile("1060", ref forkName);
            ViewBag.ForkName = forkName;
            ViewBag.FileName = list[0].name;
            ViewBag.FileType = list[0].type;
            ViewBag.FileSize = list[0].size;

            return View();
        }

        public ActionResult Download()
        {
            using (var client = new WebClient())
            {
                var buffer = client.DownloadData("http://10.0.1.158:3000/images/1.jpg");
                return File(buffer, "/img", "1.jpg");
            }
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("ftp://10.0.1.158/public/"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }

        public ActionResult Personal()
        {
            return View();
        }


    }
}