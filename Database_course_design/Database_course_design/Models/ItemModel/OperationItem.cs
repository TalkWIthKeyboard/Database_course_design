using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    public class OperationItem
    {
        public string USER_NAME { get; set; }
        public string OPERATION { get; set; }
        public string REPOSITORY_ID { get; set; }
        public string REPOSITORY_NAME { get; set; }
        public DateTime DATE { get; set; }
    }
}