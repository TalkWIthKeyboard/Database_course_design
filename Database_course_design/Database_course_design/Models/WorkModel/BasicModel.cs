using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Database_course_design.Models.ItemModel;

namespace Database_course_design.Models
{
    public class BasicModel
    {
        /// <summary>
        /// 1.创建主码ID
        /// 输入：表名
        /// 输出：该表的主码ID
        /// 测试成功
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
        /// 2.检查权限
        /// 输入：用户id，仓库id
        /// 输出：该用户对仓库的权限
        /// 待测试
        /// </summary>
        public int? auditAuthority(string userId, string repId)
        {
            var db = new KUXIANGDBEntities();
            int? flag = -1;
            try
            {
                var result = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == userId
                                                                     && p.REPOSITORY_ID == repId).ToList();
                if (result.Count() == 0)
                {
                    return flag;
                }

                flag = 2;
                foreach (var each in result)
                {
                    if (each.RELATIONSHIP != 2)
                    {
                        flag = each.RELATIONSHIP;
                        break;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("查找数据库中用户仓库关系表异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return flag;
            }
        }

        /// <summary>
        /// 3.记录操作历史
        /// 输入：用户的编号，仓库编号，操作
        /// 输出： 布尔值，true为成功，false为失败
        /// 测试成功
        /// </summary>
        public bool recordOperation(string userId, string repositoryID, string operation, string description)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var name = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryID).FirstOrDefault().NAME;
                    USER_REPOSITORY_OPERATION rep_operation = new USER_REPOSITORY_OPERATION
                    {
                        USER_ID = userId,
                        REPOSITORY_ID = repositoryID,
                        OPERATION_DATE = DateTime.Now,
                        OPERATION = operation,
                        DESCRIPTION = description,
                        REPOSITORY_NAME = name
                    };
                    db.USER_REPOSITORY_OPERATION.Add(rep_operation);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("记录操作历史异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}