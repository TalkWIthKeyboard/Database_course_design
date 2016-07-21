using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models.ItemModel;

namespace Database_course_design.Models.WorkModel
{
	public class AboutFile
	{
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.拷贝文件
        /// 输入：旧文件id，新文件id，用户id, 旧仓库id
        /// 输出：拷贝是否成功
        /// 测试成功
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
                    //string fullName = old_file.FILE_NAME + "." + old_file.FILE_TYPE;
                    //string sourcePath = @old_file.PATH;
                    string new_path = (old_file.PATH).Replace(old_user_id, userId);
                    string targetPath = @new_path;                    //debug
                    //string sourceFile = System.IO.Path.Combine(sourcePath, fullName);
                    //string destFile = System.IO.Path.Combine(targetPath, fullName);
                    //if (!System.IO.Directory.Exists(targetPath))
                    //{
                    //    System.IO.Directory.CreateDirectory(targetPath);
                    //}
                    //System.IO.File.Copy(sourceFile, destFile, true);

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
        /// 2.创建文件或者文件夹（type为0是文件，1是文件夹）
        /// 输入：仓库id，文件（夹）名字，文件类型（是否为文件夹），文件大小，所在位置，父节点是仓库还是文件夹（0是仓库），操作描述
        /// 输出：是否创建成功，文件id
        /// 测试成功
        /// </summary>
        public bool createFile(string rep_id, string name, string type, int size, string position, int flag, string description, out string file_id)
        {
            var db = new KUXIANGDBEntities();
            ErrorMessage errorMessage = new ErrorMessage();
            if (!verifyDuplicateName(name, position, flag, out errorMessage))
            {
                file_id = null;
                return false;
            }
            string newFileId = basic.createNewId("FILETABLE");
            string newPath = "";
            string userId = null;
            int? deepInt = 0;
            try
            {
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
                userId = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == rep_id).FirstOrDefault().USER_ID;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("文件所在位置异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                file_id = null;
                return false;
            }

            FILETABLE new_file = new FILETABLE
            {
                FILE_ID = newFileId,
                PATH = newPath + @"/" + name,
                FILE_NAME = name,
                FILE_TYPE = type,
                FILE_STATE = 0,
                FILE_SIZE = size,
                FILE_DEEP = deepInt,
                UPDATE_DATE = System.DateTime.Now
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
                    REPOSITORY_FILE_ID = "r_f_id"
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

            if (type == "0")
            {
                if (!basic.recordOperation(userId, rep_id, "Upload", description))
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
                if (!basic.recordOperation(userId, rep_id, "Create", description))
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
        /// 3.创建文件夹（权限检查）
        /// 输入：仓库id，操作者id，文件夹名字，所在位置，父节点是仓库还是文件夹（0是仓库）
        /// 输出：文件夹id，错误信息
        /// 测试成功
        /// </summary>
        public bool createFolder(string rep_id, string user_id, string name, string position, int fatherIsFolder, out string fileId, out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            var basicModel = new BasicModel();
            var auth = basicModel.auditAuthority(user_id, rep_id);
            if (auth != 0 && auth != 1)
            {
                var error = new ErrorMessage("创建文件夹", "没有权限创建文件夹");
                errorMessage = error;
                fileId = "";
                return false;
            }
            else
            {
                string file_id = "";
                if (!createFile(rep_id, name, "1", 0, position, fatherIsFolder, "创建了一个文件夹", out file_id))
                {
                    var error = new ErrorMessage("创建文件夹", "创建文件夹失败");
                    errorMessage = error;
                    fileId = "";
                    return false;
                }
                else
                {
                    var rep = db.REPOSITORies.Where(p => p.REPOSITORY_ID == rep_id).FirstOrDefault();
                    rep.UPDATE_DATE = DateTime.Now;
                    db.SaveChanges();
                    errorMessage = null;
                    fileId = file_id;
                    return true;
                }
            }
        }

        /// <summary>
        /// 4.上传文件
        /// 输入：用户id, 仓库id，文件名，文件类型，文件大小，操作描述，当前所在位置，flag在仓库下还是文件夹下(0是仓库，1是文件夹)
        /// 输出：文件id
        /// 测试成功
        /// </summary>
        public bool uploadFile(string userid, string rep_id, string name, string type, int size, string description, string position, int flag, out string fileId, out ErrorMessage errorMessage)
        {
            using (KUXIANGDBEntities db = new KUXIANGDBEntities())
            {
                var file_id = "";
                BasicModel basicModel = new BasicModel();
                var auth = basicModel.auditAuthority(userid, rep_id);
                if (!createFile(rep_id, name, "0", size, position, flag, description, out file_id))
                {
                    var error = new ErrorMessage("创建文件", "创建文件失败");
                    errorMessage = error;
                    fileId = "";
                    return false;
                }
                else
                {
                    //没有权限，需要给管理员发信息获得许可
                    if (auth != 0 && auth != 1)
                    {
                        var manageArray = new List<USER_REPOSITORY_RELATIONSHIP>();
                        manageArray = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == rep_id
                                                                            && (p.RELATIONSHIP == 0 || p.RELATIONSHIP == 1)).ToList();
                        var rep = db.REPOSITORies.Where(p => p.REPOSITORY_ID == rep_id).FirstOrDefault();
                        foreach (var each in manageArray)
                        {
                            var message = new AboutMessage();
                            message.addMessageToUser(each.USER_ID, "2\n您所管理的" + rep.NAME + "仓库有上传请求。请问是否同意？\n" + userid + "\n" + rep_id + "\n" + file_id);
                        }
                    }
                    fileId = file_id;
                    errorMessage = null;
                    var user = new AboutUser();
                    user.changeUserGrade(userid, 1);
                    return true;
                }
            }
        }

        /// <summary>
        /// 5.删除文件
        /// 输入：仓库id，文件id
        /// 输出：bool(是否删除成功)
        /// 测试成功
        /// </summary>
        public bool removeFile(string userId, string repositoryId, string fileId, out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            BasicModel basicModel = new BasicModel();
            var auth = basicModel.auditAuthority(userId, repositoryId);
            if (auth != 0 && auth != 1)
            {
                var error = new ErrorMessage("删除文件操作", "没有权限进行删除操作");
                errorMessage = error;
                return false;
            }
            else
            {
                try
                {
                    //更新操作
                    USER_REPOSITORY_OPERATION operation = new USER_REPOSITORY_OPERATION
                    {
                        USER_ID = userId,
                        REPOSITORY_ID = repositoryId,
                        OPERATION_DATE = DateTime.Now,
                        OPERATION = "Delete"
                    };
                    db.USER_REPOSITORY_OPERATION.Add(operation);
                    Queue files = new Queue();
                    files.Enqueue(fileId);
                    var belongs = db.FILE_FILE.Where(t => t.FILE_ID2 == fileId).FirstOrDefault();
                    while (files.Count > 0)
                    {
                        var cur = files.Dequeue().ToString();
                        var subFiles = db.FILE_FILE.Where(q => q.FILE_ID1 == cur);
                        foreach (var temp in subFiles)
                        {
                            files.Enqueue(temp.FILE_ID2);
                        }
                        db.FILETABLEs.Remove(db.FILETABLEs.Where(p => p.FILE_ID == cur).FirstOrDefault());
                    }
                    db.SaveChanges();
                    errorMessage = null;
                    return true;
                }
                catch (Exception ex)
                {
                    var error = new ErrorMessage("删除文件下面的文件操作异常", ex.Message);
                    errorMessage = error;
                    return false;
                }
            }
        }

        /// <summary>
        /// 6.判断文件是否重名
        /// 输入：文件名，父节点的id，文件是在文件夹中还是仓库中（flag=0为文件夹，1为仓库）
        /// 输出：是否重名
        /// 待测试
        /// </summary>
        public bool verifyDuplicateName(string fileName, string fatherId, int flag,out ErrorMessage errorMessage)
        {
            var db = new KUXIANGDBEntities();
            var fileFile = new List<String>();
            try
            {
                if (flag == 0)
                {
                    fileFile = db.FILE_FILE.Where(p => p.FILE_ID1 == fatherId).Select(p => p.FILE_ID2).ToList();
                }
                else
                {
                    fileFile = db.REPOSITORY_FILE.Where(p => p.REPOSITORY_ID == fatherId).Select(p => p.FILE_ID).ToList();
                }
                foreach (var each in fileFile)
                {
                    var name = db.FILETABLEs.Where(p => p.FILE_ID == each).FirstOrDefault().FILE_NAME;
                    if (name == fileName)
                    {
                        var error = new ErrorMessage("检查文件（夹）重名操作","存在重名");
                        errorMessage = error;
                        return false;
                    }
                }
                errorMessage = null;
                return true;
            }
            catch(Exception ex)
            {
                var error = new ErrorMessage("检查文件（夹）重名操作异常", ex.Message);
                errorMessage = error;
                return false;
            }
        }
    }
}