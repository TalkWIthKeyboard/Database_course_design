using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.ItemModel;

namespace Database_course_design.Models.WorkModel
{
	public class AboutSearch
	{
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.仓库多种方式检索
        /// 输入：检索仓库的关键字（标签，仓库名），标记变量d(d为true：标签检索，d为false：关键字检索）
        /// 输出：检索出的仓库ID列表
        /// 测试成功
        /// </summary>
        public List<RepertorySearchResult> searchRepository(string keyWord, bool flag)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var newRepositoryList = new List<REPOSITORY>();
                    if (flag)
                    {
                        var newRepository = from p in db.REPOSITORies
                                            join c in db.COURSEs
                                            on p.COURSE_ID equals c.COURSE_ID
                                            where c.LABEL1.Contains(keyWord) || c.LABEL2.Contains(keyWord) || c.LABEL3.Contains(keyWord)
                                            orderby p.STAR_NUM descending
                                            select p;
                        newRepositoryList = newRepository.ToList();
                    }
                    else
                    {
                        var newRepository = from p in db.REPOSITORies
                                            join c in db.COURSEs
                                            on p.COURSE_ID equals c.COURSE_ID
                                            where  p.NAME.Contains(keyWord)
                                            orderby p.STAR_NUM descending
                                            select p;
                        newRepositoryList = newRepository.ToList();
                    }

                    var results = new List<RepertorySearchResult>();
                    foreach (var each in newRepositoryList)
                    {
                        var result = new RepertorySearchResult(each);
                        results.Add(result);
                    }
                    return results;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("检索仓库操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 2.文件检索
        /// 输入：要检索的文件的名称，父节点id，是在仓库下还是文件下(0是文件夹，1是仓库)
        /// 输出：检索出的文件ID列表
        /// 测试成功
        /// </summary>
        public List<FILETABLE> searchFile(string FileName, string fatherId, int flag)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    if (flag == 1)
                    {
                        var newfile = from p in db.FILETABLEs
                                      join c in db.REPOSITORY_FILE
                                      on p.FILE_ID equals c.FILE_ID
                                      where c.REPOSITORY_ID == fatherId
                                      where p.FILE_NAME.Contains(FileName)
                                      select p;
                        return newfile.ToList();
                    }
                    else
                    {
                        var newfile = from p in db.FILETABLEs
                                      join c in db.FILE_FILE
                                      on p.FILE_ID equals c.FILE_ID2
                                      where c.FILE_ID1 == fatherId
                                      where p.FILE_NAME.Contains(FileName)
                                      select p;
                        return newfile.ToList();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("查找文件操作异常 ");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 3. 用户检索
        /// 输入：要检索的用户的名称
        /// 输出：检索出的用户ID列表
        /// 测试成功
        /// </summary>
        public List<USERTABLE> searchUser(string UserName)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var newuser = from user in db.USERTABLEs
                                  where user.USER_NAME.Contains(UserName)
                                  select user;
                    return newuser.ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("查找用户操作异常 ");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 4.官方库推荐
        /// 输入：用户id
        /// 输出：检索出的仓库列表
        /// 测试成功
        /// </summary>
        public List<REPOSITORY> recommendRepositoryByAttribute(string userId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var userDepartment = db.USERTABLEs.Where(p => p.USER_ID == userId).FirstOrDefault().DEPARTMENT;
                    List<REPOSITORY> heatrepository = new List<REPOSITORY>();
                    var temps =
                        (from s in db.REPOSITORies
                         where s.ATTRIBUTE == 1
                         orderby s.STAR_NUM descending, s.FORK_FROM descending
                         select s);
                    foreach (var temp in temps)
                    {
                        if (userDepartment == temp.COURSE.LABEL2)
                        {
                            heatrepository.Add(temp);
                        }
                    }
                    return heatrepository;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("按官方库推荐仓库读取异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }
    }
}