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
        [HttpDelete(Name = "DeletePairing")]
        public IActionResult DeletePairing(string alarmId)
        {
            if (_alarmService.DeletePairing(alarmId))
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// post request that takes alarmid and notifies users and changes alarm state
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        [HttpPost(Name = "ActivateAlarm")]
        public IActionResult ActivateAlarm(string alarmId)
        {
            if (_alarmService.AlertUser(alarmId))
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpGet(Name = "GetStatus")]
        public IActionResult GetStatus(string alarmId)
        {
            return Ok(_alarmService.GetStatus(alarmId));
        }
    }
}
