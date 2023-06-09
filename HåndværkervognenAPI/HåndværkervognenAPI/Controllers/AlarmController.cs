﻿using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public IActionResult DeletePairing(string alarmID)
        {

            if (_alarmService.DeletePairing(alarmID))
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// post request that takes alarmid and notifies users and change a field in the database
        /// </summary>
        /// <param name="alarmID"></param>
        /// <returns></returns>
        [HttpPost(Name = "ActivateAlarm")]
        public IActionResult ActivateAlarm(AlarmIdPOGO alarmID)
        {
            if (_alarmService.CheckIfAlarmExist(alarmID.AlarmID))
            {
                if (!_alarmService.GetStatus(alarmID.AlarmID))
                {
                    if (_alarmService.AlertUser(alarmID.AlarmID))
                    {
                        return Ok("Alarm startet");
                    }
                    return BadRequest();
                }
                return BadRequest("Alarm already on");
            }
            return NotFound("No alarm found");
        }


        [HttpGet(Name = "GetStatus")]
        public IActionResult GetStatus(string alarmID)
        {
            if (_alarmService.CheckIfAlarmExist(alarmID))
            {
                return Ok(_alarmService.GetStatus(alarmID));
            }
            return NotFound();
        }
    }
}
