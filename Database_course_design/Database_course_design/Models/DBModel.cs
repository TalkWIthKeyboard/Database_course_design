using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Database_course_design.Models
{
    public class DBModel
    {
        /// <summary>
        /// 创建主码ID
        /// 输入：表名
        /// 输出：该表的主码ID
        /// 待测试
        /// </summary>
        public string createNewId(string tableName)
        {
            string newId = tableName + '_';
            string timeStr = DateTime.Now.Ticks.ToString();
            string time = timeStr.Substring(timeStr.Length - 10, 10);
            newId += time;
            return newId;
        }

        /// <summary>
        /// 添加新用户到数据库
        /// </summary>
        public bool addUserInfo(string UserAccount,string UserKey,string UserName,string UserDepartment,string UserEmail,short UserIdentity,short UserGrade)
        {
            string newId = createNewId("USERTABLE");

            USERTABLE newUser = new USERTABLE
            {
                USER_ID = newId,
                USER_NAME = UserName,
                PASSWORD = UserKey,
                DEPARTMENT = UserDepartment,
                EMAIL = UserEmail,
                IDENTITY = UserIdentity,
                GRADE = UserGrade
            };

            using (KUXIANGEntities db = new KUXIANGEntities())
            {
                try
                {
                    db.USERTABLEs.Add(newUser);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("用户添加异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return false;
        }
    }
}