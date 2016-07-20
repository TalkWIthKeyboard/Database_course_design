using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models.ItemModel
{
    public class ErrorMessage
    {
        public string ErrorOperation { get; set; }       
        public string ErrorReason { get; set; }
        public DateTime ErrorTime { get; set; }

        public ErrorMessage(string op, string reason)
        {
            ErrorOperation = op;
            ErrorReason = reason;
            ErrorTime = DateTime.Now;
        }
        
        public ErrorMessage()
        {

        }
    }

}

