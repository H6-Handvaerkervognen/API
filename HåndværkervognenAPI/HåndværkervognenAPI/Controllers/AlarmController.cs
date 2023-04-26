﻿using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
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
        /// <param name="username"></param>
        /// <returns>AlarmInfoDto alarmInfo</returns>
        [HttpGet(Name = "GetAlarmInfo")]
        public IActionResult GetAlarmInfo(string AlarmId)
        {
           AlarmInfoDto alarmInfo = _alarmService.GetAlarmInfo(AlarmId);
            if (alarmInfo==null)
            {
                return BadRequest();
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
