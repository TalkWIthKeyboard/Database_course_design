using System;
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
        public List<Database_course_design.Models.ItemModel.OperationItem> showOperationHistory(string userId, DateTime start)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<Database_course_design.Models.ItemModel.OperationItem> Operation = new List<Database_course_design.Models.ItemModel.OperationItem>();
                    Database_course_design.Models.ItemModel.OperationItem res = new ItemModel.OperationItem();
                    var history = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == userId && p.OPERATION_DATE >= start);
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
        /// 输入：评论内容，评论Id,评论日期，仓库id，发布者id，
        /// 输出：布尔类型，判断评论是否成功
        /// </summary>   KUXIANGDBEntities
        public bool addCommentToRepository(Nullable<System.DateTime> CommentData, string content, string RepositoryID, string AnnouncerID)
        {
            COMMENTTABLE newComment = new COMMENTTABLE
            {
                COMMENT_DATE = CommentData,
                CONTENT = content
            };
            USER_COMMENT_REPOSITORY newCommentRepository = new USER_COMMENT_REPOSITORY
            {
                REPOSITORY_ID = RepositoryID,
                ANNOUNCER = AnnouncerID
            };

            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    db.COMMENTTABLEs.Add(newComment);
                    db.SaveChanges();
                    newCommentRepository.COMMENT_ID = newComment.COMMENT_ID;
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
        ///对仓库进行评论
        /// 输入：评论内容，评论Id,Y用户id，评论日期，仓库id，发布者id，
        /// 输出：布尔类型，判断评论是否成功
        /// </summary>  
        public bool addCommentToUser(Nullable<System.DateTime> CommentData, string content, string UserId, string AnnouncerID)
        {


            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    COMMENTTABLE newComment = new COMMENTTABLE
                    {
                        COMMENT_DATE = CommentData,
                        CONTENT = content
                    };
                    USER_COMMENT_USER newCommentUser = new USER_COMMENT_USER
                    {
                        USER_ID = UserId,
                        ANNOUNCER = AnnouncerID
                    };
                    db.COMMENTTABLEs.Add(newComment);
                    db.SaveChanges();
                    newCommentUser.COMMENT_ID = newComment.COMMENT_ID;
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

        ///张志强
        /// <summary>
        /// 邀请管理者
        /// 输入:用户ID
        /// 输出：返回布尔类型，确定是否添加成功
        /// 待测试
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
        /// 待测试

        public bool DletePartner(string UserID, string RepositoryID)
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
        /// 输入：仓库描述des，仓库id,仓库权限，仓库
        /// 输出：返回布尔类型，确定是否添加描述成功
        /// 待测试
        /// /// </summary>
        public bool ModifyInformation(string RepositoryId, string newDes, string newName, string newLabel1, string newLabel2, string newLabel3)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    REPOSITORY oldrepository = db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepositoryId).FirstOrDefault();
                    oldrepository.DESCRIPTION = newDes;//新加属性（description）
                    oldrepository.NAME = newName;
                    oldrepository.LABEL1 = newLabel1;
                    oldrepository.LABEL2 = newLabel2;
                    oldrepository.LABEL3 = newLabel3;
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
        /// 显示仓库文件
        /// 输入：仓库ID
        /// 输出：文件名,文件类型，文件大小
        /// 待测试
        /// </summary>
        public struct FileInfo
        {
            public string name;
            public string type;
            public string size;
        }
        public List<FileInfo> showFile(string resipositoryId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<FileInfo> res = null;
                    var file = db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == resipositoryId);
                    foreach (var str in file)
                    {
                        FileInfo temp = new FileInfo();
                        temp.name = (from s in db.FILETABLEs
                                     where s.FILE_ID == str.FILE_ID
                                     select s.FILE_NAME).FirstOrDefault();
                        temp.type = (from q in db.FILETABLEs
                                     where q.FILE_ID == str.FILE_ID
                                     select q.FILE_TYPE).FirstOrDefault();
                        temp.size = (from p in db.FILETABLEs
                                     where p.FILE_ID == str.FILE_ID
                                     select p.FILE_SIZE).ToString();
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
        /// 显示关注者动态
        /// 输入：用户ID
        /// 输出：被关注者信息
        /// 待测试
        /// </summary>
        public anctionInfo showFollowers(string userId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    anctionInfo res = new anctionInfo();
                    DateTime latest_temp = new DateTime(0, 0, 0);
                    var followers = db.USER_USER.Where(p => p.USER_ID1 == userId);
                    foreach (var str in followers)
                    {
                        anctionInfo temp = new anctionInfo();
                        temp.UserName = (from s in db.USERTABLEs
                                         where s.USER_ID == str.USER_ID2
                                         select s.USER_NAME).FirstOrDefault();
                        temp.UserUrl = (from q in db.USERTABLEs
                                        where q.USER_ID == str.USER_ID2
                                        select q.USER_URL).FirstOrDefault();             
                        temp.UserPhotoUrl = (from a in db.USERTABLEs
                                             where a.USER_ID == str.USER_ID2
                                             select a.IMAGE).FirstOrDefault();
                        var latest = (from b in db.USER_REPOSITORY_OPERATION
                                      where b.USER_ID == str.USER_ID2
                                      select b.OPERATION_DATE).Max();
                        string latest_repertory = (from c in db.USER_REPOSITORY_OPERATION
                                                   where c.USER_ID == str.USER_ID2 && c.OPERATION_DATE == latest
                                                   select c.REPOSITORY_ID).FirstOrDefault();
                        temp.UserOperation = (from c in db.USER_REPOSITORY_OPERATION
                                              where c.USER_ID == str.USER_ID2 && c.OPERATION_DATE == latest
                                              select c.OPERATION).FirstOrDefault();
                        temp.RepertoryName = (from d in db.REPOSITORies
                                              where d.REPOSITORY_ID == latest_repertory
                                              select d.NAME).FirstOrDefault();
                        temp.RepertoryUrl = (from d in db.REPOSITORies
                                             where d.REPOSITORY_ID == latest_repertory
                                             select d.URL).FirstOrDefault();
                        temp.UpdateInfo = (from e in db.USER_REPOSITORY_OPERATION
                                           where e.USER_ID == str.USER_ID2 && e.OPERATION_DATE == latest
                                           select e.DESCRIPTION).FirstOrDefault();
                        if (DateTime.Compare(latest, latest_temp) > 0)
                        {
                            latest_temp = latest;
                            res = temp;
                        }
                    }
                    return res;
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("更新显示异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 创建仓库
        /// 输入：用户名，库的名称，创建库的公私级别
        /// 输出：创建成功与否
        /// 待测试
        /// </summary>
        public bool CreateRepository(string userid, string name, int authority,string description)
        {
            
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    String rname;
                    if (name != null)
                        rname = name;
                    else
                    {
                        String timeStr = DateTime.Now.Ticks.ToString();
                        String time = timeStr.Substring(timeStr.Length - 10, 10);
                        rname = userid.Substring(userid.Length - 10, 10) + "_" + time;
                    }
                    string repositoryid = createNewId("REPOSITORY");
                    short attri = 0;
                    short iscreate = 1;
                    String forkfrom = "";
                    short relationship = 0;
                    REPOSITORY newRep = new REPOSITORY
                    {
                        REPOSITORY_ID = repositoryid,
                        NAME = rname,
                        AUTHORITY = authority,
                        ATTRIBUTE = attri,
                        IS_CREATE = iscreate,
                        FORK_FROM = forkfrom,
                        DESCRIPTION = description
                    };

                    USER_REPOSITORY_RELATIONSHIP userrepr = new USER_REPOSITORY_RELATIONSHIP
                    {
                        USER_ID = userid,
                        REPOSITORY_ID = newRep.REPOSITORY_ID,
                        RELATIONSHIP = relationship
                    };

                    db.REPOSITORies.Add(newRep);
                    db.USER_REPOSITORY_RELATIONSHIP.Add(userrepr);
                    db.SaveChanges();
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
        public bool RemoveRepository(string userid, string repositoryid)
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
                        db.REPOSITORies.Remove(db.REPOSITORies.Where(p =>

p.REPOSITORY_ID == repositoryid).FirstOrDefault());
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("仓库删除失败！");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                
                return false;
            }
        }



  /// <summary>
        /// 修改文件状态
        /// 输入：文件id
        /// 输出：修改状态是否成功
        /// 待测试
        /// </summary>
        public bool ModifyFileState(string fileid)
        {
            using (var db = new KUXIANGDBEntities())
            {  
                try
                {
                    var mfile =
                   from p
                   in db.FILETABLEs
                   where p.FILE_ID == fileid
                   select p;
                    foreach (var mf in mfile)
                    {
                        mf.FILE_STATE = 1;
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("修改文件状态失败！");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
          
            }
        }
        ///<summary>
        ///留言板显示
        ///输入： 接收者的编号
        ///输出：评论队列
        ///待测试
        ///</summary> 
        public List <COMMENTTABLE > showUserComment(string userid)
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
                    return comment.ToList ();
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
        ///待测试
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
        /// 检索功能--文件
        /// 输入：要检索的文件的名称
        /// 输出：检索出的文件ID列表
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
        /// </summary>
        public List<USERTABLE > searchUser(string UserName)
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
        /// 输入：无
        /// 输出：检索出的仓库列表
        /// 待测试
        /// </summary>
        public List<REPOSITORY > recommendRepositoryByHeat()
        {
          
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<REPOSITORY> heatrepository = new List<REPOSITORY>();
                    int forkfigure = 20;
                    int starfigure = 50;
                    var temps =
                        (from s in db.REPOSITORies
                         where s.FORK_NUM >= forkfigure || s.STAR_NUM >= starfigure
                         orderby s.FORK_NUM descending,s.STAR_NUM descending             
                         select s);
                    foreach (var temp in temps)
                    { 
                        heatrepository.Add(temp);
                    }
                    return heatrepository;
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

        /// <summary>
        /// 下载头像
        /// 输入：用户id
        /// 输出：头像图片url
        /// 待测试
        /// </summary>
        public string DownloadImage(string userid)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    string url = "";
                    url = (from s in db.USERTABLEs
                           where s.USER_ID == userid
                           select s.USER_ID).FirstOrDefault();
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


    }
}
