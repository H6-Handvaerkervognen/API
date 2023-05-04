using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HåndværkervognenAPI.Controllers
{   
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppController : ControllerBase
    {
        private IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// Patch request for updating timespan for a specific alarm
        /// </summary>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        [HttpPatch(Name = "UpdateAlarmInfo")]
        public IActionResult UpdateAlarmInfo(PairInfo pairInfo)
        {
            bool exists = Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (exists)
            {
                bool response = _appService.UpdateAlarmInfo(pairInfo.Username, pairInfo.AlarmInfo, headerValue[0]);
                if (response)
                {
                    return Ok();
                }
                return NotFound();
            }
            return Unauthorized();
        }


        /// <summary>
        /// Get request for getting all alarms that belongs to a specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>list of alarms</returns>
        [HttpGet(Name = "GetAlarms")]
        public IActionResult GetAlarms(string username)
        {
            bool exists = Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (exists)
            {
                var alarms = _appService.GetAlarms(username, headerValue[0]);
                if (alarms == null || alarms.Count <= 0)
                {
                    return NotFound();
                }
                return Ok(alarms);
            }
            return Unauthorized();
        }

        /// <summary>
        /// post request for pairng of alarm and user
        /// </summary>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "PairAlarm")]
        public IActionResult PairAlarm(PairInfo pairInfo)
        {
            bool exists = Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (exists)
            {
                if (_appService.PairAlarm(pairInfo, headerValue[0]))
                {
                    return Created("", pairInfo);
                }
                return NotFound();
            }
            return Unauthorized();
        }

        /// <summary>
        /// post request for stopping an alarm from the app
        /// </summary>
        /// <param name="alarmStop"></param>
        /// <returns></returns>
        [HttpPost(Name = "StopAlarm")]
        public IActionResult StopAlarm(AlarmStopPOGO alarmStop)
        {
            bool exists = Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (exists)
            {
                bool response = _appService.StopAlarm(alarmStop.AlarmID, alarmStop.Username, headerValue[0]);
                if (response)
                {
                    return Ok();
                }
                return NotFound();
            }
            return Unauthorized();
        }
    }
}
