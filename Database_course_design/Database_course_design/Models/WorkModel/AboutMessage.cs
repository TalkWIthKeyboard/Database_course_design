using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.WorkModel
{
    public class AboutMessage
    {
        public BasicModel basic = new BasicModel();

        /// <summary>
        /// 1.创建系统消息
        /// 输入: 用户ID，消息内容
        /// 输出：返回布尔类型，确定是否创建成功
        /// 测试成功
        /// </summary>
        public bool addMessageToUser(string userid, string content)
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
                    newMessage.MESSAGE_ID = basic.createNewId("MESSAGE");
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

        /// <summary>
        /// 2.消息推送
        /// 输入：接收者的编号
        /// 输出：消息队列
        /// 测试成功
        /// </summary>
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

        /// <summary>
        /// 3.阅读消息
        /// 输入：消息id
        /// 输出：是否操作成功
        /// </summary>
        public bool readMessage(string messageId)
        {
            var db = new KUXIANGDBEntities();
            try
            {
                var message = db.MESSAGEs.Where(p => p.MESSAGE_ID == messageId).FirstOrDefault();
                if (message != null)
                {
                    message.IS_READ = 1;
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
                System.Diagnostics.Debug.WriteLine("阅读消息操作异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}