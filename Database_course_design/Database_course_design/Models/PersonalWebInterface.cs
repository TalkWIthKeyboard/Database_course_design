using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models
{
    public class PersonalWebInterface
    {
        /// <summary>
        /// 修改用户资料（对DBmodel的封装）
        /// 输入：/*待定参数*/， 错误信息
        /// 输出：是否成功
        /// </summary>
        public bool changeUserInfo( /*待定参数*/ out ItemModel.ErrorMessage)
        {

        }

        /// <summary>
        /// 修改用户好友关系
        /// 输入：用户自己的Id, 对象用户的Id, 关注或取消关注（true是关注）, 错误信息
        /// 输出：是否成功
        /// </summary>
        public bool changeFriend(string _SelfUserId, string _TarUserId, bool Focus,out ItemModel.ErrorMessage ErrorInfo)
        {

        }

        /// <summary>
        /// 查询用户该月每天的操作和热度
        /// 输入：用户Id, 返回列表， 错误信息
        /// 输出 ：是否成功
        /// 待修改
        /// 待测试
        /// </summary>
        public bool getUserHeat(string _UserId, out List<ItemModel.DayHeat> DayHeatResult, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                //实例化数据库
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                DBModel func = new DBModel();
                //拿到本月该用户所有的操作
                var OpList = func.showOperationHistory(_UserId, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                //创建返回列表
                List<Database_course_design.Models.ItemModel.DayHeat> ret = new List<ItemModel.DayHeat>();
                //插入本月的天数
                for (int i = 1; i <= DateTime.Now.Day; i++)
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
                return ret;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("用户分数修改异常");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}