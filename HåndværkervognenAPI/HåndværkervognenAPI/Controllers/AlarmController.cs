using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlarmController:ControllerBase
    {
        [HttpGet(Name = "GetAlarmInfo")]
        public IActionResult GetAlarmInfo(string AppId)
        {
            return Ok(true);
        }

        [HttpPost(Name = "DeleteParring")]
        public IActionResult DeleteParring(string AlarmID)
        {
            return Ok();
        }
        [HttpPost(Name = "ActivateAlarm")]
        public IActionResult ActivateAlarm(string AlarmID)
        {
            return Ok();
        }
    }
}
