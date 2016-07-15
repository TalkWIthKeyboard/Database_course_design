using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    /// <summary>
    /// 搜索用户信息结果类
    /// 用户名
    /// 用户个性签名
    /// 用户头像链接
    /// 用户创建时间
    /// 用户的email
    /// </summary>
    public class UserSearchResult
    {
        public string UserName;
        public string UserState;
        public string UserPhotoUrl;
        public string UserCreateTime;
        public string UserEmail;
    }
}