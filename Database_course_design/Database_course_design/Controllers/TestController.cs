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
    public class TestController :Controller
    {
        private IndexWebInterface iweb = new IndexWebInterface();
        string user_id;

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
            //向数据库注入数据
            DBModel fun = new DBModel();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            var sArray = new List<string>();
            StreamReader st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\data.in",Encoding.Default);
            while (st.Peek() != -1)
            {
                string str = st.ReadLine();
                var realStr = str.Split('\t');
                sArray.Add(realStr[0]);
            }

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

            /*//测试personal页面
            var person = new PersonalWebInterface();
            var fun = new KUXIANGDBEntities();
            var db = new DBModel();
            var resultArray = new List<actionInfo>();
            var error = new ErrorMessage();*/

            return View();
        }
    }
}