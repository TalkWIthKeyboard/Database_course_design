//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database_course_design.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class USER_COMMENT_REPOSITORY
    {
        public string COMMENT_ID { get; set; }
        public string REPOSITORY_ID { get; set; }
        public string ANNOUNCER { get; set; }
    
        public virtual COMMENTTABLE COMMENTTABLE { get; set; }
        public virtual REPOSITORY REPOSITORY { get; set; }
        public virtual USERTABLE USERTABLE { get; set; }
    }
}
