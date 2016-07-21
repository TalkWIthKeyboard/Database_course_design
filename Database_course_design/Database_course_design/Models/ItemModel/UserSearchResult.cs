using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database_course_design.Models;

namespace Database_course_design.Models.ItemModel
{
    /// <summary>
    /// 搜索用户信息结果类
    /// 用户id
    /// 用户名
    /// 用户个性签名
    /// 用户头像链接
    /// 用户创建时间
    /// 用户的email
    /// </summary>
    public class UserSearchResult
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserSignature { get; set; }
        public string UserPhotoUrl { get; set; }
        public string  UserCreateTime { get; set; }
        public string UserEmail { get; set; }

        public UserSearchResult(USERTABLE user)
        {
            UserId = user.USER_ID;
            UserName = user.NICKNAME;
            UserSignature = user.SIGNATURE;
            UserPhotoUrl = user.IMAGE;
            UserCreateTime = user.CREATE_DATE.ToString();
            UserEmail = user.EMAIL;
        }
    }
}