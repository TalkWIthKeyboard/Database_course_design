﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    /// <summary>
    /// 搜索仓库信息结果类
    /// 仓库名
    /// 仓库链接
    /// 仓库信息
    /// 仓库最近更新时间
    /// 仓库star数量
    /// 仓库fork数量
    /// 仓库创建者
    /// 仓库的一级标签
    /// 仓库的二级标签
    /// 仓库的三级标签
    /// </summary>
    public class RepertorySearchResult
    {
        public string RepertoryName { get; set; }
        public string RepertoryUrl { get; set; }
        public string RepertoryInfo { get; set; }
        public string RepertoryUpdateTime { get; set; }
        public int RepertoryStar { get; set; }
        public int RepertoryFork { get; set; }
        public string RepertoryCreater { get; set; }
        public string RepertoryLabel1 { get; set; }
        public string RepertoryLabel2 { get; set; }
        public string RepertoryLabel3 { get; set; }
    }
}