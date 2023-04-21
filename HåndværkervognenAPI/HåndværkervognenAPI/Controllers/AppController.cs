using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController
    {
        [HttpPost(Name = "UpdateTimespan")]
        public void UpdateTimespan(string AppID)
        {
           
        }
        [HttpGet(Name = "DeleteParring")]
        public void GetAlarms(string AppID)
        {

        }
        [HttpPost(Name = "PairAlarm")]
        public void PairAlarm(string AlarmID)
        {

        }
        [HttpPost(Name = "StopAlarm")]
        public void StopAlarm(string AlarmID)
        {

        }
    }
}
