using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.WorkModel;
using System.IO;
using System.Text;

namespace Database_course_design.Models.ScriptModel
{
    public class addDataToDatabase
    {
        KUXIANGDBEntities db = new KUXIANGDBEntities();

        /// <summary>
        /// 1.为仓库表注入数据（同时注入用户仓库操作表、用户仓库关系表[创建]）
        /// 输入：无
        /// 输出：无
        /// 测试成功
        /// </summary>
        public void addDataToRepository()
        {
            var repOp = new AboutRepository();
            var sArray = new List<string>();
            StreamReader st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\data.in", Encoding.Default);
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
                    repOp.CreateRepository(sArray[a], str + "仓库", 0, "这是一个仓库", str);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 2.为评论用户表注入数据
        /// 输入：无
        /// 输出：无
        /// 测试成功
        /// </summary>
        public void addDataToUserComment()
        {
            var commentOp = new AboutComment();
            var db = new KUXIANGDBEntities();
            var sArray = new List<string>();
            StreamReader st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\data.in", Encoding.Default);
            while (st.Peek() != -1)
            {
                string str = st.ReadLine();
                var realStr = str.Split('\t');
                sArray.Add(realStr[0]);
            }
            int i = 0;
            while (i < 100)
            {
                var rand = new Random();
                int a = rand.Next(0, sArray.Count());
                int b = rand.Next(0, sArray.Count());
                if (a != b)
                {
                    try
                    {
                        var la = sArray[a];
                        var userA = db.USERTABLEs.Where(p => p.USER_ID == la).FirstOrDefault().USER_NAME;
                        var lb = sArray[b];
                        var userB = db.USERTABLEs.Where(p => p.USER_ID == lb).FirstOrDefault().USER_NAME;
                        commentOp.addCommentToUser(userA + "喜欢" + userB, sArray[a], sArray[b]);
                        i++;
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 3.为评论仓库表注入数据
        /// 输入：无
        /// 输出：无
        /// 测试成功
        /// </summary>
        public void addDataToRepositoryComment()
        {
            var commentOp = new AboutComment();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            var sArray = new List<string>();
            StreamReader st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\data.in", Encoding.Default);
            while (st.Peek() != -1)
            {
                string str = st.ReadLine();
                var realStr = str.Split('\t');
                sArray.Add(realStr[0]);
            }

            st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\course.in");
            while (st.Peek() != -1)
            {
                string str = st.ReadLine() + "仓库";
                try
                {
                    var rep = db.REPOSITORies.Where(p => p.NAME == str).FirstOrDefault();
                    var rand = new Random();
                    int a = rand.Next(0, sArray.Count());
                    string la = sArray[a];
                    var username = db.USERTABLEs.Where(p => p.USER_ID == la).FirstOrDefault().USER_NAME;
                    string lb = username + "很喜欢" + rep.NAME + ",并很想和你搅基！";
                    commentOp.addCommentToRepository(lb, rep.REPOSITORY_ID, la);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 4.为用户仓库表注入数据
        /// 输入：无
        /// 输出：无
        /// 测试成功
        /// </summary>
        public void addDataToUser()
        {
            var userOp = new AboutUser();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            StreamReader st = new StreamReader(@"C:\code\Database\Database_course_design\Database_course_design\Controllers\data.in", Encoding.Default);
            while (st.Peek() != -1)
            {
                string str = st.ReadLine();
                var realStr = str.Split('\t');
                try
                {
                    userOp.addUserInfo(realStr[0], "123456", realStr[1], "软件学院", "", 0, 0);
                }
                catch
                {

                }
            }
        }
    }
}