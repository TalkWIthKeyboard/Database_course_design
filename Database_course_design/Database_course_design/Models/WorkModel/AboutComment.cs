using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.WorkModel
{
    public class AboutComment
    {
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.创建评论
        /// 输入：评论内容
        /// 输出：是否操作成功，评论id
        /// 待测试
        /// </summary>
        public bool createComment(string content,out string comment_id)
        {
            var db = new KUXIANGDBEntities();
            try
            {
                var newComment = new COMMENTTABLE
                {
                    COMMENT_ID = basic.createNewId("COMMENTTABLE"),
                    COMMENT_DATE = DateTime.Now,
                    CONTENT = content
                };
                db.SaveChanges();
                comment_id = newComment.COMMENT_ID;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("创建评论异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                comment_id = null;
                return false;
            }
        }

        /// <summary>
        /// 2.对用户进行评论
        /// 输入：评论内容，被评论者id，发布者id，
        /// 输出：布尔类型，判断评论是否成功
        /// 测试成功
        /// </summary>  
        public bool addCommentToUser(string content, string UserId, string AnnouncerID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    string id = "";
                    if (createComment(content,out id))
                    {
                        USER_COMMENT_USER newCommentUser = new USER_COMMENT_USER
                        {
                            COMMENT_ID = id,
                            USER_ID = UserId,
                            ANNOUNCER = AnnouncerID
                        };
                        db.USER_COMMENT_USER.Add(newCommentUser);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("创建用户评论用户关系异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 3.对仓库进行评论
        /// 输入：评论内容，评论日期，仓库id，发布者id，
        /// 输出：布尔类型，判断评论是否成功
        /// </summary>   KUXIANGDBEntities
        public bool addCommentToRepository(string content, string RepositoryID, string AnnouncerID)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    string id = "";
                    if (createComment(content,out id))
                    {
                        USER_COMMENT_REPOSITORY newCommentRepository = new USER_COMMENT_REPOSITORY
                        {
                            COMMENT_ID = id,
                            REPOSITORY_ID = RepositoryID,
                            ANNOUNCER = AnnouncerID
                        };
                        db.USER_COMMENT_REPOSITORY.Add(newCommentRepository);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("创建用户评论仓库关系异常");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 4.留言板显示
        /// 输入：接收者的编号
        /// 输出：评论队列
        /// 测试成功
        /// </summary>
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
                    System.Diagnostics.Debug.WriteLine("留言板显示失败！");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 5.仓库留言板显示
        /// 输入：仓库的编号
        /// 输出：评论队列
        /// 测试成功
        /// </summary>
        public List<COMMENTTABLE> showRepositoryComment(string repId)
        {
            var db = new KUXIANGDBEntities();
            var comments = new List<COMMENTTABLE>();
            try
            {
                var commentId = db.USER_COMMENT_REPOSITORY.Where(p => p.REPOSITORY_ID == repId).ToList();
                foreach(var each in commentId)
                {
                    var comment = db.COMMENTTABLEs.Where(p => p.COMMENT_ID == each.COMMENT_ID).FirstOrDefault();
                    comments.Add(comment);
                }
                return comments;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("显示仓库的留言板操作异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}