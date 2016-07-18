﻿using System;
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


        ///宋伟
        /// <summary>
        /// 添加新用户到数据库
        /// 输入：用户账号，用户密码，用户名，所在院系，邮箱地址，用户身份，用户积分
        /// 输出：是否创建成功
        /// 待测试
        /// </summary>
        public bool addUserInfo(string UserAccount, string UserKey, string UserName, string UserDepartment, string UserEmail, short UserIdentity, short UserGrade)
        {
            USERTABLE newUser = new USERTABLE
            {
                USER_NAME = UserName,
                PASSWORD = UserKey,
                DEPARTMENT = UserDepartment,
                EMAIL = UserEmail,
                IDENTITY = UserIdentity,
                GRADE = UserGrade
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
        /// 修改用户信息（只能修改用户名，邮箱）
        /// 输入：用户id，用户名，用户邮箱
        /// 输出：是否修改成功
        /// 待测试
        /// </summary>
        public bool changeUserInfo(string UserId, string UserName, string UserEmail)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    USERTABLE oldUserInfo = db.USERTABLEs.Where(p => p.USER_ID == UserId).FirstOrDefault();
                    oldUserInfo.USER_NAME = UserName;
                    oldUserInfo.EMAIL = UserEmail;
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
        /// 改变用户的积分
        /// 输入：用户id,修改的分数值（+为正，-为负）
        /// 输出：修改后的分数(失败后返回-1)
        /// 待测试
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
        /// 关注好友
        /// 输入：用户的id，朋友的id
        /// 输出：操作是否成功
        /// 待测试
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
        /// 删除好友
        /// 输入：用户的id，朋友的id
        /// 输出：操作是否成功
        /// 待测试
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
        /// 展示好友的动态
        /// 输入：用户的id
        /// 输出：所有好友的最近动态
        /// 待测试
        /// </summary>
        public string[] showFriendDynamics(string UserId)
        {
            return null;
        }

        /// <summary>
        /// 展示用户相关的所有仓库
        /// 输入：用户的id
        /// 输出：所有相关的USER_REPOSITORY_RELATIONSHIP类（失败返回空）
        /// 待测试
        /// </summary>
        public USER_REPOSITORY_RELATIONSHIP[] showUserRepertory(string UserId)
        {
            USER_REPOSITORY_RELATIONSHIP[] userRep = { };
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    userRep = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == UserId).ToArray();
                    return userRep;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("查找用户相关仓库操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }

            }
        }

        /// <summary>
        /// 展示这个用户的所有收藏
        /// 输入：用户的id
        /// 输出：所有收藏的仓库名（是否要链接）
        /// 待测试
        /// </summary>
        public string[] showOthersStar(string UserId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var result =
                    from reposit in db.REPOSITORies
                    join user_reposit in db.USER_REPOSITORY_RELATIONSHIP
                    on reposit.REPOSITORY_ID equals user_reposit.REPOSITORY_ID
                    where user_reposit.RELATIONSHIP == 2
                    where user_reposit.USER_ID == UserId
                    select reposit.NAME;
                    return result.ToArray();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("显示收藏操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 展示这个用户的所有相关仓库
        /// 输入：用户的id
        /// 输出：所有相关的仓库名（是否要链接）
        /// 待测试
        /// </summary>
        public string[] showOthersRepertory(string UserId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var result =
                    from reposit in db.REPOSITORies
                    join user_reposit in db.USER_REPOSITORY_RELATIONSHIP
                    on reposit.REPOSITORY_ID equals user_reposit.REPOSITORY_ID
                    where user_reposit.USER_ID == UserId
                    select reposit.NAME;
                    return result.ToArray();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("显示相关仓库操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 添加副本仓库
        /// 输入：用户id，被添加副本的仓库id
        /// 输出：副本仓库
        /// 待测试
        /// </summary>
        public REPOSITORY addForkRepertory(string UserId, string RepertoryId)
        {

            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    REPOSITORY origin = db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepertoryId).FirstOrDefault();

                    var ofile = db.REPOSITORY_FILE.Where(a => a.REPOSITORY_ID == RepertoryId);
                    USER_REPOSITORY_OPERATION operation = new USER_REPOSITORY_OPERATION
                    {
                        USER_ID = UserId,
                        REPOSITORY_ID = RepertoryId,
                        OPERATION_DATE = DateTime.Now,
                        OPERATION = "Fork"
                    };
                    REPOSITORY reposit = new REPOSITORY
                    {
                        REPOSITORY_ID = "NULL",
                        NAME = origin.NAME,
                        LABEL1 = origin.LABEL1,
                        LABEL2 = origin.LABEL2,
                        LABEL3 = origin.LABEL3,
                        ATTRIBUTE = origin.ATTRIBUTE,
                        AUTHORITY = origin.AUTHORITY,
                        FORK_NUM = origin.FORK_NUM,
                        STAR_NUM = origin.STAR_NUM,
                        FORK_FROM = RepertoryId,
                        IS_CREATE = 0
                    };

                    var newfile = ofile;
                    foreach (var temp in newfile)
                    {
                        temp.REPOSITORY_ID = RepertoryId;
                        db.REPOSITORY_FILE.Add(temp);
                    }
                    db.REPOSITORies.Add(reposit);
                    db.USER_REPOSITORY_OPERATION.Add(operation);
                    db.SaveChanges();
                    return reposit;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("分支仓库操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 删除副本仓库
        /// 输入：副本仓库的id
        /// 输出：是否操作成功
        /// 待测试
        /// </summary>
        public bool deleteForkRepertory(string RepertoryId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    db.REPOSITORies.Remove(db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepertoryId).FirstOrDefault());
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("删除副本仓库操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }

        }
        ///<summary>
        ///消息推送
        ///输入： 接收者的编号
        ///输出： 消息队列
        ///待测试
        ///</summary> 
        public List<MESSAGE> pushMessage(string receiver_ID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var message = db.MESSAGEs.Where(a => a.RECEIVER_ID == receiver_ID);
                    return message.ToList();
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("消息推送操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        ///<summary>
        ///记录操作历史
        ///输入：用户的编号，仓库编号，操作
        ///输出： 布尔值，true为成功，false为失败
        ///待测试
        ///</summary> 
        public bool recordOperation(string userId, string repositoryID, string operation)
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        ///<summary>
        ///操作历史显示
        ///输入：用户的编号
        ///输出： 操作历史(是否要链接）
        ///待测试
        ///</summary> 
        public List<string> showOperationHistory(string userId, DateTime start)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<string> res = null;
                    var history = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == userId);
                    foreach (var str in history)
                    {
                        string reposit, date, operation, username;
                        username = (from s in db.USERTABLEs
                                    where s.USER_ID == userId
                                    select s.USER_NAME).FirstOrDefault();
                        operation = str.OPERATION;
                        date = str.OPERATION_DATE.ToString();
                        reposit = (from r in db.REPOSITORies
                                   where r.REPOSITORY_ID == str.REPOSITORY_ID
                                   select r.NAME).FirstOrDefault();
                        string temp = username + ' ' + operation + ' ' + reposit + ' ' + date;
                        res.Add(temp);
                    }
                    return res;
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("操作历史显示异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        /*
        ///<summary>
        ///浏览历史显示
        ///输入： 接收者的编号
        ///输出： 浏览历史（是否要链接？）
        ///待测试
        ///</summary> 
        public string[] showLookHistory(string userId)
        {

        }

        ///<summary>
        ///留言板显示
        ///输入： 接收者的编号
        ///输出：评论队列
        ///待测试
        ///</summary> 
        public List<comment> showUserComment(string userId)
        {

        }

        ///<summary>
        ///官方库主页面统计量
        ///输入： 官方库的编号
        ///输出： 被收藏次数，被浏览次数，被分支次数
        ///待测试
        ///</summary> 
        public List<int> officialStatistics(string repositoryId)
        {

        }

        ///<summary>
        ///客户端流量和上次次数统计
        ///输入： 起始日期,截至日期
        ///输出： 浏览量，上传次数
        ///待测试
        ///</summary> 
        public List<int> clientStatistics(DateTime start, DateTime end)
        {

        }
        /*
        ///张尹嘉
        /// <summary>
        /// 添加评论到数据库
        /// 后两个参数用什么确定被评论的仓库和评论人还有待确定，暂时用ID进行识别
        /// </summary>
        public bool addComment(Nullable<System.DateTime> CommentData, string content, string RepositoryID, string AnnouncerID)
        {
            //TODO
        }

        /// <summary>
        /// 检索功能--仓库
        /// 输入：要检索的仓库的名称和标签
        /// 输出：检索出的仓库ID列表
        /// 待测试
        /// </summary>
        public List<string> searchRepository(string RepositoryName, string RepositoryLabel)
        {
            //TODO
        }

        /// <summary>
        /// 检索功能--文件
        /// 输入：要检索的文件的名称
        /// 输出：检索出的文件ID列表
        /// 待测试
        /// </summary>
        public List<string> searchFile(string FileName)
        {
            //TODO
        }

        /// <summary>
        /// 检索功能--用户
        /// 输入：要检索的用户的名称
        /// 输出：检索出的用户ID列表
        /// 待测试
        /// </summary>
        public List<string> searchUser(string UserName)
        {
            //TODO
        }

        /// <summary>
        /// 推荐--按老师推荐
        /// 输入：老师的ID列表
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public List<string> recommendRepositoryByTeachers(List<string> TeacherID)
        {
            //TODO
        }

        /// <summary>
        /// 推荐--按仓库热度推荐
        /// 输入：无
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public List<string> recommendRepositoryByHeat()
        {
            //TODO
        }

        /// <summary>
        /// 推荐--按用户浏览历史推荐
        /// 输入：用户ID
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public List<string> recommendRepositoryByLookingHistory(string UserID)
        {
            //TODO
        }

        /// <summary>
        /// 查询用户的统计信息
        /// 输入：用户ID
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public int getContributionNum(string UserID)
        {
            //TODO
        }

        ///张志强
        /// <summary>
        /// 邀请管理者
        /// 输入:用户ID
        /// 输出：返回布尔类型，确定是否添加成功
        /// 待测试
        /// </summary>
        public bool AddPartner(string User_id)
        {
        }

        /// <summary>
        /// 删除管理者
        /// 输入：用户ID
        /// 输出：返回布尔类型，确定是否删除成功
        /// 待测试
        /// </summary>
        public bool DletePartner(string User_id)
        {
        }

        /// <summary>
        /// 添加或修改仓库描述
        /// 输入：仓库描述des
        /// 输出：返回布尔类型，确定是否添加描述成功
        /// 待测试
        /// /// </summary>
        public bool AddDescribion(string des)
        {
        }

        /// <summary>
        /// 修改仓库基本信息（仓库名称，仓库标签）
        /// 输入：名称，标签
        /// 输出：返回布尔类型，确定是否修改成功
        /// 待测试
        /// </summary>
        public bool ModifyInformation(string name, string label1, string label2, string label3)
        {
        }
        */

    }
}