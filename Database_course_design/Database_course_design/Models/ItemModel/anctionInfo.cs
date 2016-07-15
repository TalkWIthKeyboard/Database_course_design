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
        public string UserName;
        public string UserUrl;
        public string UserPhotoUrl;
        public string UserOperation;
        public string RepertoryName;
        public string RepertoryUrl;
        public string UpdateInfo;
    }
}