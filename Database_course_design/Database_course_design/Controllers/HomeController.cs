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
        private PersonalWebInterface pweb = new PersonalWebInterface();
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
            string field = null;
            ErrorMessage ex = new ErrorMessage();

            for (int i = 0; i <  20; i++)
            {
                var ss = dbmodel.createFolder("REPOSITORY_2392485163", "1452802", "王冠淞测试文件夹" + i.ToString(), "REPOSITORY_2392485163", 0, out field, out ex);
            }
            //var s = dbmodel.createFile("REPOSITORY_2405964274", "王家慧测试文件.exe", "1", 23, "FILETABLE_6976112805", 1, "", out field);
            /*被赞次数*/
            int ? StarNum = 0;
            iweb.getStarNum("REPOSITORY_8111840549",out StarNum,out errorInfo);
            ViewBag.StarNum = StarNum;

            /*Fork次数*/
            int? Forknum = 0;
            iweb.getForkNum("REPOSITORY_8111840549",out Forknum,out errorInfo);
            ViewBag.ForkNum = Forknum;

            /*好友动态*/
            List<actionInfo> SearchResult = null;
            iweb.getFriendDynamic("1452739",out SearchResult,out errorInfo);
            ViewBag.FriendDynamic = SearchResult;



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

       public JsonResult statistics()
        {
            /*统计图表*/
            List<DayHeat> dayheat = null;
            ErrorMessage error = null;
            pweb.getUserHeat("1452716", out dayheat, out error);
            /*dayheat[0].OpList[0].OPERATION
            dayheat[0].OpList[0].TARGET_REPOSITORY_NAME
                dayheat[0].OpList[0].TARGET_USER_NAME*/
            JsonResult js = Json(dayheat);
            return js;
        }

        public ActionResult Personal()
        {
           


            return View();
        }



    }
}