using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Database_course_design.Models
{
    public class JsonConvert
    {
        public string HeatOpItemToJson(ItemModel.HeatOpItem _SourceObj)
        {
            string JsonResult = new string('{',1);
            JsonResult =JsonResult + "\"Operation\": " +'\"'+ _SourceObj.OPERATION + '\"' +" , ";
            JsonResult =JsonResult + "\"TARGET_REPOSITORY_NAME\": " + '\"' + _SourceObj.TARGET_REPOSITORY_NAME + '\"' + " , ";
            JsonResult = JsonResult + "\"TARGET_USER_NAME\":" + '\"' + _SourceObj.TARGET_USER_NAME + '\"' + "}";
            return JsonResult;
        }

        public string HeatListToJson(List<ItemModel.DayHeat> _SourceObj)
        {
            string JsonResult = new string('{', 1);// Obj:  begin
            JsonResult += " [ ";// List:  begin
            //对于每个DayHeat对象
            foreach (var dayheat in _SourceObj)
            {
                JsonResult += "{ ";//DayHeat:  begin
                JsonResult += "\"Count\": " + dayheat.Count + " , ";
                JsonResult += "\"OpList\": ";
                JsonResult += "[ ";//Oplist:  begin
                for(int i = 0; i < dayheat.OpList.Count; i++)
                {
                    var opitem = dayheat.OpList[i];
                    JsonResult += HeatOpItemToJson(opitem);
                    if( i != dayheat.OpList.Count - 1)
                    {
                        JsonResult += " , ";
                    }
                }
                JsonResult += " ]";//OpList:  end
                JsonResult += " }";//DayHeat:  end
                JsonResult += " , ";
            }
            //删掉最后一个逗号
            JsonResult = JsonResult.Remove(JsonResult.Length - 2);
            JsonResult += " ]";//List:  end
            JsonResult += " }";// Obj:  end
            return JsonResult;
        }
    }
}