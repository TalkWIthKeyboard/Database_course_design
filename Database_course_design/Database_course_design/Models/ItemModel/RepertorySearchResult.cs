using System;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public RepertorySearchResult(REPOSITORY _Repo)
        {
            KUXIANGDBEntities db = new KUXIANGDBEntities();
            RepertoryName = _Repo.NAME;
            RepertoryUrl = _Repo.URL;
            RepertoryInfo = _Repo.DESCRIPTION;
            RepertoryUpdateTime = _Repo.UPDATE_DATE.ToString();
            RepertoryStar = _Repo.STAR_NUM.Value;
            RepertoryFork = _Repo.FORK_NUM.Value;

            RepertoryCreater = 
                db.USERTABLEs.Where(
                q => q.USER_ID == (_Repo.USER_REPOSITORY_RELATIONSHIP .Where(p => p.REPOSITORY_ID == _Repo.REPOSITORY_ID) .Select(p => p.USER_ID).FirstOrDefault()))
                .Select(q => q.USER_NAME).First();

            RepertoryLabel1 = _Repo.COURSE.LABEL1;
            RepertoryLabel2 = _Repo.COURSE.LABEL2;
            RepertoryLabel3 = _Repo.COURSE.LABEL3;
        }
    }
}