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


        /// <summary>
        /// Get request that gets info on specific alarm form alarmManager
        /// </summary>
        /// <param name="AppId"></param>
        /// <returns>AlarmInfoDto alarmInfo</returns>
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

        /// <summary>
        /// post request that takes alarmid and deletes all parrings connected to it
        /// </summary>
        /// <param name="AlarmID"></param>
        /// <returns></returns>
        [HttpPost(Name = "DeleteParring")]
        public IActionResult DeleteParring(string AlarmID)
        {
            if (_alarmService.DeletePairing(AlarmID))
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// post request that takes alarmid and notyfies users and change a field in the database
        /// </summary>
        /// <param name="AlarmID"></param>
        /// <returns></returns>
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
