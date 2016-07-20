using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.ItemModel;
using System.Collections;

namespace Database_course_design.Models.WorkModel
{
    public class AboutRepository
    {
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.获取一个仓库的管理者人数
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
        /// 2.显示仓库文件
        /// 输入：仓库ID
        /// 输出：文件名,文件类型，文件大小
        /// 测试成功
        /// </summary>
        public List<FileInfo> showFile(string resipositoryId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    List<FileInfo> res = new List<FileInfo>();
                    var rep = db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == resipositoryId);
                    foreach (var each in rep)
                    {
                        var f = db.FILETABLEs.Where(p => p.FILE_ID == each.FILE_ID).FirstOrDefault();
                        var resipositoryName = db.REPOSITORies.Where(p => p.REPOSITORY_ID == resipositoryId).FirstOrDefault().NAME;
                        FileInfo temp = new FileInfo(f,resipositoryName);
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
        /// 3.添加浏览历史
        /// 输入：用户的id，仓库的id
        /// 输出：操作是否成功
        /// 待测试
        /// </summary>
        public bool recordLookHistory(string userId, string repositoryId)
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

        /// <summary>
        /// 4.操作历史显示
        /// 输入：用户的编号
        /// 输出： 操作历史(是否要链接）
        /// 测试成功
        /// </summary>
        public List<OperationItem> showOperationHistory(string userId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                try
                {
                    var Operation = new List<OperationItem>();
                    var res = new OperationItem();
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

        /// <summary>
        /// 5.邀请管理者
        /// 输入: 用户ID，仓库ID
        /// 输出：返回布尔类型，确定是否添加成功
        /// 待测试
        /// </summary>
        public bool addPartner(string userId, string partnerId, string repositoryId,out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            try
            {
                int? authority = basic.auditAuthority(userId, repositoryId);
                if (authority == 0 || authority == 1)
                {
                    USER_REPOSITORY_RELATIONSHIP partner = new USER_REPOSITORY_RELATIONSHIP
                    {
                        USER_ID = userId,
                        REPOSITORY_ID = repositoryId,
                        RELATIONSHIP = 1
                    };
                    db.USER_REPOSITORY_RELATIONSHIP.Add(partner);
                    db.SaveChanges();
                    errorMessage = null;
                    return true;
                }
                else
                {
                    var error = new ErrorMessage("邀请管理者操作", "没有权限邀请管理者");
                    errorMessage = error;
                    return false;
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorMessage("邀请管理者操作异常", ex.Message);
                errorMessage = error;
                return false;
            }
        }

        /// <summary>
        /// 6.删除管理者
        /// 输入：用户ID
        /// 输出：返回布尔类型，确定是否删除成功
        /// 待测试
        /// </summary>
        public bool deletPartner(string userId, string partnerId, string repositoryId,out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();    
            try
            {
                if (basic.auditAuthority(userId,repositoryId) == 0)
                {
                    db.USER_REPOSITORY_RELATIONSHIP.Remove(db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.USER_ID == userId && 
                                                                                                 p.REPOSITORY_ID == repositoryId).FirstOrDefault());
                    db.SaveChanges();
                    errorMessage = null;
                    return true;
                }
                else
                {
                    var error = new ErrorMessage("删除管理者操作异常", "没有权限删除管理者");
                    errorMessage = error;
                    return false;
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorMessage("删除管理者操作异常", ex.Message);
                errorMessage = error;
                return false;
            }

        }

        /// <summary>
        /// 7.修改仓库信息
        /// 输入：仓库描述des，仓库id
        /// 输出：返回布尔类型，确定是否添加描述成功
        /// 待测试
        /// </summary>
        public bool modifyInformation(string userId,string repositoryId, string newDes,out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            try
            {
                var authority = basic.auditAuthority(userId, repositoryId);
                if (authority == 0 || authority == 1)
                {
                    REPOSITORY oldrepository = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryId).FirstOrDefault();
                    if (newDes != "")
                    {
                        oldrepository.DESCRIPTION = newDes;
                    }
                    db.SaveChanges();
                    errorMessage = null;
                    return true;
                }
                else
                {
                    var error = new ErrorMessage("修改仓库信息异常", "没有权限修改仓库信息");
                    errorMessage = error;
                    return false;
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorMessage("修改仓库信息异常", ex.Message);
                errorMessage = error;
                return false;
            }
        }

        /// <summary>
        /// 8.创建仓库
        /// 输入：用户名，库的名称，创建库的公私级别，库的描述，课程的名字
        /// 输出：创建成功与否
        /// 测试成功
        /// </summary>
        public bool CreateRepository(string userid, string name, int authority, string description, string label3)
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
                            COURSE_ID = basic.createNewId("COURSE")
                        };
                        db.COURSEs.Add(newCourse);
                        oldCourse = newCourse;
                    }

                    string repositoryid = basic.createNewId("REPOSITORY");
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

                    basic.recordOperation(userid, repositoryid, "Create", description);
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
        /// 9.删除仓库
        /// 输入：用户ID，仓库ID
        /// 输出：删除成功与否
        /// 测试成功
        /// </summary>
        public bool RemoveRepository(string userid, string repositoryid, out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            var fileOp = new AboutFile();
            try
            {
                if (basic.auditAuthority(userid,repositoryid) == 0)
                {
                    if (!basic.recordOperation(userid, repositoryid, "Delete", null))
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

                    errorMessage = null;
                    var fileList = db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == repositoryid).ToList();
                    foreach (var each in fileList)
                    {
                        var error = new ErrorMessage();
                        fileOp.removeFile(userid, repositoryid, each.FILE_ID, out error);
                        if (error != null)
                        {
                            errorMessage = error;
                        }
                    }
                    db.REPOSITORies.Remove(db.REPOSITORies.Where(p => p.REPOSITORY_ID == repositoryid).FirstOrDefault());
                    db.SaveChanges();
                    return true;
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

        /// <summary>
        /// 10.改变用户对仓库的star状态
        /// 输入：用户id，仓库id
        /// 输出：是否成功，错误信息
        /// 待测试
        /// </summary>
        public bool changeStar(string userId, string repositoryId, out ErrorMessage errorMessage)
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
                    if (basic.recordOperation(userId, repositoryId, "Unstar", username + "取消收藏了" + repname))
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
                    if (basic.recordOperation(userId, repositoryId, "Star", username + "收藏了" + repname))
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

        /// <summary>
        /// 11.添加副本仓库
        /// 输入：用户id，被添加副本的仓库id
        /// 输出：副本仓库
        /// 多级文件部分未测试
        /// </summary>
        public REPOSITORY addForkRepertory(string UserId, string RepositoryId)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                var fileOp = new AboutFile();
                try
                {
                    REPOSITORY origin = db.REPOSITORies.Where(p => p.REPOSITORY_ID == RepositoryId).FirstOrDefault();
                    var ofile = db.REPOSITORY_FILE.Where(a => a.REPOSITORY_ID == RepositoryId);
                    string new_id = basic.createNewId("REPOSITORY");

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
                        ATTRIBUTE = 0,
                        AUTHORITY = origin.AUTHORITY,
                        FORK_NUM = 0,                                                                                                   //一致性?
                        STAR_NUM = 0,
                        FORK_FROM = RepositoryId,
                        IS_CREATE = 0,
                        COURSE_ID = origin.COURSE_ID,
                        UPDATE_DATE = System.DateTime.Now
                    };
                    USER_REPOSITORY_RELATIONSHIP relationship = new USER_REPOSITORY_RELATIONSHIP
                    {
                        REPOSITORY_ID = new_id,
                        USER_ID = UserId,
                        RELATIONSHIP = 0
                    };
                    //db.SaveChanges();
                    db.REPOSITORies.Add(reposit);
                    db.USER_REPOSITORY_OPERATION.Add(operation);
                    db.USER_REPOSITORY_RELATIONSHIP.Add(relationship);
                    db.SaveChanges();

                    var newfile = ofile;
                    Queue files = new Queue();
                    Dictionary<string, string> old_new = new Dictionary<string, string>();
                    foreach (var temp in newfile)
                    {
                        var file = db.FILETABLEs.Where(p => p.FILE_ID == temp.FILE_ID).FirstOrDefault();
                        string new_file_id = basic.createNewId("FILETABLE");
                        old_new.Add(file.FILE_ID, new_file_id);
                        fileOp.copyFile(file.FILE_ID, new_file_id, UserId, RepositoryId);

                        var file_repository = new REPOSITORY_FILE
                        {
                            REPOSITORY_ID = new_id,
                            FILE_ID = new_file_id,
                            REPOSITORY_FILE_ID = "NFK"
                        };
                        db.REPOSITORY_FILE.Add(file_repository);
                        files.Enqueue(file.FILE_ID);
                        //db.SaveChanges();
                    }
                    while (files.Count > 0)
                    {
                        string cur = (files.Dequeue()).ToString();
                        var cur_file = db.FILE_FILE.Where(p => p.FILE_ID1 == cur);
                        foreach (var temp2 in cur_file)
                        {
                            var file = db.FILETABLEs.Where(p => p.FILE_ID == temp2.FILE_ID2).FirstOrDefault();
                            string new_file_id2 = basic.createNewId("FILETABLE");
                            old_new.Add(file.FILE_ID, new_file_id2);
                            fileOp.copyFile(file.FILE_ID, new_file_id2, UserId, RepositoryId);
                            FILE_FILE file_file = new FILE_FILE
                            {
                                FILE_ID1 = old_new[temp2.FILE_ID1],
                                FILE_ID2 = new_file_id2,
                                FILE_FILE_ID = "file_file"
                            };
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
        /// 12.显示文件夹中文件
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
                    foreach (var str in file)
                    {
                        var f = db.FILETABLEs.Where(p => p.FILE_ID == str.FILE_ID2).FirstOrDefault();
                        FileInfo temp = new FileInfo(f);
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
        /// 13.挑选官方库
        /// 输入：3级标签
        /// 输出：是否操作成功
        /// 待测试
        /// </summary>
        public bool chooseOfficialRepository(string label1,string label2,string label3)
        {
            var db = new KUXIANGDBEntities();
            try
            {
                var reps = db.REPOSITORies.Where(p => p.COURSE.LABEL1 == label1
                                             && p.COURSE.LABEL2 == label2
                                             && p.COURSE.LABEL3 == label3).ToList();
                double? max = 0;
                int? l = -1;
                int num = 0;
                foreach (var each in reps)
                {
                    int? iden = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == each.REPOSITORY_ID).FirstOrDefault().USERTABLE.IDENTITY;
                    if ((each.STAR_NUM + each.FORK_NUM) * (1 + 0.5 * iden) > max)
                    {
                        max = (each.STAR_NUM + each.FORK_NUM) * (1 + 0.5 * iden);
                        l = num;
                    }
                    num++;
                }
                reps[num].ATTRIBUTE = 1;
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("挑选官方库操作失败");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}