using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    /// <summary>
    /// 动态信息类
    /// 用户id
    /// 用户名
    /// 用户的头像链接
    /// 用户的操作
    /// 仓库的id
    /// 仓库的名字
    /// 仓库的更新信息
    /// </summary>
    public class ActionInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhotoUrl { get; set; }
        public string UserOperation { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryId { get; set; }
        public string UpdateInfo { get; set; }
    }
}