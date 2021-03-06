﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    /// <summary>
    /// 搜索仓库信息结果类
    /// 仓库id
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
        public string RepertoryID { get; set; }
        public string RepertoryName { get; set; }
        public string RepertoryInfo { get; set; }
        public string RepertoryUpdateTime { get; set; }
        public int RepertoryStar { get; set; }
        public int RepertoryFork { get; set; }
        public string RepertoryCreater { get; set; }
        public string RepertoryLabel1 { get; set; }
        public string RepertoryLabel2 { get; set; }
        public string RepertoryLabel3 { get; set; }

        /// <summary>
        /// 带参数构造函数
        /// </summary>
        public RepertorySearchResult(REPOSITORY _Repo)
        {
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            RepertoryID = _Repo.REPOSITORY_ID;
            RepertoryName = _Repo.NAME;
            RepertoryInfo = _Repo.DESCRIPTION;
            RepertoryUpdateTime = _Repo.UPDATE_DATE.ToString();
            RepertoryStar = _Repo.STAR_NUM.Value;
            RepertoryFork = _Repo.FORK_NUM.Value;
            RepertoryCreater = db.USER_REPOSITORY_RELATIONSHIP.Where(p => p.REPOSITORY_ID == _Repo.REPOSITORY_ID
                                                                 && p.RELATIONSHIP == 0).FirstOrDefault().USERTABLE.USER_NAME;
            RepertoryLabel1 = _Repo.COURSE.LABEL1;
            RepertoryLabel2 = _Repo.COURSE.LABEL2;
            RepertoryLabel3 = _Repo.COURSE.LABEL3;
        }

        /// <summary>
        /// 空构造函数
        /// </summary>
        public RepertorySearchResult()
        {

        }
    }
}