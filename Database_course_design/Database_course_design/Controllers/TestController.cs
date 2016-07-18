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
            return View();
        }

    }
}