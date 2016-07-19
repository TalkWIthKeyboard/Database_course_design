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
using WebApplication1.Controllers;

namespace Database_course_design.Controllers
{
    public class HomeController : Controller
    {
        private DBModel dbmodel = new DBModel();
        private IndexWebInterface iweb = new IndexWebInterface();
        private LoginController lgc = new LoginController();
        string user_id;

        public HomeController(){}

        public HomeController(string user_id)
        {
            this.user_id = user_id;
        }

        public ActionResult Index()
        {
            List<IndexWebInterface.FileItem> ret = null;
            ErrorMessage errorInfo = null;
            iweb.getFIleByRepoId("REPOSITORY_8111840549", out ret, out errorInfo);
            ViewBag.CardContent = ret;



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

        public ActionResult Download(string url,string file)
        {
            using (var client = new WebClient())
            {
                /*url.ToLower(); file.ToLower();*/ 
               var buffer = client.DownloadData(url);
                return File(buffer, "/img", file);
            }
        }

        public ActionResult Personal()
        {
            return View();
        }


    }
}