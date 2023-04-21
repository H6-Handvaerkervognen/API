using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlarmController
    {
        [HttpGet(Name = "GetAlarmInfo")]
        public bool GetAlarmInfo(string AppId)
        {
            return true;
        }
        [HttpPost(Name = "DeleteParring")]
        public void DeleteParring(string AlarmID)
        {

        }
        [HttpPost(Name = "ActivateAlarm")]
        public void ActivateAlarm(string AlarmID)
        {

        }
    }
}
