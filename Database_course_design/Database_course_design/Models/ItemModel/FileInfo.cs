using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    public class FileInfo
    {
        public string RepositoryName { set; get; }
        public string FileName { set; get; }
        public string FileType { set; get; }
        public int? FileSize { set; get; }
        public string Url { set; get; }
        public string LastUpdateTime { set; get; }

        /// <summary>
        /// 带仓库id的构造函数
        /// </summary>
        public FileInfo(FILETABLE fileTable, string repId)
        {
            var db = new KUXIANGDBEntities();
            RepositoryName = db.REPOSITORies.Where(p => p.REPOSITORY_ID == repId).Select(p => p.NAME).FirstOrDefault();
            FileName = fileTable.FILE_NAME;
            FileType = fileTable.FILE_TYPE;
            LastUpdateTime = fileTable.UPDATE_DATE.ToString();
            Url = fileTable.PATH;
            FileSize = fileTable.FILE_SIZE;
        }

        /// <summary>
        /// 带仓库id的构造函数
        /// </summary>
        public FileInfo(FILETABLE fileTable)
        {
            var db = new KUXIANGDBEntities();
            RepositoryName = null;
            FileName = fileTable.FILE_NAME;
            FileType = fileTable.FILE_TYPE;
            LastUpdateTime = fileTable.UPDATE_DATE.ToString();
            Url = fileTable.PATH;
            FileSize = fileTable.FILE_SIZE;
        }
    }
}