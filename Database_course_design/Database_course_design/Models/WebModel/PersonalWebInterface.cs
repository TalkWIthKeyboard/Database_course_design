﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models
{
    public class PersonalWebInterface
    {

        /// <summary>
        /// 获取用户自己的动态
        /// 输入：用户的id, 仓库id（null时为获取所有的动态）， 要返回的动态列表, 错误的信息
        /// 输出：是否成功
        /// 完成测试
        /// </summary>
        public bool getSelfDynamic(string _UserId, string _RepoId, out List<ItemModel.actionInfo> SearchResul, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                bool flag = false;
                DBModel dbmodel = new DBModel();
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                SearchResul = new List<ItemModel.actionInfo>();
                if (_RepoId == null)
                {
                    var result = dbmodel.getUserDynamics(_UserId);
                    if (result != null)
                    {
                        flag = true;
                        SearchResul = result;
                    }
                }
                else
                {
                    var res = db.USER_REPOSITORY_OPERATION.Where(p => p.USER_ID == _UserId && p.REPOSITORY_ID == _RepoId);
                    foreach (var row in res)
                    {
                        ItemModel.actionInfo newAction = new ItemModel.actionInfo();
                        newAction.UpdateInfo = row.DESCRIPTION;
                        newAction.UserId = _UserId;
                        newAction.UserName = row.USERTABLE.USER_NAME;
                        newAction.UserOperation = row.OPERATION;
                        newAction.UserPhotoUrl = row.USERTABLE.IMAGE;
                        newAction.RepositoryId = _RepoId;
                       // newAction.RepositoryName = row.REPOSITORY.NAME;
                        SearchResul.Add(newAction);
                    }
                    flag = true;
                }
                if (flag == true)
                {
                    ErrorInfo = null;
                    return true;
                }
                else
                {
                    ErrorInfo = new ItemModel.ErrorMessage();
                    ErrorInfo.ErrorOperation = "获取用户自己的动态";
                    ErrorInfo.ErrorReason = "获取动态函数失败";
                    ErrorInfo.ErrorTime = System.DateTime.Now;
                    return false;
                }
            }
            catch (Exception ex)
            {
                SearchResul = null;
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "获取用户自己的动态";
                ErrorInfo.ErrorReason = ex.Message;
                return false;
            }
        }



        /// <summary>
        /// 修改用户好友关系
        /// 输入：用户自己的Id, 对象用户的Id, 关注或取消关注（true是关注）, 错误信息
        /// 输出：是否成功
        /// 完成测试
        /// </summary>
        public bool changeFriend(string _SelfUserId, string _TarUserId, bool Focus, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                bool flag = false;
                DBModel dbmodel = new DBModel();
                if (Focus == true)
                {
                    if (dbmodel.makeFriend(_SelfUserId, _TarUserId))
                        flag = true;
                }
                else
                {
                    if (dbmodel.deleteFriend(_SelfUserId, _TarUserId))
                        flag = true;
                }
                if (flag == true)
                {
                    ErrorInfo = null;
                    return true;
                }
                else
                {
                    ErrorInfo = new ItemModel.ErrorMessage();
                    ErrorInfo.ErrorOperation = "修改用户好友关系";
                    ErrorInfo.ErrorReason = "添加或者删除好友错误";
                    ErrorInfo.ErrorTime = System.DateTime.Now;
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "修改用户好友关系";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = System.DateTime.Now;
                return false;
            }

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
                //拿到该用户本月所有的操作
                var OpList = func.showOperationHistory(_UserId);
                OpList.Where(p => p.DATE > new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                //创建返回列表
                List<Database_course_design.Models.ItemModel.DayHeat> ret = new List<ItemModel.DayHeat>();
                //插入本月的天数

                for (int i = 0; i <= DateTime.Now.Day; i++)
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
                ErrorInfo = null;
                DayHeatResult = ret;
                return true;
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "getUserHeat";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = DateTime.Now;
                DayHeatResult = null;
                return false;
            }
        }

        
        /// <summary>
        /// 修改用户资料（对DBmodel的封装）
        /// 输入：待定参数， 错误信息
        /// 输出：是否成功
        /// </summary>
        public bool modifyBasicUserInfo(string _UserId, string _ImgUrl, string _NickName, string _Signature, string _Email, string _PersonalUrl/*这个参数什么意思*/,
                                                            string _Address, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                ErrorInfo = null;
                //修改表项信息
                DBModel func = new DBModel();
                if (func.changeUserInfo(_UserId, _Email, _ImgUrl, _Signature, _NickName, _PersonalUrl, _Address))
                {
                    throw new Exception("Modify error!");
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "changeBasicUserInfo";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = DateTime.Now;
                return false;
            }
        }

        /// <summary>
        /// 修改密码
        /// 输入：用户ID， 旧密码，新密码
        ///输出：是否修改成功
        /// </summary>
        public bool modifyPassword(string _UserId, string _OldPassword, string _NewPassword, out ItemModel.ErrorMessage ErrorInfo)
        {
            try
            {
                //实例化数据库
                KUXIANGDBEntities db = new KUXIANGDBEntities();
                //提取旧密码
                string OldPassword = db.USERTABLEs.Where(p => p.USER_ID == _UserId).Select(P => P.PASSWORD).FirstOrDefault();
                //旧密码正确
                if (0 == string.Compare(_OldPassword, OldPassword))
                {
                    //修改密码
                    var User = from p in db.USERTABLEs
                               where p.USER_ID == _UserId
                               select p;
                    User.FirstOrDefault().PASSWORD = _NewPassword;
                    db.SaveChanges();
                    ErrorInfo = null;
                    return true;
                }
                //密码不正确，抛出异常
                else
                {
                    throw new Exception("Password Wrong!");
                }
            }
            catch (Exception ex)
            {
                ErrorInfo = new ItemModel.ErrorMessage();
                ErrorInfo.ErrorOperation = "modifyPassword";
                ErrorInfo.ErrorReason = ex.Message;
                ErrorInfo.ErrorTime = DateTime.Now;
                return false;
            }
        }

    }
}