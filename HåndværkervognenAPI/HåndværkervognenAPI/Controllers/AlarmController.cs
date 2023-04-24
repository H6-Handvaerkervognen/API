using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlarmController:ControllerBase
    {
        private IAlarmService _alarmService;

        public AlarmController(IAlarmService alarmService)
        {
            _alarmService = alarmService;
        }



        [HttpGet(Name = "GetAlarmInfo")]
        public IActionResult GetAlarmInfo(string AppId)
        {
           AlarmInfoDto alarmInfo = _alarmService.GetAlarmInfo(AppId);
            if (alarmInfo==null)
            {
                return BadRequest();
            }
            return Ok(alarmInfo);
        }

        [HttpPost(Name = "DeleteParring")]
        public IActionResult DeleteParring(string AlarmID)
        {
            if (_alarmService.DeletePairing(AlarmID))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost(Name = "ActivateAlarm")]
        public IActionResult ActivateAlarm(string AlarmID)
        {
            if (_alarmService.AlertUser(AlarmID))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
