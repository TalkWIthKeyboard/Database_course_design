using Database_course_design.Models;
using Database_course_design.Models.ItemModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Database_course_design.Controllers
{
    public class TestController : Controller
    {
        private IndexWebInterface iweb = new IndexWebInterface();
        private PersonalWebInterface pweb = new PersonalWebInterface();
        // string user_id;

        /*得到卡片的内容*/
        /* private ActionResult getCardContent()
         {
             List<RepertorySearchResult> SearchResut = null;
             ErrorMessage errorInfo = null;
             iweb.getRepositoryByLabel(user_id, out SearchResut, out errorInfo);
             ViewBag.SearchResut = SearchResut;


             ViewBag.errorInfo = errorInfo;
             retu
         }*/

        public ActionResult test()
        {
            //测试1
            /* Dictionary<string,IndexWebInterface.FileItem> ret = null;
             ErrorMessage errorInfo = null;
             iweb.getFIleByRepoId("REPOSITORY_8111840549", out ret, out errorInfo);
             ViewBag.He = ret;*/
            //向数据库注入数据
            /*DBModel fun = new DBModel();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            var sArray = new List<string>();
            StreamReader st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\data.in",Encoding.Default);
            while (st.Peek() != -1)
            {
                string str = st.ReadLine();
                var realStr = str.Split('\t');
                sArray.Add(realStr[0]);
            }


            //测试2
            /*ErrorMessage errorInfo = null;
            int? num = 0;
            iweb.getForkNum("REPOSITORY_1863516702", out num,out errorInfo);
            ViewBag.Num = num;*/

            //测试3
            /*List<actionInfo> SearchResult = null;
            ErrorMessage errorInfo = null;
            iweb.getFriendDynamic("1452706",out SearchResult,out errorInfo);
            ViewBag.SearchResult = SearchResult;*/

            //测试4
            /*List<IndexWebInterface.FileItem> ret = null;
             ErrorMessage errorInfo = null;
             iweb.getFIleByRepoId("REPOSITORY_8111840549", out ret, out errorInfo);

             ViewBag.He = ret;*/

            //测试4
            /*List<DayHeat> dayheat = null;
            ErrorMessage errorInfo = null;
            pweb.getUserHeat("1452716", out dayheat, out errorInfo);
            ViewBag.DayHeat = dayheat;*/

            var qiniuOp = new Models.ScriptModel.QiNiuOperation();
            qiniuOp.upload("c:/1.txt", "sw/1.txt");

            return View();
        }

        /*

            st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\course.in");
            while (st.Peek() != -1)
            {
                string str = st.ReadLine();
                var rand = new Random();
                int a = rand.Next(0, sArray.Count());
                try
                {
                   fun.CreateRepository(sArray[a], str + "仓库", 0, "这是一个仓库", str);
                }
                catch
                {

                } 
            }
        }*/

        public JsonResult GetPersonInfo()
        {
            List<DayHeat> dayheat = null;
            ErrorMessage error = null;
            pweb.getUserHeat("1452716", out dayheat, out error);
            // dayheat[0].Count;
            //    dayheat[0].OpList[0].TARGET_USER_NAME;

           /* var person = new
            {
                Count = dayheat[i].Count,
                Op = new {
                    Name = dayheat[i].OpList[j].TARGET_USER_NAME,
                    Operation = dayheat[i].OpList[0].OPERATION,
                    ReName = dayheat[19].OpList[0].TARGET_REPOSITORY_NAME
                },
             };*/
            JsonResult js = Json(dayheat);

            return js;
        }
    }
}