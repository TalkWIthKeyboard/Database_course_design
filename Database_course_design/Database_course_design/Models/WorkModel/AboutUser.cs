﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.ItemModel;

namespace Database_course_design.Models.WorkModel
{
    public class AboutUser
    {
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.添加新用户到数据库
        /// 输入：用户账号，用户密码，用户名，所在院系，邮箱地址，用户身份，用户积分
        /// 输出：是否创建成功
        /// 测试成功
        /// </summary>
        public bool addUserInfo(string UserAccount, string UserKey, string UserName, string UserDepartment, string UserEmail, short UserIdentity, short UserGrade)
        {
            USERTABLE newUser = new USERTABLE
            {
                USER_ID = UserAccount,
                USER_NAME = UserName,
                PASSWORD = UserKey,
                DEPARTMENT = UserDepartment,
                EMAIL = UserEmail,
                IDENTITY = UserIdentity,
                GRADE = UserGrade,
                UNIVERSITY = "同济大学"
            };

            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
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
                    return false;
                }
            }
        }

        /// <summary>
        /// 2.修改用户信息（只能修改用户名，邮箱）
        /// 输入：用户id，用户名，用户邮箱
        /// 输出：是否修改成功
        /// 测试成功
        /// </summary>
        public bool changeUserInfo(string UserId, string UserEmail, string UserImage, string UserSignature, string UserNickname, string UserSelfUrl, string UserAddress)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    USERTABLE oldUserInfo = db.USERTABLEs.Where(p => p.USER_ID == UserId).FirstOrDefault();

                    if (UserEmail != "")
                    {
                        oldUserInfo.EMAIL = UserEmail;
                    }
                    if (UserImage != "")
                    {
                        oldUserInfo.IMAGE = UserImage;
                    }
                    if (UserSignature != "")
                    {
                        oldUserInfo.SIGNATURE = UserSignature;
                    }
                    if (UserNickname != "")
                    {
                        oldUserInfo.NICKNAME = UserNickname;
                    }
                    if (UserSelfUrl != "")
                    {
                        oldUserInfo.SELFURL = UserSelfUrl;
                    }
                    if (UserAddress != "")
                    {
                        oldUserInfo.ADDRESS = UserAddress;
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("用户修改异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 3.改变用户的积分
        /// 输入：用户id,修改的分数值（+为正，-为负）
        /// 输出：修改后的分数(失败后返回-1)
        /// 测试成功
        /// </summary>
        public short changeUserGrade(string UserId, short changeGrade)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    USERTABLE oldUser = db.USERTABLEs.Where(p => p.USER_ID == UserId).FirstOrDefault();
                    oldUser.GRADE += changeGrade;
                    if (oldUser.GRADE < 0)
                    {
                        oldUser.GRADE = 0;
                    }
                    db.SaveChanges();
                    return (short)oldUser.GRADE;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("用户分数修改异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return -1;
                }
            }
        }

        /// <summary>
        /// 4.关注好友
        /// 输入：用户的id，朋友的id
        /// 输出：操作是否成功
        /// 测试成功
        /// </summary>
        public bool makeFriend(string UserId, string FriendId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    USER_USER friend = new USER_USER
                    {
                        USER_ID1 = UserId,
                        USER_ID2 = FriendId
                    };

                    db.USER_USER.Add(friend);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("关注操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 5.删除好友
        /// 输入：用户的id，朋友的id
        /// 输出：操作是否成功
        /// 测试成功
        /// </summary>
        public bool deleteFriend(string UserId, string FriendId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    db.USER_USER.Remove(db.USER_USER.Where(p => p.USER_ID1 == UserId & p.USER_ID2 == FriendId).FirstOrDefault());
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("删除好友操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 6.展示这个用户的动态
        /// 输入：用户的id
        /// 输出：这个用户的最近动态
        /// 完成测试
        /// </summary>
        public List<ActionInfo> getUserDynamics(string _UserId)
        {
            var db = new KUXIANGDBEntities();
            var results = new List<ActionInfo>();
            try
            {
                var ans = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == _UserId).ToList();
                var user = db.USERTABLEs.Where(p => p.USER_ID == _UserId).FirstOrDefault();
                foreach (var each in ans)
                {
                    var repo = db.REPOSITORies.Where(p => p.REPOSITORY_ID == each.REPOSITORY_ID).FirstOrDefault();
                    var result = new ActionInfo
                    {
                        UserId = user.USER_ID,
                        UserName = user.USER_NAME,
                        UserPhotoUrl = user.IMAGE,
                        UserOperation = each.OPERATION,
                        RepositoryName = repo.NAME,
                        RepositoryId = repo.REPOSITORY_ID,
                        UpdateInfo = each.DESCRIPTION
                    };
                    results.Add(result);
                }
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("查找个人动态操作异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 7.展示好友的动态
        /// 输入：用户的id
        /// 输出：所有好友的最近动态
        /// 测试成功
        /// </summary>
        public List<ActionInfo> showFriendDynamics(string UserId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<ActionInfo> newdy = new List<ActionInfo>();
                    var friends = db.USER_USER.Where(p => p.USER_ID1 == UserId).ToArray();
                    foreach (var friend in friends)
                    {
                        var result = getUserDynamics(friend.USER_ID2);
                        foreach (var each in result)
                        {
                            newdy.Add(each);
                        }
                    }
                    return newdy;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("查找好友动态操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 8.展示这个用户的所有相关仓库
        /// 输入：用户的id，关系类型(0创建的仓库，1管理的仓库，2star的仓库，3fork的仓库，4所有的仓库)
        /// 输出：所有相关的仓库名（是否要链接）
        /// 测试成功
        /// </summary>
        public List<RepertorySearchResult> showOthersRepertory(string UserId,int flag)
        {
            var db = new KUXIANGDBEntities();
            List<RepertorySearchResult> res = new List<RepertorySearchResult>();

            try
            {
                var results =
                from reposit in db.REPOSITORies
                join user_reposit in db.USER_REPOSITORY_RELATIONSHIP
                on reposit.REPOSITORY_ID equals user_reposit.REPOSITORY_ID
                where user_reposit.USER_ID == UserId
                select new { reposit, relation = user_reposit.RELATIONSHIP};

                if (flag < 3)
                {
                    results =
                        from reposit in results
                        where reposit.relation == flag
                        select reposit;
                }
                else
                {
                    if (flag == 3)
                    {
                       results =
                            from reposit in results
                            where reposit.relation == 0
                            where reposit.reposit.IS_CREATE == 0
                            select reposit;
                    }
                }

                foreach (var row in results)
                {
                    RepertorySearchResult newres = new RepertorySearchResult(row.reposit);
                    res.Add(newres);
                }
                return res;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("展示用户的相关仓库");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 9.浏览历史显示
        /// 输入：接收者的编号
        /// 输出：浏览历史
        /// 测试成功
        /// </summary> 
        public List<USER_REPOSITORY_LOOKHISTORY> showLookHistory(string userId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var lookhistory = from row in db.USER_REPOSITORY_LOOKHISTORY
                                      where row.USER_ID == userId
                                      select row;
                    return lookhistory.ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("显示浏览历史操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 10.下载头像
        /// 输入：用户id
        /// 输出：头像图片url
        /// 测试成功
        /// </summary>
        public string DownloadImage(string userid)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    string url = null;
                    url = (from s in db.USERTABLEs
                           where s.USER_ID == userid
                           select s.IMAGE).FirstOrDefault();
                    return url;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("图片地址读取异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 11.审核请求
        /// 输入：用户id，仓库id，文件id，是否许可(0是许可，1是不许可)
        /// 输出：是否操作成功，错误信息
        /// 待测试
        /// </summary>
        public bool verifyRequest(string userId, string repId, string fileId, int flag, out ErrorMessage errorMessage)
        {
            var fileOp = new AboutFile();
            var db = new KUXIANGDBEntities();
            var user = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == userId
                                                             && p.REPOSITORY_ID == repId
                                                             && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).FirstOrDefault();
            if (user == null)
            {
                var error = new ErrorMessage();
                error.ErrorOperation = "审核请求操作";
                error.ErrorReason = "没有权限进行审核";
                error.ErrorTime = DateTime.Now;
                errorMessage = error;
                return false;
            }
            else
            {
                if (flag == 0)
                {
                    var file = db.FILETABLEs.Where(p => p.FILE_ID == fileId).FirstOrDefault();
                    file.FILE_STATE = 1;
                    changeUserGrade(userId, 1);
                    errorMessage = null;
                    return true;
                }
                else
                {
                    var errorM = new ErrorMessage();
                    if (!fileOp.removeFile(userId, repId, fileId, out errorM))
                    {
                        var error = new ErrorMessage();
                        error.ErrorOperation = "审核请求操作";
                        error.ErrorReason = "删除操作失败";
                        error.ErrorTime = DateTime.Now;
                        errorMessage = error;
                        return false;
                    }
                    else
                    {
                        errorMessage = null;
                        return true;
                    }
                }
            }
        }
    }
}