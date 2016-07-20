using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    public class FileInfo
    {
        public string path { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public int? size { set; get; }

        public FileInfo(FILETABLE fileTable)
        {
            path = fileTable.PATH;
            name = fileTable.FILE_NAME;
            type = fileTable.FILE_TYPE;
            size = fileTable.FILE_SIZE;
        }
    }
}