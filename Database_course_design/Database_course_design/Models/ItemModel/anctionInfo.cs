using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    /// <summary>
    /// 动态信息类
    /// 用户名
    /// 用户的链接
    /// 用户的头像链接
    /// 用户的操作
    /// 仓库的名字
    /// 仓库的链接
    /// 仓库的更新信息
    /// </summary>
    public class anctionInfo
    {
        public string UserName { get; set; }
        public string UserUrl { get; set; }
        public string UserPhotoUrl { get; set; }
        public string UserOperation { get; set; }
        public string RepertoryName { get; set; }
        public string RepertoryUrl { get; set; }
        public string UpdateInfo { get; set; }
    }
}