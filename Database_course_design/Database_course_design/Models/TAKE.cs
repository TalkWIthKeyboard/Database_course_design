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
    
    public partial class TAKE
    {
        public string USER_ID { get; set; }
        public string COURSE_ID { get; set; }
        public string TAKES_ID { get; set; }
    
        public virtual COURSE COURSE { get; set; }
        public virtual USERTABLE USERTABLE { get; set; }
    }
}
