using Database_course_design.Models;
using Database_course_design.Models.ItemModel;
using Database_course_design.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Threading;
using WebApplication1.Controllers;
using static Database_course_design.Models.IndexWebInterface;

namespace Database_course_design.Controllers
{
    public class HomeController : Controller
    {
        private IndexWebInterface iweb = new IndexWebInterface();
        private LoginController lgc = new LoginController();
        private PersonalWebInterface pweb = new PersonalWebInterface();
        string user_id ="1452716" ; /*默认*/
        string curent_user_id = "1454093"; /*进入对方主页*/


        public ActionResult Index()
        {
            //ser_id = lgc.getUser_id();
            ViewBag.User_ID = user_id;
            ViewBag.Current_User_ID = curent_user_id;
            //getPersonalBasicInfo(user_id);


            Dictionary<string, IndexRepoItem> card;
            ErrorMessage errorInfo = null;
            iweb.RecommendByCourse(user_id,out card,out errorInfo);
            ViewBag.CardContent = card;


            /*被赞次数*/
            /*int? StarNum = 0;
            iweb.getStarNum("REPOSITORY_8111840549", out StarNum, out errorInfo);
            if (StarNum == -1) ViewBag.StarNum = 0;
            else ViewBag.StarNum = StarNum;

            /*Fork次数*/
           /* int? Forknum = 0;
            iweb.getForkNum("REPOSITORY_8111840549", out Forknum, out errorInfo);
            if (StarNum == -1) ViewBag.ForkNum = 0;
            else ViewBag.ForkNum = Forknum;*/

            /*好友动态*/
            List<ActionInfo> SearchResult = null;
            iweb.getFriendDynamic(user_id, out SearchResult, out errorInfo);
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
            //user_id = lgc.getUser_id();
            List<RepertorySearchResult> SearchResut = null;
            ErrorMessage errorInfo = null;
            iweb.getRepositoryByLabel(user_id, out SearchResut, out errorInfo);
            ViewBag.SearchResut = SearchResut;
            ViewBag.ErrorInfo = errorInfo;
        }

        public ActionResult Download(string url, string file)
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
            //user_id = lgc.getUser_id();
            /*统计图表*/
            List<DayHeat> dayheat = null;
            ErrorMessage error = null;
            pweb.getUserHeat(user_id, out dayheat, out error);
            /*dayheat[0].OpList[0].OPERATION
            dayheat[0].OpList[0].TARGET_REPOSITORY_NAME
                dayheat[0].OpList[0].TARGET_USER_NAME*/
            JsonResult js = Json(dayheat);
            return js;
        }

        /*个人主页（详情）*/
        public ActionResult Personal()
        {
            //user_id = lgc.getUser_id();
            /*标签*/
            ViewBag.CardContent = pweb.getPrivate(user_id);
            return View();
        }

        /*个人主页(概览）*/
        public ActionResult OverView()
        {
            //user_id = lgc.getUser_id();
            getPersonalBasicInfo(user_id);

            ViewBag.CardContent = pweb.getPrivate(user_id);
            return View();
        }

        /*public void Redirect()
        {
             Response.Redirect("/Home/Search");

        }*/

        /*留言*/
        public ActionResult Comment()
        {
            //user_id = lgc.getUser_id();
            //ViewBag.Comment = dbmodel.showUserComment("1452693")/*[0].USER_COMMENT_USER.COMMENTTABLE.COMMENT_DATE*/;

            ViewBag.Comment = pweb.getAllComment(user_id);

            return View();
        }



        public ActionResult Repository()
        {
            var repOp = new AboutRepository();
            //ser_id = lgc.getUser_id();
            ViewBag.User_ID = user_id;
            //getPersonalBasicInfo(user_id);

            /*中间点击得到仓库名*/
            string RepName = "操作系统仓库";
            ViewBag.RepName = RepName;
;            /*仓库名转仓库ID*/
            string Rep_ID = pweb.getIDByRepo(RepName, "1452716");
            List<Models.ItemModel.FileInfo> item;
            ErrorMessage errorInfo = null;
            iweb.getFIleByRepoId(Rep_ID,out item,out errorInfo);
            ViewBag.Fork = item;

            /*被赞次数*/
            int? StarNum = 0;
            iweb.getStarNum(Rep_ID, out StarNum, out errorInfo);
            if (StarNum == -1) ViewBag.StarNum = 0;
            else ViewBag.StarNum = StarNum;

            /*Fork次数*/
            int? Forknum = 0;
             iweb.getForkNum(Rep_ID, out Forknum, out errorInfo);
             if (StarNum == -1) ViewBag.ForkNum = 0;
             else ViewBag.ForkNum = Forknum;

            /*仓库管理员人数*/
            ViewBag.ManagerNum = repOp.getRepertoryManageNum(Rep_ID);
            
            return View();
        }

        public void getPersonalBasicInfo(string user_id)
        {
            ViewBag.User_ID = user_id;
            ViewBag.Image = "http://10.0.1.158:3000/Tongji/SE/" + user_id + "/img/head.jpg";
            /*ViewBag.NickName = ; 获得昵称*/
        }

        public ActionResult Search()
        {
            var searchOp = new AboutSearch();
            PersonalWebInterface pwi = new PersonalWebInterface();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            List<PersonalWebInterface.PersonalRepoItem> RepoItem = new List<PersonalWebInterface.PersonalRepoItem>();
            //返回仓库ID
            string teststr = Request["search"];

            var OpList = searchOp.searchRepository(teststr,false);
            //获取仓库项
            foreach(var RepoIDItem in OpList)
            {
                //获取仓库项
                RepertorySearchResult item = new RepertorySearchResult(db.REPOSITORies.Where(p=>p.REPOSITORY_ID == RepoIDItem.RepertoryID).FirstOrDefault());
                PersonalWebInterface.PersonalRepoItem retitem = new PersonalWebInterface.PersonalRepoItem();
                int Aut = pwi.getAuthority(item.RepertoryID);
                retitem.RepoName = item.RepertoryName;
                retitem.RepoDescription = pwi.getRepositoryDes(item.RepertoryID);
                retitem.RepoUpdateDate = item.RepertoryUpdateTime;
                if(1 == Aut)
                {
                    retitem.RepoAuthority = "官方";
                }
                else if(2 == Aut)
                {
                    retitem.RepoAuthority = "原创";
                }
                else if(3 == Aut)
                {
                    retitem.RepoAuthority = "贡献";
                }
                else
                {
                    retitem.RepoAuthority = "";
                }
                RepoItem.Add(retitem);
            }
            ViewBag.SearchResultList = RepoItem;


            return View();
        }
    }
}