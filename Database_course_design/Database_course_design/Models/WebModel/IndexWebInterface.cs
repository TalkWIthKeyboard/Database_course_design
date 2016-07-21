using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.ItemModel;
using Database_course_design.Models.WorkModel;

namespace Database_course_design.Models
{
    public class IndexWebInterface
    {
        public struct IndexRepoItem
        {
            public string RepositoryID;
            public int StarNum;
            public int ForkNum;
            public List<FileInfo> FileList;
        }

        /// <summary>
        /// 1.按照课程推荐仓库（包含仓库下的文件）
        /// 输入：用户id
        /// 输出：是否操作成功，仓库、文件列表键值对，错误信息
        /// 测试成功
        /// </summary>
        public bool RecommendByCourse(string _UserID, out Dictionary<string, IndexRepoItem> ret, out ErrorMessage ErrorRet)
        {
            try
            {
                //初始化返回值
                ret = new Dictionary<string, IndexRepoItem>();
                //获取所有要推荐的仓库列表
                List<RepertorySearchResult> RepoList;
                ErrorMessage ErrorInfo;
                getRepositoryByLabel(_UserID, out RepoList, out ErrorInfo);
                if (null != ErrorInfo)
                {
                    throw new Exception(ErrorInfo.ErrorReason);
                }
                foreach (var RepoItem in RepoList)
                {
                    IndexRepoItem ValueItem;
                    ErrorMessage ErrorInfo_2 = null;
                    //拿到仓库下所有文件
                    getFIleByRepoId(RepoItem.RepertoryID, out ValueItem.FileList, out ErrorInfo_2);
                    ValueItem.RepositoryID = RepoItem.RepertoryID;
                    ValueItem.StarNum = RepoItem.RepertoryStar;
                    ValueItem.ForkNum = RepoItem.RepertoryFork;
                    if (false == ret.ContainsKey(RepoItem.RepertoryName))
                        ret.Add(RepoItem.RepertoryName, ValueItem);
                }
                ErrorRet = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorRet = new ItemModel.ErrorMessage("RecommendByCourse",ex.Message);
                ret = null;
                return false;
            }
        }

        /// <summary>
        /// 2.获取一个文件夹下的所有文件
        /// 输入：仓库id
        /// 输出：是否操作成功，文件数组，错误信息
        /// 待测试
        /// </summary>
        public bool getFIleByRepoId(string _RepoId, out List<FileInfo> ret, out ErrorMessage ErrorInfo)
        {
            var fileOp = new AboutRepository();
            try
            {
                ret = fileOp.showFile(_RepoId);
                ErrorInfo = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage("getFIleByRepoId",ex.Message);
                ret = null;
                return false;
            }
        }

        /// <summary>
        ///  3.根据用户的所有Label获取推荐的仓库
        ///  输入：用户的Id， 返回的参数， 错误信息
        ///  输出：是否成功
        ///  待测试
        /// </summary>
        public bool getRepositoryByLabel(string _UserId, out List<RepertorySearchResult> SearchResult, out ErrorMessage ErrorInfo)
        {
            try
            {
                var searchOp = new AboutSearch();
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                SearchResult = new List<ItemModel.RepertorySearchResult>();
                ErrorInfo = null;
                /*根据用户的仓库获取用户所有的Course*/
                List<string> LabelList = db.TAKES.Where(p => p.USER_ID == _UserId).Select(p => p.COURSE.LABEL3).ToList();
                if (null == LabelList)
                {
                    SearchResult = null;
                    ErrorInfo = new ItemModel.ErrorMessage("getRepositoryByLabel", "用户没有参加课程");
                    return false;
                }
                /*获取该用户所有的仓库，用于下面从结果序列中排除*/
                var UserRepoRelaTuples = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == _UserId).Select(p => p.REPOSITORY_ID);
                /*获取所有标签下除去用户的仓库之外其他所有的仓库*/
                //获取所有的仓库
                foreach (var label in LabelList)//对于每个label
                {
                    //获取该标签下所有仓库id（string）
                    var RepoLabelList = searchOp.searchRepository(label, true);
                    //根据id找到仓库记录并检查是否是该用户的仓库，如果是的话，不放入结果列表
                    foreach (var rep in RepoLabelList)//对于每个该label下的仓库
                    {
                        //根据仓库id获取仓库
                        REPOSITORY RepoItem = db.REPOSITORies.Where(p => rep.RepertoryID == p.REPOSITORY_ID).FirstOrDefault();

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
                        SearchResult.Add(ResultItem);
                    }//foreach(var repoid in RepoLabelList): end
                }
                SearchResult.Sort(
                    delegate (ItemModel.RepertorySearchResult x, ItemModel.RepertorySearchResult y)
                    {
                        return (x.RepertoryStar + x.RepertoryFork) > (y.RepertoryStar + y.RepertoryFork) ? 1 : 0;
                    }
                    );
                return true;

            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage(" getRepositoryByLabel",ex.Message);
                SearchResult = null;
                return false;
            }
        }

        /// <summary>
        /// 4.官方库按热度推荐
        /// 输入：用户的id
        /// 输出：仓库输出列表
        /// 测试成功
        /// </summary>
        public bool getRepertoryByAttribute(string _UserId, out Dictionary<string, IndexRepoItem> ret, out ErrorMessage ErrorInfo)
        {
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            var searchOp = new AboutSearch();
            ret = new Dictionary<string, IndexRepoItem>();
            try
            {
                var result = searchOp.recommendRepositoryByAttribute(_UserId);
                if (result != null)
                {
                    foreach (var each in result)
                    {
                        IndexRepoItem ValueItem;
                        ErrorMessage ErrorInfo_2 = null;
                        //拿到仓库下所有文件
                        getFIleByRepoId(each.REPOSITORY_ID, out ValueItem.FileList, out ErrorInfo_2);
                        ValueItem.RepositoryID = each.REPOSITORY_ID;
                        ValueItem.StarNum = (int)each.STAR_NUM;
                        ValueItem.ForkNum = (int)each.FORK_NUM;
                        if (false == ret.ContainsKey(each.NAME))
                            ret.Add(each.NAME, ValueItem);
                    }
                    ErrorInfo = null;
                    return true;
                }
                else
                {
                    var er = new ItemModel.ErrorMessage("官方库按热度推荐异常","数据库检索不到信息");
                    ErrorInfo = er;
                    ret = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                var er = new ItemModel.ErrorMessage("官方库按热度推荐异常", ex.Message);
                ErrorInfo = er;
                ret = null;
                return false;
            }
        }

        /// <summary>
        /// 5.获取用户的好友的动态
        /// 输入：用户的id, 要返回的好友动态列表, 错误的信息
        /// 输出：是否成功
        /// 测试成功
        /// </summary>
        public bool getFriendDynamic(string _UserId, out List<ActionInfo> SearchResult, out ErrorMessage ErrorInfo)
        {
            var userOp = new AboutUser();
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            try
            {
                SearchResult = userOp.showFriendDynamics(_UserId);
                if (SearchResult != null)
                {
                    ErrorInfo = null;
                    return true;
                }
                else
                {
                    var er = new ItemModel.ErrorMessage("获取好友动态时异常","搜索不到所要求的值");
                    ErrorInfo = er;
                    return false;
                }
            }
            catch (Exception ex)
            {
                var er = new ItemModel.ErrorMessage("获取好友动态时异常",ex.Message);
                ErrorInfo = er;
                SearchResult = null;
                return false;
            }
        }

        /// <summary>
        /// 6.获取仓库被Star的次数
        /// 输入：仓库的Id， 返回的被Star次数， 错误信息
        /// 输出：是否成功
        /// 测试成功
        /// </summary>
        public bool getStarNum(string _RepoId, out int? StarNum, out ErrorMessage ErrorInfo)
        {
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
                var er = new ItemModel.ErrorMessage("返回fork次数时异常",ex.Message);
                ErrorInfo = er;
                StarNum = -1;
                return false;
            }
        }

        /// <summary>
        ///  7.获取仓库被Fork的次数
        ///  输入：仓库的Id， 返回的被Fork次数， 错误信息
        ///  输出：是否成功
        ///  测试成功
        /// </summary>
        public bool getForkNum(string _RepoId, out int? ForkNum, out ErrorMessage ErrorInfo)
        {
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
                var er = new ItemModel.ErrorMessage("返回fork次数时异常",ex.Message);
                ErrorInfo = er;
                ForkNum = -1;
                return false;
            }
        }
    }
}