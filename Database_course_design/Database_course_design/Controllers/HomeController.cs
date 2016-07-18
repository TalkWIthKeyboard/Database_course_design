using Database_course_design.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Threading;
using Database_course_design.Models.ItemModel;

namespace Database_course_design.Controllers
{
    public class HomeController : Controller
    {
        private DBModel dbmodel = new DBModel();
        private IndexWebInterface iweb = new IndexWebInterface();
        string user_id;

        public HomeController(string user_id)
        {
            this.user_id = user_id;
        }

        public ActionResult Index()
        {

            return View();
        }

        /*获取单击后仓库的id*/
        public void getRepository()
        {

        }

        /*得到卡片的内容*/
        [NonAction]
        private void getCardContent()
        {
            List<RepertorySearchResult> SearchResut = null;
            ErrorMessage errorInfo = null;
            iweb.getRepositoryByLabel(user_id,out SearchResut,out errorInfo);
            ViewBag.SearchResut = SearchResut;
            ViewBag.ErrorInfo = errorInfo;
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