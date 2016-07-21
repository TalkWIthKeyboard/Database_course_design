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
        private string user_id ="1452716" ; /*默认*/
        static private string current_user_id = "1452716"; /*进入对方主页*/
        private static int selected =1;/*官方或热度*/
        private static int choice = 1;/*概览*/

        public void follow(bool followed)
        {
            AboutUser ab = new AboutUser();

            if (followed)
            {
                ab.makeFriend(user_id, current_user_id);
            }
            else
            {
                ab.deleteFriend(user_id, current_user_id);
            }

        }

        /*历史浏览*/
        public ActionResult History()
        {
            var userOp = new AboutUser();
            ViewBag.History = userOp.showLookHistory(user_id);
            

            return View();
        }

        /*个人收藏*/
        public ActionResult StarFork()
        {
            var userOp = new AboutUser();
            /*得到Star的仓库*/
            ViewBag.StarForkList = userOp.showOthersRepertory(user_id, 2);
            return View();
        }

        public ActionResult Index()
        {
            getPersonalBasicInfo();

            /*兴趣推荐*/
            Dictionary<string, IndexRepoItem> card;
            ErrorMessage errorInfo = null;
            iweb.RecommendByCourse(user_id, out card, out errorInfo);
            ViewBag.CardContent = card;


            /*官方推荐*/
            Dictionary<string, IndexRepoItem> official;
            iweb.getRepertoryByAttribute(user_id, out official, out errorInfo);
            ViewBag.Official = official;

            /*好友动态*/
            List<ActionInfo> SearchResult = null;
            iweb.getFriendDynamic(user_id, out SearchResult, out errorInfo);
            ViewBag.FriendDynamic = SearchResult;

            /*选择推荐方式*/
            ViewBag.Selected = selected;

            /*概览*/
            ViewBag.Choice = choice;

            return View();
        }

        public void Selected()
        {
            string str = Request["office"];
            if(str == "1")
            {
                selected = 1;
            }
            else
            {
                selected = 0;
            }
            Response.Redirect("/Home/Index");
        }


        /*获取单击后仓库的id*/
        public void getRepository()
        {

        }

        /*得到卡片的内容*/
        /*[NonAction]
        private void getCardContent()
        {
            //user_id = lgc.getUser_id();
            List<RepertorySearchResult> SearchResut = null;
            ErrorMessage errorInfo = null;
            iweb.getRepositoryByLabel(current_user_id, out SearchResut, out errorInfo);
            ViewBag.SearchResut = SearchResut;
            ViewBag.ErrorInfo = errorInfo;
        }*/

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
            pweb.getUserHeat(current_user_id, out dayheat, out error);
            /*dayheat[0].OpList[0].OPERATION
            dayheat[0].OpList[0].TARGET_REPOSITORY_NAME
                dayheat[0].OpList[0].TARGET_USER_NAME*/
            JsonResult js = Json(dayheat);
            return js;
        }

        /*个人主页（详情）*/
        public ActionResult Personal()
        {
            getPersonalBasicInfo();
            //user_id = lgc.getUser_id();
            /*标签*/

            ViewBag.Current_User_ID = current_user_id;

            ViewBag.CardContent = pweb.getPrivate(current_user_id);

            return View();
        }

        /*个人主页(概览）*/
        public ActionResult OverView()
        {
            getPersonalBasicInfo();
            string str = Request["friend_id"];

            if (str != null)
            {
                current_user_id = Request["friend_id"];
            }

            ViewBag.Current_User_ID = current_user_id;

            ViewBag.Follow = pweb.FellowOrNot(user_id, current_user_id);

            ViewBag.CardContent = pweb.getPrivate(current_user_id);

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


            ViewBag.Current_User_ID = current_user_id;
            getPersonalBasicInfo();
            ViewBag.Comment = pweb.getAllComment(current_user_id);



            return View();
        }

        public ActionResult Repository()
        {
            //var a = Request;
            getPersonalBasicInfo();
            string Rep_ID = Request["rID"];
            var repOp = new AboutRepository();
            //ser_id = lgc.getUser_id();
            ViewBag.User_ID = user_id;
            //getPersonalBasicInfo(user_id);

            /*中间点击得到仓库名*/
            //string RepName = "操作系统仓库";
            //ViewBag.RepName = RepName;
;            /*仓库名转仓库ID*/
            //string Rep_ID = pweb.getIDByRepo(RepName, "1452716");
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

            /*仓库留言*/
            var commentOp = new AboutComment();
            ViewBag.ForkComment = commentOp.showRepositoryComment(Rep_ID);

            return View();
        }


        public ActionResult Search()
        {
            getPersonalBasicInfo();

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

        public void Choose()
        {
            string str = Request["type"];
            if (str == "1")
            {
                choice = 1;
                Response.Redirect("/Home/OverView");
            }
            else if(str == "2")
            {
                choice = 2;
                Response.Redirect("/Home/Personal");
            }
            else
            {
                choice = 3;
                Response.Redirect("/Home/Comment");
            }
        }

        public void getPersonalBasicInfo()
        {
            ViewBag.User_ID = user_id;
            ViewBag.Current_User_ID = current_user_id;
            /*好友数量*/
            var db = new KUXIANGDBEntities();
            int FriendNum = db.USER_USER.Where(p => p.USER_ID1 == user_id).Count();
            ViewBag.FriendNum = FriendNum;

            /*好友信息*/
            ViewBag.Friend = pweb.getUserFriend(user_id);

            ViewBag.Image = "http://10.0.1.158:3000/Tongji/SE/" + user_id + "/img/head.jpg";
            /*ViewBag.NickName = ; 获得昵称*/

            /*显示所有信息*/
            var messageOp = new AboutMessage();
            ViewBag.MessageArr = messageOp.pushMessage(user_id);

        }

        public bool AcceptMessage(string messageId, string messageType, string userId, string repId, string fileId, bool agree)
        {
            var messageOp = new AboutMessage();
            var userOp = new AboutUser();
            var flag = 0;
            if (!agree)
            {
                flag = 1;
            }
            //设置为已读
            messageOp.readMessage(messageId);
            //处理操作(1的时候是请求权限处理，2的时候是文件上传处理)
            if (messageType == "1")
            {
                if (userOp.sureApplyManager(userId, repId, flag))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (messageType == "2")
                {
                    var error = new ErrorMessage();
                    if (userOp.verifyRequest(userId, repId, fileId, flag, out error))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public void CreateFork()
        {
            string repositoryName = Request["repositoryName"];
            string repositoryDescription = Request["repositoryDescription"];
            string isPublic = Request["isPublic"];
            string tag = Request["tag"];

            var ar = new AboutRepository();
            var authority = isPublic == "true" ? 1 : 0;
            if (ar.CreateRepository(user_id, repositoryName, authority, repositoryDescription, tag))
                Response.Redirect("/Home/Personal");
            else
                Response.Redirect("/Home/Index");
        }

    }
}