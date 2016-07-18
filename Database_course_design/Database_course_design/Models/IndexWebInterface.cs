using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models;

namespace Database_course_design.Models
{
    public class IndexWebInterface
    {
        /// <summary>
        ///  根据用户的所有Label获取推荐的仓库
        ///  输入：用户的Id， 返回的参数， 错误信息
        ///  输出：是否成功
        /// </summary>
        public bool getRepositoryByLabel(string _UserId, out List<ItemModel.RepertorySearchResult> SearchResut, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                DBModel func = new DBModel();
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                SearchResut = new List<ItemModel.RepertorySearchResult>();
                ErrorInfo = null;
                /*根据用户的仓库获取用户所有的Course*/
                List<string> LabelList = new List<string>();
                LabelList = db.TAKES.Where(p => p.USER_ID == _UserId).Select(p => p.COURSE.LABEL3).ToList();
                /*获取该用户所有的仓库，用于下面从结果序列中排除*/
                var UserRepoRelaTuples = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == _UserId).Select(p => p.REPOSITORY_ID);

                /*获取所有标签下除去用户的仓库之外其他所有的仓库*/
                //获取所有的仓库
                foreach (var label in LabelList)//对于每个label
                {
                    //获取该标签下所有仓库id（string）
                    var RepoLabelList = func.searchRepository(label, true);
                    //根据id找到仓库记录并检查是否是该用户的仓库，如果是的话，不放入结果列表
                    foreach (var repoid in RepoLabelList)//对于每个该label下的仓库
                    {
                        //根据仓库id获取仓库
                        REPOSITORY RepoItem = db.REPOSITORies.Where(p => repoid == p.REPOSITORY_ID).FirstOrDefault();

                        //检查该仓库是否属于_UserId
                        bool save = true;//是否保留该记录
                        foreach (var temp in UserRepoRelaTuples)//UserRepoRelaTuples在函数前部获取
                        {//foreach:begin
                            if (RepoItem.REPOSITORY_ID == temp) // 如果该记录属于该用户就不插入到结果序列
                            {
                                save = false;
                                break;
                            }
                        }//foreach:end
                        if (false == save)
                            continue;
                        //插入该仓库结果项到结果中
                        ItemModel.RepertorySearchResult ResultItem = new ItemModel.RepertorySearchResult(RepoItem);
                        SearchResut.Add(ResultItem);
                    }//foreach(var repoid in RepoLabelList): end
                }
                SearchResut.Sort(
                    delegate (ItemModel.RepertorySearchResult x, ItemModel.RepertorySearchResult y)
                    {
                        return (x.RepertoryStar + x.RepertoryFork) > (y.RepertoryStar + y.RepertoryFork) ? 1 : 0;
                    }
                    );
                return true;

            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = " getRepositoryByLabel";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = DateTime.Now;
                SearchResut = null;
                return false;
            }
        }

        /// <summary>
        /// 获取用户的好友的动态
        /// 输入：用户的id, 要返回的好友动态列表, 错误的信息
        /// 输出：是否成功
        /// 未测试
        /// </summary>
        public bool getFriendDynamic(string _UserId, string _RepoId, out List<ItemModel.actionInfo> SearchResul, out ItemModel.ErrorMessage ErrorInfo)
        {
            DBModel func = new DBModel();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            try
            {
                SearchResul = func.showFriendDynamics(_UserId);
                ErrorInfo = null;
                return true;
            }
            catch (Exception ex)
            {
                var er = new ItemModel.ErrorMessage();
                er.ErrorOperation = "获取好友动态时异常";
                er.ErrorReason = ex.Message;
                er.ErrorTime = DateTime.Now;
                ErrorInfo = er;
                SearchResul = null;
                return false;
            }
        }

        /// <summary>
        /// 获取仓库被Star的次数
        /// 输入：仓库的Id， 返回的被Star次数， 错误信息
        /// 输出：是否成功
        /// </summary>
        public bool getStarNum(string _RepoId, out int? StarNum, out ItemModel.ErrorMessage ErrorInfo)
        {
            DBModel func = new DBModel();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            try
            {
                var repo = db.REPOSITORies.Where(p => p.REPOSITORY_ID == _RepoId).FirstOrDefault();
                StarNum = repo.STAR_NUM;
                ErrorInfo = null;
                return true;
            }
            catch (Exception ex)
            {
                var er = new ItemModel.ErrorMessage();
                er.ErrorOperation = "返回fork次数时异常";
                er.ErrorReason = ex.Message;
                er.ErrorTime = DateTime.Now;
                ErrorInfo = er;
                StarNum = -1;
                return false;
            }
        }

        /// <summary>
        ///  获取仓库被Fork的次数
        ///  输入：仓库的Id， 返回的被Fork次数， 错误信息
        ///  输出：是否成功
        /// </summary>
        public bool getForkNum(string _RepoId, out int? ForkNum, out ItemModel.ErrorMessage ErrorInfo)
        {
            DBModel func = new DBModel();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            try
            {
                var repo = db.REPOSITORies.Where(p => p.REPOSITORY_ID == _RepoId).FirstOrDefault();
                ForkNum = repo.FORK_NUM;
                ErrorInfo = null;
                return true;
            }
            catch (Exception ex)
            {
                var er = new ItemModel.ErrorMessage();
                er.ErrorOperation = "返回fork次数时异常";
                er.ErrorReason = ex.Message;
                er.ErrorTime = DateTime.Now;
                ErrorInfo = er;
                ForkNum = -1;
                return false;
            }
        }
    }
}