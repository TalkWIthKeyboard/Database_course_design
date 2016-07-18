using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    public class HeatOpItem
    {
        public string OPERATION { get; set; }
        public string TARGET_REPOSITORY_NAME { get; set; }
        public string TARGET_USER_NAME { get; set; }
    }

    public class DayHeat
    {
        public List<HeatOpItem> OpList = new List<HeatOpItem>();
        public int Count;
    }
}