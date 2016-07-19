﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Database_course_design.Models.ItemModel;

namespace Database_course_design.Models
{
    public class DBModel
    {
        /// <summary>
        /// 创建主码ID
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
        /// 添加新用户到数据库
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
        /// 创建课程
        /// 输入：三级label（1级学校、2级院系、3级课程）
        /// 输出：是否插入成功
        /// 待测试
        /// </summary>
        public bool createCourse(string label1,string label2, string label3)
        {
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            var newCourse = new COURSE()
            {
                COURSE_ID = createNewId("COURSE"),
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

        /// <summary>
        /// 获取一个仓库的管理者人数
        /// 输入：仓库ID
        /// 输出：管理者的人数
        /// 测试成功
        /// </summary>
        public int getRepertoryManageNum(string RepertoryId)
        {
            var db = new KUXIANGDBEntities();
            try
            {
                var result = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == RepertoryId 
                                                                && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).ToList();
                int num = 0;
                if (result == null)
                {
                    return num;
                }
                else
                {
                    return result.Count();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("查找仓库的管理者人数异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 显示仓库文件
        /// 输入：仓库ID
        /// 输出：文件名,文件类型，文件大小
        /// 测试成功
        /// </summary>
        public struct FileInfo
        {
            public string name;
            public string type;
            public string size;
        }
        public List<FileInfo> showFile(string resipositoryId, ref string resipositoryName)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<FileInfo> res = new List<FileInfo>();
                    var file = db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == resipositoryId);
                    resipositoryName = (from a in db.REPOSITORies
                                        where a.REPOSITORY_ID == resipositoryId
                                        select a.NAME).FirstOrDefault();
                    foreach (var str in file)
                    {
                        FileInfo temp = new FileInfo();
                        temp.name = (from s in db.FILETABLEs
                                     where s.FILE_ID == str.FILE_ID
                                     select s.FILE_NAME).FirstOrDefault();
                        temp.type = (from q in db.FILETABLEs
                                     where q.FILE_ID == str.FILE_ID
                                     select q.FILE_TYPE).FirstOrDefault();
                        temp.size = Convert.ToString((from p in db.FILETABLEs
                                                      where p.FILE_ID == str.FILE_ID
                                                      select p.FILE_SIZE).FirstOrDefault());
                        res.Add(temp);
                    }
                    return res;
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("文件显示异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 修改用户信息（只能修改用户名，邮箱）
        /// 输入：用户id，用户名，用户邮箱
        /// 输出：是否修改成功
        /// 测试成功
        /// </summary>
        public bool changeUserInfo(string UserId, string UserEmail,string UserImage,string UserSignature,string UserNickname,string UserSelfUrl,string UserAddress)
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
        /// 改变用户的积分
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
        /// 关注好友
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
        /// 删除好友
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
        /// 展示这个用户的动态
        /// 输入：用户的id
        /// 输出：这个用户的最近动态
        /// 完成测试
        /// </summary>
        public List<actionInfo> getUserDynamics(string _UserId)
        {
            var db = new KUXIANGDBEntities();
            var results = new List<ItemModel.actionInfo>();
            try
            {
                var ans = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == _UserId).ToList();
                var user = db.USERTABLEs.Where(p => p.USER_ID == _UserId).FirstOrDefault();
                foreach (var each in ans)
                {
                    var repo = db.REPOSITORies.Where(p => p.REPOSITORY_ID == each.REPOSITORY_ID).FirstOrDefault();
                    var result = new ItemModel.actionInfo
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
        /// 展示好友的动态
        /// 输入：用户的id
        /// 输出：所有好友的最近动态
        /// 测试成功
        /// </summary>
        public List<actionInfo> showFriendDynamics(string UserId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<actionInfo> newdy = new List<ItemModel.actionInfo>();
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
        /// 测试成功
        /// </summary>
        public List<RepertorySearchResult> showOthersStar(string UserId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<RepertorySearchResult> res = new List<RepertorySearchResult>();

                    var result =
                    from reposit in db.REPOSITORies
                    join user_reposit in db.USER_REPOSITORY_RELATIONSHIP
                    on reposit.REPOSITORY_ID equals user_reposit.REPOSITORY_ID
                    where (user_reposit.USER_ID == UserId && user_reposit.RELATIONSHIP == 2)
                    select reposit;

                    foreach (var row in result)
                    {
                        var labels = db.COURSEs.Where(p => p.COURSE_ID == row.COURSE_ID).FirstOrDefault();
                        var createrName = from user in db.USERTABLEs
                                          join relation in db.USER_REPOSITORY_RELATIONSHIP
                                          on user.USER_ID equals relation.USER_ID
                                          where relation.RELATIONSHIP == 0
                                          select user.USER_NAME;

                        RepertorySearchResult newres = new RepertorySearchResult
                        {
                            RepertoryName = row.NAME,
                            RepertoryFork = (int)row.FORK_NUM,
                            RepertoryStar = (int)row.STAR_NUM,
                            RepertoryUpdateTime = row.UPDATE_DATE.ToString(),
                            RepertoryInfo = row.DESCRIPTION,
                            RepertoryLabel1 = labels.LABEL1,
                            RepertoryLabel2 = labels.LABEL2,
                            RepertoryLabel3 = labels.LABEL3,
                            RepertoryCreater = createrName.FirstOrDefault()
                        };
                        res.Add(newres);
                    }
                    return res;
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
        /// 测试成功
        /// </summary>
        public List<RepertorySearchResult> showOthersRepertory(string UserId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<RepertorySearchResult> res = new List<RepertorySearchResult>();
                    var result =
                    from reposit in db.REPOSITORies
                    join user_reposit in db.USER_REPOSITORY_RELATIONSHIP
                    on reposit.REPOSITORY_ID equals user_reposit.REPOSITORY_ID
                    where user_reposit.USER_ID == UserId
                    select reposit;

                    foreach (var row in result)
                    {
                        var labels = db.COURSEs.Where(p => p.COURSE_ID == row.COURSE_ID).FirstOrDefault();
                        var name = from user in db.USERTABLEs
                                   join relation in db.USER_REPOSITORY_RELATIONSHIP
                                   on user.USER_ID equals relation.USER_ID
                                   where relation.RELATIONSHIP == 0
                                   select user.USER_NAME;

                        RepertorySearchResult newres = new RepertorySearchResult
                        {
                            RepertoryName = row.NAME,
                            RepertoryFork = (int)row.FORK_NUM,
                            RepertoryStar = (int)row.STAR_NUM,
                            RepertoryUpdateTime = row.UPDATE_DATE.ToString(),
                            RepertoryInfo = row.DESCRIPTION,
                            RepertoryLabel1 = labels.LABEL1,
                            RepertoryLabel2 = labels.LABEL2,
                            RepertoryLabel3 = labels.LABEL3,
                            RepertoryCreater = name.FirstOrDefault()
                        };
                        res.Add(newres);
                    }
                    return res;
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
        /// 添加浏览历史
        /// 输入：用户的id，仓库的id
        /// 输出：操作是否成功
        /// </summary>
        public bool recordLookHistory(string userId,string repositoryId)
        {
            var db = new KUXIANGDBEntities();
            var name = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryId).FirstOrDefault().NAME;
            var lookHistory = new USER_REPOSITORY_LOOKHISTORY
            {
                USER_ID = userId,
                REPOSITORY_ID = repositoryId,
                REPOSITORY_NAME = name,
                LOOK_DATE = DateTime.Now
            };

            try
            {
                db.USER_REPOSITORY_LOOKHISTORY.Add(lookHistory);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("记录浏览历史异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        ///<summary>
        ///记录操作历史
        ///输入：用户的编号，仓库编号，操作
        ///输出： 布尔值，true为成功，false为失败
        ///测试成功
        ///</summary>
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

        /// <summary>
        /// 删除副本仓库
        /// 输入：副本仓库的id
        /// 输出：是否操作成功
        /// 测试成功
        /// </summary>
        public bool deleteForkRepertory(string RepositoryId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    string userId = db.USER_REPOSITORY_RELATIONSHIP.Where(q => q.REPOSITORY_ID == RepositoryId).FirstOrDefault().USER_ID;
                    recordOperation(userId, RepositoryId, "Delete", null);
                    var res_file = from row in db.REPOSITORY_FILE
                                   where row.REPOSITORY_ID == RepositoryId
                                   select row;
                    foreach (var file in res_file)
                    {
                        db.FILETABLEs.Remove(db.FILETABLEs.Where(p => p.FILE_ID == file.FILE_ID).First());
                        db.REPOSITORY_FILE.Remove(db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == RepositoryId).FirstOrDefault());
                    }

                    db.REPOSITORies.Remove(db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepositoryId).FirstOrDefault());
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

        /// <summary>
        /// 创建系统消息
        /// 输入: 用户ID，消息内容
        /// 输出：返回布尔类型，确定是否创建成功
        /// 测试成功
        //</summary>
        public bool addMessageToUser(string userid,string content)
        {
            using (KUXIANGDBEntities db = new Models.KUXIANGDBEntities())
            {
                try
                {
                    MESSAGE newMessage = new MESSAGE();
                    newMessage.IS_READ = 0;
                    newMessage.MESSAGE_CONTENT = content;
                    newMessage.MESSAGE_DATE = System.DateTime.Now;
                    newMessage.RECEIVER_ID = userid;
                    newMessage.MESSAGE_ID = createNewId("MESSAGE");
                    db.MESSAGEs.Add(newMessage);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("添加消息到用户操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        ///<summary>
        ///消息推送
        ///输入： 接收者的编号
        ///输出： 消息队列
        ///测试成功
        ///</summary>
        public List<MESSAGE> pushMessage(string receiver_ID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var message = db.MESSAGEs.Where(a => a.RECEIVER_ID == receiver_ID && a.IS_READ == 0);
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
        ///操作历史显示
        ///输入：用户的编号
        ///输出： 操作历史(是否要链接）
        ///测试成功
        ///</summary>
        public List<Database_course_design.Models.ItemModel.OperationItem> showOperationHistory(string userId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<Database_course_design.Models.ItemModel.OperationItem> Operation = new List<Database_course_design.Models.ItemModel.OperationItem>();
                    Database_course_design.Models.ItemModel.OperationItem res = new ItemModel.OperationItem();
                    var history = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == userId);
                    foreach (var str in history)
                    {
                        string reposit, operation, username;
                        username = (from s in db.USERTABLEs
                                    where s.USER_ID == userId
                                    select s.USER_NAME).FirstOrDefault();
                        operation = str.OPERATION;
                        DateTime date = str.OPERATION_DATE;
                        reposit = (from r in db.REPOSITORies
                                   where r.REPOSITORY_ID == str.REPOSITORY_ID
                                   select r.NAME).FirstOrDefault();
                        res.DATE = date;
                        res.REPOSITORY_ID = str.REPOSITORY_ID;
                        res.OPERATION = operation;
                        res.REPOSITORY_NAME = reposit;
                        res.USER_NAME = username;
                        Operation.Add(res);
                    }
                    return Operation;
                }
                
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("操作历史显示异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }


        ///张尹嘉
        /// <summary>
        ///对仓库进行评论
        /// 输入：评论内容，评论日期，仓库id，发布者id，
        /// 输出：布尔类型，判断评论是否成功
        /// </summary>   KUXIANGDBEntities
        public bool addCommentToRepository(string content, string RepositoryID, string AnnouncerID)
        {
            COMMENTTABLE newComment = new COMMENTTABLE
            {
                COMMENT_ID = createNewId("COMMENTTABLE"),
                COMMENT_DATE = DateTime.Now,
                CONTENT = content
            };
            USER_COMMENT_REPOSITORY newCommentRepository = new USER_COMMENT_REPOSITORY
            {
                COMMENT_ID = newComment.COMMENT_ID,
                REPOSITORY_ID = RepositoryID,
                ANNOUNCER = AnnouncerID
            };

            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    db.COMMENTTABLEs.Add(newComment);
                    db.USER_COMMENT_REPOSITORY.Add(newCommentRepository);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("创建评论异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }

        }
        /// <summary>
        ///对用户进行评论
        /// 输入：评论内容，评论日期，被评论者id，发布者id，
        /// 输出：布尔类型，判断评论是否成功
        /// 测试成功
        /// </summary>  
        public bool addCommentToUser(string content, string UserId, string AnnouncerID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    COMMENTTABLE newComment = new COMMENTTABLE
                    {
                        COMMENT_ID = createNewId("COMMENTTABLE"),
                        COMMENT_DATE = DateTime.Now,
                        CONTENT = content
                    };
                    USER_COMMENT_USER newCommentUser = new USER_COMMENT_USER
                    {
                        COMMENT_ID = newComment.COMMENT_ID,
                        USER_ID = UserId,
                        ANNOUNCER = AnnouncerID
                    };
                    db.COMMENTTABLEs.Add(newComment);
                    db.USER_COMMENT_USER.Add(newCommentUser);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("创建评论异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 邀请管理者
        /// 输入: 用户ID，仓库ID
        /// 输出：返回布尔类型，确定是否添加成功
        /// 测试成功
        //</summary>
        public bool AddPartner(string UserID, string RepositoryID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    USER_REPOSITORY_RELATIONSHIP partner = new USER_REPOSITORY_RELATIONSHIP
                    {
                        USER_ID = UserID,
                        REPOSITORY_ID = RepositoryID,
                        RELATIONSHIP = 1
                    };

                    db.USER_REPOSITORY_RELATIONSHIP.Add(partner);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("邀请管理者操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除管理者
        /// 输入：用户ID
        /// 输出：返回布尔类型，确定是否删除成功
        /// 测试成功
        /// </summary>
        public bool DeletPartner(string UserID, string RepositoryID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    db.USER_REPOSITORY_RELATIONSHIP.Remove(db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == UserID && p.REPOSITORY_ID == RepositoryID).FirstOrDefault());
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("删除管理者操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 修改仓库信息
        /// 输入：仓库描述des，仓库id
        /// 输出：返回布尔类型，确定是否添加描述成功
        /// 测试成功
        /// /// </summary>
        public bool ModifyInformation(string RepositoryId, string newDes)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    REPOSITORY oldrepository = db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepositoryId).FirstOrDefault();
                    if (newDes != "")
                    {
                        oldrepository.DESCRIPTION = newDes;
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("仓库修改异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 创建仓库
        /// 输入：用户名，库的名称，创建库的公私级别，库的描述，课程的名字
        /// 输出：创建成功与否
        /// 测试成功
        /// </summary>
        public bool CreateRepository(string userid, string name, int authority, string description,string label3)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    USERTABLE user = db.USERTABLEs.Where(p => p.USER_ID == userid).FirstOrDefault();
                    COURSE oldCourse = db.COURSEs.Where(p => p.LABEL3 == label3 
                                                        && p.LABEL1 == user.UNIVERSITY 
                                                        && p.LABEL2 == user.DEPARTMENT).FirstOrDefault();
                    if (oldCourse == null)
                    {
                        COURSE newCourse = new COURSE
                        {
                            LABEL1 = user.UNIVERSITY,
                            LABEL2 = user.DEPARTMENT,
                            LABEL3 = label3,
                            COURSE_ID = createNewId("COURSE")
                        };
                        db.COURSEs.Add(newCourse);
                        oldCourse = newCourse;
                    }

                    string repositoryid = createNewId("REPOSITORY");
                    short attri = 0;
                    //1是创建 0是fork
                    short iscreate = 1;
                    String forkfrom = "";
                    short relationship = 0;
                    REPOSITORY newRep = new REPOSITORY
                    {
                        REPOSITORY_ID = repositoryid,
                        NAME = name,
                        AUTHORITY = authority,
                        ATTRIBUTE = attri,
                        IS_CREATE = iscreate,
                        FORK_FROM = forkfrom,
                        DESCRIPTION = description,
                        COURSE_ID = oldCourse.COURSE_ID,
                        STAR_NUM = 0,
                        FORK_NUM = 0,
                        UPDATE_DATE = DateTime.Now
                    };
                    db.REPOSITORies.Add(newRep);

                    USER_REPOSITORY_RELATIONSHIP userrepr = new USER_REPOSITORY_RELATIONSHIP
                    {
                        USER_ID = userid,
                        REPOSITORY_ID = newRep.REPOSITORY_ID,
                        RELATIONSHIP = relationship
                    };
                    db.USER_REPOSITORY_RELATIONSHIP.Add(userrepr);
                    db.SaveChanges();

                    recordOperation(userid, repositoryid, "Create", description);
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("仓库创建失败");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除仓库
        /// 输入：用户ID，仓库ID
        /// 输出：删除成功与否
        /// 待测试
        /// </summary>
        public bool RemoveRepository(string userid, string repositoryid,out ErrorMessage errorMessage)
        {
            int numb = 0;
            using (var db = new KUXIANGDBEntities())
            {
                try
                {
                    numb =
                        (from p
                         in db.USER_REPOSITORY_RELATIONSHIP
                         where repositoryid == p.REPOSITORY_ID
                         where userid == p.USER_ID
                         where p.RELATIONSHIP == 0
                         select p
                        ).Count();

                    if (numb != 0)
                    {
                        var fileList = db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == repositoryid).ToList();
                        foreach (var each in fileList)
                        {
                            var error = new ErrorMessage();
                            removeFile(userid,repositoryid,each.FILE_ID,out error);
                            if (error != null)
                            {
                                errorMessage = error;
                            }
                        }
                        db.REPOSITORies.Remove(db.REPOSITORies.Where(p =>p.REPOSITORY_ID == repositoryid).FirstOrDefault());
                        db.SaveChanges();
                    }
                    else
                    {
                        var error = new ErrorMessage
                        {
                            ErrorOperation = "删除仓库操作",
                            ErrorReason = "没有权限删除仓库",
                            ErrorTime = DateTime.Now
                        };
                        errorMessage = error;
                        return false;
                    }

                    if (!recordOperation(userid, repositoryid, "Delete", null))
                    {
                        var error = new ErrorMessage
                        {
                            ErrorOperation = "添加操作信息操作",
                            ErrorReason = "内函数异常",
                            ErrorTime = DateTime.Now
                        };
                        errorMessage = error;
                        return false;
                    }
                    else
                    {
                        errorMessage = null;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    var error = new ErrorMessage();
                    error.ErrorOperation = "仓库删除失败！";
                    error.ErrorReason = ex.Message;
                    error.ErrorTime = DateTime.Now;
                    errorMessage = error;
                    return false;
                }
            }
        }

        /// <summary>
        /// 改变用户对仓库的star状态
        /// 输入：用户id，仓库id
        /// 输出：是否成功，错误信息
        /// 待测试
        /// </summary>
        public bool changeStar(string userId,string repositoryId,out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            var result = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == userId
                                                               && p.REPOSITORY_ID == repositoryId
                                                               && p.RELATIONSHIP == 2).FirstOrDefault();
            var username = db.USERTABLEs.Where(p => p.USER_ID == userId).FirstOrDefault().NICKNAME;
            var repname = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryId).FirstOrDefault().NAME;
            try
            {
                if (result != null)
                {
                    db.USER_REPOSITORY_RELATIONSHIP.Remove(result);
                    var error = new ErrorMessage();
                    if (recordOperation(userId, repositoryId, "Unstar", username + "取消收藏了" + repname))
                    {
                        error.ErrorOperation = "添加操作信息操作";
                        error.ErrorReason = "内部函数异常";
                        error.ErrorTime = DateTime.Now;    
                    }
                    else
                    {
                        error = null;
                    }

                    var rep = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryId).FirstOrDefault();
                    rep.STAR_NUM--;
                    db.SaveChanges();
                    errorMessage = error;
                    return true;
                }
                else
                {
                    var newRelation = new USER_REPOSITORY_RELATIONSHIP
                    {
                        USER_ID = userId,
                        REPOSITORY_ID = repositoryId,
                        RELATIONSHIP = 2
                    };
                    db.USER_REPOSITORY_RELATIONSHIP.Add(newRelation);
                    var error = new ErrorMessage();
                    if (recordOperation(userId, repositoryId, "Star", username + "收藏了" + repname))
                    {
                        error.ErrorOperation = "添加操作信息操作";
                        error.ErrorReason = "内部函数异常";
                        error.ErrorTime = DateTime.Now;
                    }
                    else
                    {
                        error = null;
                    }
                    var rep = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryId).FirstOrDefault();
                    rep.STAR_NUM++;
                    db.SaveChanges();
                    errorMessage = error;
                    return true;
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorMessage();
                error.ErrorOperation = "修改仓库star状态失败！";
                error.ErrorReason = ex.Message;
                error.ErrorTime = DateTime.Now;
                errorMessage = error;
                return false;
            }
        }

        ///<summary>
        ///留言板显示
        ///输入： 接收者的编号
        ///输出：评论队列
        ///测试成功
        ///</summary>
        public List<COMMENTTABLE> showUserComment(string userid)
        {
            using (var db = new KUXIANGDBEntities())
            {
                try
                {
                    var comment = from user in db.USER_COMMENT_USER
                                  join commnt in db.COMMENTTABLEs
                                  on user.COMMENT_ID equals commnt.COMMENT_ID
                                  where user.USER_ID == userid
                                  select commnt;
                    return comment.ToList();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("修改文件状态失败！");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }

            }
        }

        ///<summary>
        ///浏览历史显示
        ///输入： 接收者的编号
        ///输出： 浏览历史（是否要链接？）
        ///测试成功
        ///</summary> 
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
        /// 检索功能——仓库
        /// 输入：检索仓库的关键字（标签，仓库名），标记变量d(d为true：标签检索，d为false：关键字检索）
        /// 输出：检索出的仓库ID列表
        /// 测试成功
        /// </summary>
        public string[] searchRepository(string zd, bool d)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    if (d)
                    {
                        var newRepositoryLabel = from p in db.REPOSITORies
                                                 join c in db.COURSEs
                                                 on p.COURSE_ID equals c.COURSE_ID
                                                 where c.LABEL1.Contains(zd) || c.LABEL2.Contains(zd) || c.LABEL3.Contains(zd)
                                                 select p.REPOSITORY_ID;
                        return newRepositoryLabel.ToArray();
                    }
                    var newRepository = from p in db.REPOSITORies
                                        join c in db.COURSEs
                                        on p.COURSE_ID equals c.COURSE_ID
                                        where c.LABEL1.Contains(zd) || c.LABEL2.Contains(zd) || c.LABEL3.Contains(zd) || p.NAME.Contains(zd)
                                        select p.REPOSITORY_ID;
                    return newRepository.ToArray();
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
        /// 检索功能--文件
        /// 输入：要检索的文件的名称
        /// 输出：检索出的文件ID列表
        /// 测试成功
        /// </summary>
        public string[] searchFile(string FileName, string repositoryId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var newfile = from p in db.FILETABLEs
                                  join c in db.REPOSITORY_FILE
                                  on p.FILE_ID equals c.FILE_ID
                                  where c.REPOSITORY_ID == repositoryId
                                  where p.FILE_NAME == FileName
                                  select p.FILE_ID;
                    return newfile.ToArray();
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
        /// 检索功能--用户
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
                                  where user.USER_NAME == UserName
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
        /// 推荐--按仓库热度推荐
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
        /*
        /// <summary>
        /// 推荐--按老师推荐
        /// 输入：老师的ID列表
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public List<string> recommendRepositoryByTeachers(List<string> TeacherID)
        {
            var teachrepolist = new List<string>();
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    foreach (var teacherid in TeacherID)
                    {
                        var teacherrepo =
                            from p
                            in db.USER_REPOSITORY_RELATIONSHIP
                            where p.USER_ID == teacherid
                            select p.REPOSITORY_ID;
                        foreach (var repositoryid in teacherrepo)
                        {
                            teachrepolist.Add(repositoryid);
                        }
                    }
                    return teachrepolist;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("按热度推荐仓库读取异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }
        
        /// <summary>
        /// 推荐--按用户浏览历史推荐
        /// 输入：用户ID
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public List<string> recommendRepositoryByLookingHistory(string UserID)
        {
            List<string> heatrepository = new List<string>();
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var temps = db.USER_REPOSITORY_LOOKHISTORY.Where(p => p.USER_ID == UserID);
                    string nowdate = "";
                    nowdate = DateTime.Now.ToShortDateString();
                    string dbdate, dategap;
                    int i, j, k;
                    foreach (var temp in temps)
                    {
                        dbdate = temp.ToString();
                        i = dbdate.IndexOf("-");
                        j = dbdate.IndexOf("月");
                        k = dbdate.LastIndexOf("-");
                        dbdate = "20" + dbdate.Substring(0, i) + "/" + dbdate.Substring(i + 1, j - i - 1) + "/" + dbdate.Substring(k + 1);
                        dategap = (DateTime.Parse(nowdate) - DateTime.Parse(dbdate)).ToString();
                        dategap = dategap.Substring(0, dategap.Length - 9);
                        if (int.Parse(dategap) < 30)
                            heatrepository.Add(temp.REPOSITORY_ID);
                    }
                    return heatrepository;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("按浏览历史推荐仓库读取异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        */
        /// <summary>
        /// 下载头像
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
                           select s.IMAGE ).FirstOrDefault();
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
        /// 拷贝文件
        /// 输入：旧文件id，新文件id，用户id, 旧仓库id
        /// 输出：拷贝是否成功
        /// 待测试
        /// </summary>
        public bool copyFile(string oldFileId, string newFileId, string userId, string RepositoryId)
        {
            //拷贝文件（复制，修改file_id,update_date,path(filetables))
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    FILETABLE old_file = db.FILETABLEs.Where(p => p.FILE_ID == oldFileId).FirstOrDefault();
                    string old_user_id = (from d in db.USER_REPOSITORY_RELATIONSHIP
                                          where d.REPOSITORY_ID == RepositoryId
                                          select d.USER_ID).FirstOrDefault();
                    string fullName = old_file.FILE_NAME + "." + old_file.FILE_TYPE;
                    string sourcePath = @old_file.PATH;
                    string new_path = (old_file.PATH).Replace(old_user_id, userId);
                    string targetPath = @new_path;                    //debug
                    string sourceFile = System.IO.Path.Combine(sourcePath, fullName);
                    string destFile = System.IO.Path.Combine(targetPath, fullName);
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }
                    System.IO.File.Copy(sourceFile, destFile, true);

                    FILETABLE new_file = new FILETABLE
                    {
                        FILE_ID = newFileId,
                        PATH = targetPath,
                        FILE_NAME = old_file.FILE_NAME,
                        FILE_TYPE = old_file.FILE_TYPE,
                        FILE_STATE = old_file.FILE_STATE,
                        FILE_SIZE = old_file.FILE_SIZE,
                        FILE_DEEP = old_file.FILE_DEEP
                    };
                    db.FILETABLEs.Add(new_file);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("复制文件操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }


        /// <summary>
        /// 添加副本仓库
        /// 输入：用户id，被添加副本的仓库id
        /// 输出：副本仓库
        /// 多级文件部分未测试
        /// </summary>
        public REPOSITORY addForkRepertory(string UserId, string RepositoryId)
        {

            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    REPOSITORY origin = db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepositoryId).FirstOrDefault();
                    var ofile = db.REPOSITORY_FILE.Where(a => a.REPOSITORY_ID == RepositoryId);
                    string new_id = createNewId("REPOSITORY");

                    //检查是否已经fork
                    var check = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == UserId && p.REPOSITORY_ID == RepositoryId && p.OPERATION == "Fork").FirstOrDefault();
                    if (check != null)
                    {
                        return null;
                    }

                    //更新操作
                    USER_REPOSITORY_OPERATION operation = new USER_REPOSITORY_OPERATION
                    {
                        USER_ID = UserId,
                        REPOSITORY_ID = RepositoryId,
                        OPERATION_DATE = DateTime.Now,
                        OPERATION = "Fork"
                    };
                    origin.FORK_NUM += 1;

                    //新建仓库
                    REPOSITORY reposit = new REPOSITORY
                    {
                        REPOSITORY_ID = new_id,
                        NAME = origin.NAME,
                        ATTRIBUTE =0,
                        AUTHORITY = origin.AUTHORITY,
                        //一致性?
                        FORK_NUM = 0,
                        STAR_NUM = 0,
                        FORK_FROM = RepositoryId,
                        IS_CREATE = 0,
                        COURSE_ID = origin.COURSE_ID,
                        UPDATE_DATE = System.DateTime.Now
                    };
                    USER_REPOSITORY_RELATIONSHIP relationship = new USER_REPOSITORY_RELATIONSHIP
                    {
                        REPOSITORY_ID = new_id,
                        USER_ID = UserId
                    };
                   
                    //db.SaveChanges();
                    db.REPOSITORies.Add(reposit);
                    db.USER_REPOSITORY_OPERATION.Add(operation);
                    db.USER_REPOSITORY_RELATIONSHIP.Add(relationship);

                    //db.SaveChanges();

                    var newfile = ofile;
                    Queue files = new Queue();
                    Dictionary<string, string> old_new = new Dictionary<string, string>();
                    foreach (var temp in newfile)
                    {
                        temp.REPOSITORY_ID = new_id;
                        var file = db.FILETABLEs.Where(p => p.FILE_ID == temp.FILE_ID).FirstOrDefault();
                        temp.FILE_ID = createNewId("FILETABLE");
                        old_new.Add(file.FILE_ID, temp.FILE_ID);
                        copyFile(file.FILE_ID, temp.FILE_ID, UserId, RepositoryId);
                        REPOSITORY_FILE file_repository = new REPOSITORY_FILE
                        {
                            REPOSITORY_ID = new_id,
                            FILE_ID = temp.FILE_ID
                        };
                        db.REPOSITORY_FILE.Add(file_repository);
                        files.Enqueue(file.FILE_ID);
                        db.REPOSITORY_FILE.Add(temp);
                        //db.SaveChanges();
                    }
                    while (files.Count > 0)
                    {
                        string cur = (files.Dequeue()).ToString();
                        var cur_file = db.FILE_FILE.Where(p => p.FILE_ID1 == cur);
                        foreach (var temp2 in cur_file)
                        {
                            var file = db.FILETABLEs.Where(p => p.FILE_ID == temp2.FILE_ID2).FirstOrDefault();
                            var temp3 = file;
                            temp3.FILE_ID = createNewId("FILETABLE");
                            old_new.Add(file.FILE_ID, temp3.FILE_ID);
                            copyFile(file.FILE_ID, temp3.FILE_ID, UserId, RepositoryId);
                            REPOSITORY_FILE file_repository = new REPOSITORY_FILE
                            {
                                REPOSITORY_ID = new_id,
                                FILE_ID = temp3.FILE_ID
                            };
                            FILE_FILE file_file = new FILE_FILE
                            {
                                FILE_ID1 = old_new[temp2.FILE_ID1],
                                FILE_ID2 = temp3.FILE_ID
                            };
                            db.REPOSITORY_FILE.Add(file_repository);
                            db.FILE_FILE.Add(file_file);
                            //db.SaveChanges();
                            files.Enqueue(temp2.FILE_ID2);
                        }
                    }
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
        /// 创建文件或者文件夹（type为0是文件，1是文件夹）
        /// 输入：仓库id，文件（夹）名字，文件类型（是否为文件夹），文件大小，所在位置，父节点是仓库还是文件夹（0是仓库），操作描述
        /// 输出：是否创建成功，文件id
        /// 待测试
        /// </summary>
        public bool createFile(string rep_id,string name,string type,int size,string position,int flag,string description, out string file_id)
        {
            var db = new KUXIANGDBEntities();

            string newFileId = createNewId("FILETABLE");
            string newPath = "";
            int? deepInt = 0;
            if (flag == 0)
            {
                newPath = db.REPOSITORies.Where(p => p.REPOSITORY_ID == position).FirstOrDefault().URL;
                deepInt = 1;
            }
            else
            {
                newPath = db.FILETABLEs.Where(p => p.FILE_ID == position).FirstOrDefault().PATH;
                deepInt = db.FILETABLEs.Where(p => p.FILE_ID == position).FirstOrDefault().FILE_DEEP + 1;
            }

            FILETABLE new_file = new FILETABLE
            {
                FILE_ID = newFileId,
                PATH = newPath + @"/" + name,
                FILE_NAME = name,
                FILE_TYPE = type,
                FILE_STATE = 0,
                FILE_SIZE = size,
                FILE_DEEP = deepInt
            };
            //文件夹因为判断了权限问题，所以可以直接完成，不需要审核
            if (type == "1")
            {
                new_file.FILE_STATE = 1;
            }
            try
            {
                db.FILETABLEs.Add(new_file);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("创建文件操作异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                file_id = "";
                return false;
            }

            if (flag == 0)
            {
                var repo_file = new REPOSITORY_FILE
                {
                    REPOSITORY_ID = position,
                    FILE_ID = newFileId,
                    REPOSITORY_FILE_ID = ""
                };
                try
                {
                    db.REPOSITORY_FILE.Add(repo_file);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("创建仓库与文件表操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    file_id = "";
                    return false;
                }
            }
            else
            {
                var file_file = new FILE_FILE
                {
                    FILE_ID1 = position,
                    FILE_ID2 = new_file.FILE_ID,
                    FILE_FILE_ID = ""
                };
                try
                {
                    db.FILE_FILE.Add(file_file);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("创建文件与文件表操作异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    file_id = "";
                    return false;
                }
            }
            db.SaveChanges();

            string userId = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == rep_id).FirstOrDefault().USER_ID;
            if (type == "0")
            {
                if (!recordOperation(userId, rep_id, "Upload", description))
                {
                    System.Diagnostics.Debug.WriteLine("创建文件操作表操作异常");
                    file_id = "";
                    return false;
                }
                else
                {
                    file_id = new_file.FILE_ID;
                    return true;
                }
            }
            else
            {
                if (!recordOperation(userId, rep_id, "Create", description))
                {
                    System.Diagnostics.Debug.WriteLine("创建文件操作表操作异常");
                    file_id = "";
                    return false;
                }
                else
                {
                    file_id = new_file.FILE_ID;
                    return true;
                }
            }
        }

        /// <summary>
        /// 创建文件夹（权限检查）
        /// 输入：仓库id，操作者id，文件夹名字，所在位置，父节点是仓库还是文件夹（0是仓库）
        /// 输出：文件夹id，错误信息
        /// 待测试
        /// </summary>
        public bool createFolder(string rep_id, string user_id, string name, string position, int fatherIsFolder,out string fileId, out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            var flag = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == user_id 
                                                             && p.REPOSITORY_ID == rep_id 
                                                             && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).FirstOrDefault();
            if (flag == null)
            {
                var error = new ErrorMessage();
                error.ErrorOperation = "创建文件夹";
                error.ErrorReason = "没有权限创建文件夹";
                error.ErrorTime = DateTime.Now;
                errorMessage = error;
                fileId = "";
                return false;
            }
            else
            {
                string file_id = "";
                if (!createFile(rep_id, name, "1", 0, position, fatherIsFolder, "创建了一个文件夹",out file_id))
                {
                    var error = new ErrorMessage();
                    error.ErrorOperation = "创建文件夹";
                    error.ErrorReason = "创建文件夹失败";
                    error.ErrorTime = DateTime.Now;
                    errorMessage = error;
                    fileId = "";
                    return false;
                } 
                else
                {
                    errorMessage = null;
                    fileId = file_id;
                    return true;
                }
            }
        }

        /// <summary>
        /// 上传文件
        /// 输入：用户id, 仓库id，文件名，文件类型，文件大小，操作描述，当前所在位置，flag在仓库下还是文件夹下(0是仓库，1是文件夹)
        /// 输出：文件id
        /// 待测试
        /// </summary>
        public bool uploadFile(string userid, string rep_id, string name, string type, int size, string description, string position,int flag, out string fileId,out ErrorMessage errorMessage)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                var file_id = "";
                var result = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == userid
                                                                    && p.REPOSITORY_ID == rep_id
                                                                    && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).FirstOrDefault();
                if (!createFile(rep_id, name, "0", size, position, flag, description,out file_id))
                {
                    var error = new ErrorMessage()
                    {
                        ErrorOperation = "创建文件",
                        ErrorReason = "创建文件失败",
                        ErrorTime = DateTime.Now
                    };
                    errorMessage = error;
                    fileId = "";
                    return false;
                }
                else
                {
                    //没有权限，需要给管理员发信息获得许可
                    if (result == null)
                    {
                        var manageArray = new List<USER_REPOSITORY_RELATIONSHIP>();
                        manageArray = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == rep_id
                                                                            && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).ToList();
                        var rep = db.REPOSITORies.Where(p => p.REPOSITORY_ID == rep_id).FirstOrDefault();
                        foreach (var each in manageArray)
                        {
                            addMessageToUser(each.USER_ID, "您所管理的" + rep.NAME + "仓库有上传请求。请问是否同意？");
                        }
                    }
                    fileId = file_id;
                    errorMessage = null;
                    changeUserGrade(userid, 1);
                    return true;
                }
            }
        }

        /// <summary>
        /// 审核请求
        /// 输入：用户id，仓库id，文件id，是否许可(0是许可，1是不许可)
        /// 输出：是否操作成功，错误信息
        /// 待测试
        /// </summary>
        public bool verifyRequest(string userId, string repId,string fileId, int flag,out ErrorMessage errorMessage)
        {
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
                    if (!removeFile(userId,repId, fileId,out errorM))
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

        /*/// <summary>
        /// 显示文件夹中文件
        /// 输入：文件夹ID
        /// 输出：文件信息List
        /// 待测试
        /// </summary>
        public List<FileInfo> showFileInFolder(string fileId, ref string fileName)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<FileInfo> res = null;
                    var file = db.FILE_FILE.Where(p => p.FILE_ID1 == fileId);
                    fileName = (from a in db.FILETABLEs
                                where a.FILE_ID == fileId
                                select a.FILE_NAME).FirstOrDefault();
                    foreach (var str in file)
                    {
                        FileInfo temp = new FileInfo();
                        temp.name = (from s in db.FILETABLEs
                                     where s.FILE_ID == str.FILE_ID2
                                     select s.FILE_NAME).FirstOrDefault();
                        temp.type = (from q in db.FILETABLEs
                                     where q.FILE_ID == str.FILE_ID2
                                     select q.FILE_TYPE).FirstOrDefault();
                        temp.size = (from p in db.FILETABLEs
                                     where p.FILE_ID == str.FILE_ID2
                                     select p.FILE_SIZE).ToString();
                        temp.url = (from a in db.FILETABLEs
                                    where a.FILE_ID == str.FILE_ID2
                                    select a.PATH).FirstOrDefault();
                        res.Add(temp);
                    }
                    return res;
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("文件显示异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }*/

        /// <summary>
        /// 删除文件
        /// 输入：用户id，仓库id，文件id
        /// 输出：是否成功，错误信息
        /// 待测试
        /// </summary>
        public bool removeFile(string userId,string repositoryId,string fileId,out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            var user = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == userId
                                                             && p.REPOSITORY_ID == repositoryId
                                                             && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).FirstOrDefault();
            if (user == null)
            {
                var error = new ErrorMessage()
                {
                    ErrorOperation = "删除文件操作",
                    ErrorReason = "没有权限进行删除操作",
                    ErrorTime = DateTime.Now
                };
                errorMessage = error;
                return false;
            }
            else
            {
                USER_REPOSITORY_OPERATION operation = new USER_REPOSITORY_OPERATION
                {
                    USER_ID = userId,
                    REPOSITORY_ID = repositoryId,
                    OPERATION_DATE = DateTime.Now,
                    OPERATION = "Delete"
                };
                db.USER_REPOSITORY_OPERATION.Add(operation);

                //1级的文件要删除库与文件的关系
                var deep = db.FILETABLEs.Where(t => t.FILE_ID == fileId).FirstOrDefault().FILE_DEEP;
                if (deep == 1)
                {
                    try
                    {
                        db.REPOSITORY_FILE.Remove(db.REPOSITORY_FILE.Where(t => t.FILE_ID == fileId).FirstOrDefault());
                    }
                    catch (Exception ex)
                    {
                        var error = new ErrorMessage
                        {
                            ErrorOperation = "删除文件与仓库的关系操作",
                            ErrorReason = ex.Message,
                            ErrorTime = DateTime.Now
                        };
                        errorMessage = error;
                        return false;
                    }
                }
                //非1级文件要删除文件与文件的关系
                else
                {
                    try
                    {
                        db.FILE_FILE.Remove(db.FILE_FILE.Where(p => p.FILE_ID2 == fileId).FirstOrDefault());
                    }
                    catch (Exception ex)
                    {
                        var error = new ErrorMessage
                        {
                            ErrorOperation = "删除文件与文件的关系操作",
                            ErrorReason = ex.Message,
                            ErrorTime = DateTime.Now
                        };
                        errorMessage = error;
                        return false;
                    }
                }
                
                //删除这个文件下面的文件
                try
                {
                    Queue files = new Queue();
                    files.Enqueue(fileId);
                    while (files.Count > 0)
                    {
                        var cur = files.Dequeue().ToString();
                        var subFiles = db.FILE_FILE.Where(q => q.FILE_ID1 == cur);
                        foreach (var temp in subFiles)
                        {
                            files.Enqueue(temp.FILE_ID2);
                            db.FILE_FILE.Remove(db.FILE_FILE.Where(t => t.FILE_ID1 == cur && t.FILE_ID2 == temp.FILE_ID2).FirstOrDefault());
                        }
                        db.FILETABLEs.Remove(db.FILETABLEs.Where(t => t.FILE_ID == cur).FirstOrDefault());
                    }
                    db.SaveChanges();
                    errorMessage = null;
                    return true;
                }
                catch (Exception ex)
                {
                    var error = new ErrorMessage
                    {
                        ErrorOperation = "删除文件下面的文件操作异常",
                        ErrorReason = ex.Message,
                        ErrorTime = DateTime.Now
                    };
                    errorMessage = error;
                    return false;
                }
            }
        }
    }
}
