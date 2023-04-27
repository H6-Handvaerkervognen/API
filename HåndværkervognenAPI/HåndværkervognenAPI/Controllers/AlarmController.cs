using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AlarmController : ControllerBase
    {
        private IAlarmService _alarmService;

        public AlarmController(IAlarmService alarmService)
        {
            _alarmService = alarmService;
        }


        /// <summary>
        /// Get request that gets info on specific alarm form alarmManager
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns>AlarmInfoDto alarmInfo</returns>
        [HttpGet(Name = "GetAlarmInfo")]
        public IActionResult GetAlarmInfo(string alarmId)
        {
            AlarmInfoDto alarmInfo = _alarmService.GetAlarmInfo(alarmId);
            if (alarmInfo == null)
            {
                return NoContent();
            }
            return Ok(alarmInfo);
        }

        /// <summary>
        /// post request that takes alarmid and deletes all parrings connected to it
        /// </summary>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        [HttpPost(Name = "DeleteParring")]
        public IActionResult DeleteParring(string alarmID, string username)
        {
            //MANGLER USERNAME
            if (_alarmService.DeletePairing(alarmID, username))
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// post request that takes alarmid and notyfies users and change a field in the database
        /// </summary>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        [HttpPost(Name = "ActivateAlarm")]
        public IActionResult ActivateAlarm(string alarmID)
        {
            if (_alarmService.AlertUser(alarmID))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
