using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.ItemModel;
using Database_course_design.Models.WorkModel;

namespace Database_course_design.Models
{
    public class PersonalWebInterface
    {
        public struct PersonalRepoItem
        {
            public string RepoName;
            public string RepoAuthority;
            public string RepoDescription;
            public string RepoUpdateDate;
        }

        public struct CommentSearchResult
        {
            public string User_Name;
            public string Content;
            public string CommentDate;
        }

        /// <summary>
        /// 1.获取用户自己的动态
        /// 输入：用户的id, 仓库id（null时为获取所有的动态）， 要返回的动态列表, 错误的信息
        /// 输出：是否成功
        /// 完成测试
        /// </summary>
        public bool getSelfDynamic(string _UserId, string _RepoId, out List<ActionInfo> SearchResul, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                bool flag = false;
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                var userOp = new AboutUser();
                SearchResul = new List<ActionInfo>();
                if (_RepoId == null)
                {
                    var result = userOp.getUserDynamics(_UserId);
                    if (result != null)
                    {
                        flag = true;
                        SearchResul = result;
                    }
                }
                else
                {
                    var res = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == _UserId && p.REPOSITORY_ID == _RepoId);
                    foreach (var row in res)
                    {
                        var name = db.REPOSITORies.Where(p => p.REPOSITORY_ID == _RepoId).FirstOrDefault().NAME;
                        ActionInfo newAction = new ActionInfo()
                        {
                            UserId = _UserId,
                            UserName = row.USERTABLE.USER_NAME,
                            UserOperation = row.OPERATION,
                            UserPhotoUrl = row.USERTABLE.IMAGE,
                            RepositoryId = _RepoId,
                            UpdateInfo = row.DESCRIPTION,
                            UpdateTime = row.OPERATION_DATE.ToString(),
                            RepositoryName = name
                        };
                        SearchResul.Add(newAction);
                    }
                    flag = true;
                }
                if (flag == true)
                {
                    ErrorInfo = null;
                    return true;
                }
                else
                {
                    ErrorInfo = new ErrorMessage("获取用户自己的动态","获取动态函数失败");
                    return false;
                }
            }
            catch (Exception ex)
            {
                SearchResul = null;
                ErrorInfo = new ErrorMessage("获取用户自己的动态",ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 2.修改用户好友关系
        /// 输入：用户自己的Id, 对象用户的Id, 关注或取消关注（true是关注）, 错误信息
        /// 输出：是否成功
        /// 完成测试
        /// </summary>
        public bool changeFriend(string _SelfUserId, string _TarUserId, bool Focus, out ItemModel.ErrorMessage ErrorInfo)
        {
            var userOp = new AboutUser();
            try
            {
                bool flag = false;
                if (Focus == true)
                {
                    if (userOp.makeFriend(_SelfUserId, _TarUserId))
                        flag = true;
                }
                else
                {
                    if (userOp.deleteFriend(_SelfUserId, _TarUserId))
                        flag = true;
                }
                if (flag == true)
                {
                    ErrorInfo = null;
                    return true;
                }
                else
                {
                    ErrorInfo = new ErrorMessage("修改用户好友关系","添加或者删除好友错误");
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorInfo = new ErrorMessage("修改用户好友关系",ex.Message);
                return false;
            }

        }

        /// <summary>
        /// 3.查询用户该月每天的操作和热度
        /// 输入：用户Id, 返回列表, 错误信息
        /// 输出：是否成功
        /// 待修改
        /// 待测试
        /// </summary>
        public bool getUserHeat(string _UserId, out List<ItemModel.DayHeat> DayHeatResult, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                //实例化数据库
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                var rep = new AboutRepository();
                //拿到该用户本月所有的操作
                var OpList = rep.showOperationHistory(_UserId);
                OpList.Where(p => p.DATE > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                //创建返回列表
                List<Database_course_design.Models.ItemModel.DayHeat> ret = new List<ItemModel.DayHeat>();
                //插入本月的天数

                for (int i = 0; i <= DateTime.Now.Day; i++)
                {
                    ret.Add(new ItemModel.DayHeat() { Count = 0 });
                }
                //遍历用户本月所有的操作
                foreach (var op in OpList)
                {
                    //给对应日期添加操作列表项
                    ret[op.DATE.Day].OpList.Add(//Add_Beg
                        new ItemModel.HeatOpItem
                        {//new_Beg
                         //定义操作
                            OPERATION = op.OPERATION,
                            //定义仓库名
                            TARGET_REPOSITORY_NAME = op.REPOSITORY_NAME,
                            //查询拥有被操作的仓库的用户的名字
                            TARGET_USER_NAME = (from r in db.USER_REPOSITORY_RELATIONSHIP
                                                join s in db.USERTABLEs on r.USER_ID equals s.USER_ID
                                                where op.REPOSITORY_ID == r.REPOSITORY_ID

                                                select s.USER_NAME).FirstOrDefault(),
                        }//new_End
                   );//Add_End

                    //对应日期的次数统计加一
                    ret[op.DATE.Day].Count++;
                }
                //返回列表
                ErrorInfo = null;
                DayHeatResult = ret;
                return true;
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "getUserHeat";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = DateTime.Now;
                DayHeatResult = null;
                return false;
            }
        }

        /// <summary>
        /// 4.修改用户资料（对DBmodel的封装）
        /// 输入：待定参数， 错误信息
        /// 输出：是否成功
        /// 待测试
        /// </summary>
        public bool modifyBasicUserInfo(string _UserId, string _ImgUrl, string _NickName, string _Signature, string _Email, string _PersonalUrl/*这个参数什么意思*/,
                                                            string _Address, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                ErrorInfo = null;
                //修改表项信息
                var userOp = new AboutUser();
                if (userOp.changeUserInfo(_UserId, _Email, _ImgUrl, _Signature, _NickName, _PersonalUrl, _Address))
                {
                    throw new Exception("Modify error!");
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "changeBasicUserInfo";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = DateTime.Now;
                return false;
            }
        }

        /// <summary>
        /// 5.修改密码
        /// 输入：用户ID， 旧密码，新密码
        /// 输出：是否修改成功
        /// 待测试
        /// </summary>
        public bool modifyPassword(string _UserId, string _OldPassword, string _NewPassword, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                //实例化数据库
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                //提取旧密码
                string OldPassword = db.USERTABLEs.Where(p => p.USER_ID == _UserId).Select(P => P.PASSWORD).FirstOrDefault();
                //旧密码正确
                if (0 == string.Compare(_OldPassword, OldPassword))
                {
                    //修改密码
                    var User = from p in db.USERTABLEs
                               where p.USER_ID == _UserId
                               select p;
                    User.FirstOrDefault().PASSWORD = _NewPassword;
                    db.SaveChanges();
                    ErrorInfo = null;
                    return true;
                }
                //密码不正确，抛出异常
                else
                {
                    throw new Exception("Password Wrong!");
                }
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage("modifyPassword",ex.Message);       
                return false;
            }
        }

        /// <summary>
        /// 6.获取权限
        /// 输入：仓库ID
        /// 输出：1 - 官方， 2 - 原创， 3 - 贡献
        /// 待测试
        /// </summary>
        public int getAuthority(string _RepositoryId)
        {
            try
            {
                //查到数据库
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                var ret = db.REPOSITORies.Where(p => p.REPOSITORY_ID == _RepositoryId);
                if (null == ret)
                {
                    throw new Exception("仓库不存在");
                }
                //判断返回类型
                if (1 == ret.Select(p => p.AUTHORITY).FirstOrDefault())//是官方库
                {
                    return 1;
                }
                else if (1 == ret.Select(p => p.IS_CREATE).FirstOrDefault())
                {
                    return 2;
                }
                else
                {
                    return 3;
                }

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        /// <summary>
        /// 7.根据仓库Id，获取仓库描述
        /// 输入：仓库ID
        /// 输出：仓库描述
        /// 测试成功
        /// </summary>
        public string getRepositoryDes(string _RepositoryId)
        {
            try
            {
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                return db.REPOSITORies.Where(p => p.REPOSITORY_ID == _RepositoryId).Select(p => p.DESCRIPTION).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 8.检索这个用户的所有仓库，并重新封装
        /// 输入：用户id
        /// 输出：重新封装后的仓库类
        /// 测试成功
        /// </summary>
        public List<PersonalRepoItem> getPrivate(string _UserId)
        {
            try
            {
                var userOp = new AboutUser();
                List<PersonalRepoItem> ret = new List<PersonalRepoItem>();
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                var RepoList = userOp.showOthersRepertory(_UserId,4);
                if (null == RepoList)
                {
                    throw new Exception();
                }

                foreach (var item in RepoList)
                {
                    var RetItem = new PersonalRepoItem();
                    int Aut = getAuthority(item.RepertoryID);
                    RetItem.RepoName = db.REPOSITORies.Where(p => p.REPOSITORY_ID == item.RepertoryID).Select(p => p.NAME).FirstOrDefault();
                    RetItem.RepoDescription = getRepositoryDes(item.RepertoryID);
                    if (1 == Aut)
                    {
                        RetItem.RepoAuthority = "官方";
                    }
                    else if (2 == Aut)
                    {
                        RetItem.RepoAuthority = "原创";
                    }
                    else if (3 == Aut)
                    {
                        RetItem.RepoAuthority = "贡献";
                    }
                    else
                    {
                        RetItem.RepoAuthority = "";
                    }
                    ret.Add(RetItem);
                }
                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 9.展示留言板
        /// 输入：主页用户的id
        /// 输出：留言数组
        /// 测试成功
        /// </summary>
        public List<CommentSearchResult> getAllComment(string _UserId)
        {
            try
            {
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                //发表评论的人的ID列表
                var CommenterIDList = db.USER_COMMENT_USER.Where(p => p.USER_ID == _UserId);
                if (null == CommenterIDList)
                {
                    throw new Exception();
                }
                List<CommentSearchResult> ret = new List<CommentSearchResult>();
                //对于每个发表评论的人，提取姓名和内容
                foreach (var item in CommenterIDList)
                {
                    CommentSearchResult csr = new CommentSearchResult();
                    csr.User_Name = db.USERTABLEs.Where(p => p.USER_ID == item.ANNOUNCER).Select(p => p.USER_NAME).FirstOrDefault();
                    csr.Content = db.COMMENTTABLEs.Where(p => p.COMMENT_ID == item.COMMENT_ID).Select(p => p.CONTENT).FirstOrDefault();
                    csr.CommentDate = db.COMMENTTABLEs.Where(p => p.COMMENT_ID == item.COMMENT_ID).FirstOrDefault().COMMENT_DATE.ToString();
                    ret.Add(csr);
                }
                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 10.通过仓库名获取仓库id
        /// (可能不会使用)
        /// </summary>
        public string getIDByRepo(string _RepoName, string _UserID)
        {
            try
            {
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                var userOp = new AboutUser();
                //获取该用户所有仓库ID
                var RepoList = userOp.showOthersRepertory(_UserID,4);
                if (null == RepoList)
                {
                    throw new Exception("No Repository");
                }
                //对于每个仓库
                foreach (var item in RepoList)
                {
                    //提取仓库名
                    string TempRepoName = db.REPOSITORies.Where(p => p.REPOSITORY_ID == item.RepertoryID).Select(p => p.NAME).FirstOrDefault();
                    //对比，返回仓库ID
                    if (0 == string.Compare(_RepoName, TempRepoName))
                    {
                        return item.RepertoryID;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 11.检查是否是朋友关系
        /// 输入：用户id，被关注者id
        /// 输出：是否是朋友关系
        /// 测试成功
        /// </summary>
        public bool FellowOrNot(string _UserAId, string _UserBId)
        {
            try
            {
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                var Item = db.USER_USER.Where(p => p.USER_ID1 == _UserAId && p.USER_ID2 == _UserBId).FirstOrDefault();
                return null == Item ? false : true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 12.获取这个人的所有好友信息
        /// 输入：用户id
        /// 输出：这个人的所有好友信息
        /// 待测试
        /// </summary>
        public List<UserSearchResult> getUserFriend(string userId)
        {
            var results = new List<UserSearchResult>();
            var db = new KUXIANGDBEntities();
            try
            {
                var friend = db.USER_USER.Where(p => p.USER_ID1 == userId).ToList();
                foreach (var each in friend)
                {
                    var user = db.USERTABLEs.Where(p => p.USER_ID == each.USER_ID2).FirstOrDefault();
                    var result = new UserSearchResult(user);
                    results.Add(result);
                }
                return results;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("显示好友列表操作");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}