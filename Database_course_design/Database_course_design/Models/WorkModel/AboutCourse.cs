using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.WorkModel
{
    public class AboutCourse
    {
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.创建课程
        /// 输入：三级label（1级学校、2级院系、3级课程）
        /// 输出：是否插入成功
        /// 测试成功
        /// </summary>
        public bool createCourse(string label1, string label2, string label3)
        {
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            var newCourse = new COURSE()
            {
                COURSE_ID = basic.createNewId("COURSE"),
                LABEL1 = label1,
                LABEL2 = label2,
                LABEL3 = label3
            };
            try
            {
                db.COURSEs.Add(newCourse);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("创建课程异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

        }
    }
}