using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Auth;
using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RS;
using Qiniu.Conf;
using Qiniu.RPC;

namespace Database_course_design.Models.ScriptModel
{
    public class QiNiuOperation
    {
        /// <summary>
        /// 1.上传文件
        /// 输入：文件本地地址，服务器地址
        /// 输出：是否操作成功
        /// </summary>
        public bool upload(string localPath,string realPath)
        {
            Qiniu.Conf.Config.ACCESS_KEY = "PTvRc8G0vxeyXEfUp1ZSEbZOzpKwmewIB_Uvm1BG";
            Qiniu.Conf.Config.SECRET_KEY = "HmGDEvKbHg1wIAgLePeff48p1vBMKw4kMOxJi75Q";
            IOClient target = new IOClient();
            PutExtra extra = new PutExtra();          
            try
            {
                var bucket = "filemanage";
                var key = realPath;
                PutPolicy put = new PutPolicy(bucket,3600);
                string upToken = put.Token();
                String filePath = localPath;
                PutRet ret = target.PutFile(upToken, key, filePath, extra);
                //打印出相应的信息
                Console.WriteLine(ret.Response.ToString());
                Console.WriteLine(ret.key);
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("上传文件到七牛");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
        
        /// <summary>
        /// 2.下载文件
        /// 输入：服务器地址
        /// 输出：无
        /// </summary>
        public string download(string filePath)
        {
            Qiniu.Conf.Config.ACCESS_KEY = "PTvRc8G0vxeyXEfUp1ZSEbZOzpKwmewIB_Uvm1BG";
            Qiniu.Conf.Config.SECRET_KEY = "HmGDEvKbHg1wIAgLePeff48p1vBMKw4kMOxJi75Q";
            try
            {
                string private_url = "http://o8efk558i.bkt.clouddn.com/" + filePath + "?attname=";
                Console.WriteLine(private_url);
                return private_url;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("下载文件删除异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 3.删除文件
        /// 输入：服务器地址
        /// 输出：是否操作成功
        /// </summary>
        public bool delete(string filePath)
        {
            Qiniu.Conf.Config.ACCESS_KEY = "PTvRc8G0vxeyXEfUp1ZSEbZOzpKwmewIB_Uvm1BG";
            Qiniu.Conf.Config.SECRET_KEY = "HmGDEvKbHg1wIAgLePeff48p1vBMKw4kMOxJi75Q";
            try
            {
                RSClient client = new RSClient();
                CallRet ret = client.Delete(new EntryPath("filemanage", filePath));
                if (ret.OK)
                {
                    Console.WriteLine("Delete OK");
                }
                else
                {
                    return false;
                    Console.WriteLine("Failed to delete");
                }
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("删除文件操作异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 4.覆盖上传
        /// 输入：文件本地地址，服务器保存地址
        /// 输出：无
        /// </summary>
        public void reUpload(string localPath,string realPath)
        {
            Qiniu.Conf.Config.ACCESS_KEY = "PTvRc8G0vxeyXEfUp1ZSEbZOzpKwmewIB_Uvm1BG";
            Qiniu.Conf.Config.SECRET_KEY = "HmGDEvKbHg1wIAgLePeff48p1vBMKw4kMOxJi75Q";
            IOClient target = new IOClient();
            PutExtra extra = new PutExtra();

            string bucket = "filemanage";
            string key = realPath;

            delete(realPath);
            upload(localPath, realPath);
        }
    }
}