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
    
    public partial class REPOSITORY
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REPOSITORY()
        {
            this.REPOSITORY_FILE = new HashSet<REPOSITORY_FILE>();
            this.REPOSITORY1 = new HashSet<REPOSITORY>();
            this.USER_COMMENT_REPOSITORY = new HashSet<USER_COMMENT_REPOSITORY>();
            this.USER_REPOSITORY_LOOKHISTORY = new HashSet<USER_REPOSITORY_LOOKHISTORY>();
            this.USER_REPOSITORY_OPERATION = new HashSet<USER_REPOSITORY_OPERATION>();
            this.USER_REPOSITORY_RELATIONSHIP = new HashSet<USER_REPOSITORY_RELATIONSHIP>();
        }
    
        public string REPOSITORY_ID { get; set; }
        public string NAME { get; set; }
        public int AUTHORITY { get; set; }
        public string LABEL1 { get; set; }
        public string LABEL2 { get; set; }
        public string LABEL3 { get; set; }
        public Nullable<int> ATTRIBUTE { get; set; }
        public Nullable<int> STAR_NUM { get; set; }
        public Nullable<int> FORK_NUM { get; set; }
        public Nullable<int> IS_CREATE { get; set; }
        public string FORK_FROM { get; set; }
        public string DESCRIPTION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPOSITORY_FILE> REPOSITORY_FILE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REPOSITORY> REPOSITORY1 { get; set; }
        public virtual REPOSITORY REPOSITORY2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_COMMENT_REPOSITORY> USER_COMMENT_REPOSITORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_REPOSITORY_LOOKHISTORY> USER_REPOSITORY_LOOKHISTORY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_REPOSITORY_OPERATION> USER_REPOSITORY_OPERATION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_REPOSITORY_RELATIONSHIP> USER_REPOSITORY_RELATIONSHIP { get; set; }
    }
}
