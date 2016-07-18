using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                /*根据用户的仓库获取用户所有的Course*/
                //获取所有库的用户—仓库—关系项
                var UserRepoRelaTuples = func.showUserRepertory(_UserId);
                //如果结果为空，则该用户没有库,返回错误信息
                if(null == UserRepoRelaTuples)
                {
                    ErrorInfo = new ItemModel.ErrorMessage();
                    ErrorInfo.ErrorOperation = "showUserRepertory";
                    ErrorInfo.ErrorReason = "User has no repository";
                    ErrorInfo.ErrorTime = DateTime.Now;
                    SearchResut = null;
                    return false;
                }
                /*提取该用户的所有库的三级Label*/
                List<string> LabelList = new List<string>();
                foreach (var item in UserRepoRelaTuples)
                {
                    LabelList.Add(//Add:begin
                        db.REPOSITORies.Where(p => p.REPOSITORY_ID == item.REPOSITORY_ID)
                        .Select(p =>p.COURSE.LABEL3).ToString()
                        );//Add:end
                }
                /*获取所有标签下除去用户的仓库之外其他所有的仓库*/
                //获取所有的仓库
                foreach(var label in LabelList)//对于每个label
                {
                    //获取该标签下所有仓库id（string）
                    var RepoLabelList = func.searchRepository(label, true);
                    //根据id找到仓库记录并检查是否是该用户的仓库，如果是的话，不放入结果列表
                    foreach(var repoid in RepoLabelList)//对于每个该label下的仓库
                    {
                        //根据仓库id获取仓库
                        REPOSITORY RepoItem = db.REPOSITORies.Where(p => repoid == p.REPOSITORY_ID).FirstOrDefault();

                        //检查该仓库是否属于_UserId
                        bool save = true;//是否保留该记录
                       foreach(var temp in UserRepoRelaTuples)
                        {//foreach:begin
                            if(RepoItem.REPOSITORY_ID == temp.REPOSITORY_ID) // 如果该记录属于
                            {
                                save = false;
                                break;
                            }
                        }//foreach:end
                        if (false == save)
                            continue;

                        //插入该仓库结果项到结果中
                        ItemModel.RepertorySearchResult ResultItem = new ItemModel.RepertorySearchResult();
                        ResultItem.RepertoryName = RepoItem.NAME;
                        ResultItem.RepertoryLabel1 = RepoItem.COURSE.LABEL1;
                        ResultItem.RepertoryLabel2 = RepoItem.COURSE.LABEL2;
                        ResultItem.RepertoryLabel3 = RepoItem.COURSE.LABEL3;
                        ResultItem.RepertoryFork = RepoItem.FORK_NUM.Value;
                        ResultItem.RepertoryInfo = RepoItem.DESCRIPTION;


                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
       
        /// <summary>
        /// 获取用户的好友的动态
        /// 输入：用户的id, 仓库id（null时为获取所有的仓库）， 要返回的仓库列表, 错误的信息
        /// 输出：是否成功
        /// 未测试
        /// </summary>
        public bool getFriendDynamic(string _UserId, string _RepoId, out List<ItemModel.actionInfo> SearchResul, out ItemModel.ErrorMessage ErrorInfo)
        {

        }

        /// <summary>
        /// 获取仓库被Star的次数
        /// 输入：仓库的Id， 返回的被Star次数， 错误信息
        /// 输出：是否成功
        /// </summary>
        public bool getStarNum(string _RepoId, out int StarNum, out ItemModel.ErrorMessage ErrorInfo)
        {

        }

        /// <summary>
        ///  获取仓库被Fork的次数
        ///  输入：仓库的Id， 返回的被Fork次数， 错误信息
        ///  输出：是否成功
        /// </summary>
        public bool getForkNum(string _RepoId, out int ForkNum, out ItemModel.ErrorMessage ErrorInfo)
        {

        }

    }
}