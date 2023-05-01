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
        /// Patch request for updateing timespan for a specific alarm
        /// </summary>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        [HttpPatch(Name = "UpdateAlarmInfo")]
        public IActionResult UpdateAlarmInfo(PairInfo pairInfo)
        {
            Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (headerValue.Count > 0)
            {
                bool response = _appService.UpdateTimeSpan(pairInfo.Username, pairInfo.AlarmInfo, headerValue[0]);
                if (response)
                {
                    return Ok();
                }
                return NotFound();

            }
            return BadRequest();

        }


        /// <summary>
        /// Get request for getting all alarms that belongs to a specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>list of alarms</returns>
        [HttpGet(Name = "GetAlarms")]
        public IActionResult GetAlarms(string username)
        {
            Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (headerValue.Count > 0)
            {
                var alarms = _appService.GetAlarms(username, headerValue[0]);
                if (alarms == null || alarms.Count <= 0)
                {
                    return NotFound();
                }
                return Ok(alarms);
                
            }
            return BadRequest();
            
        }

        /// <summary>
        /// post request for parring of alarm and user
        /// </summary>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "PairAlarm")]
        public IActionResult PairAlarm(PairInfo pairInfo)
        {
            Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (headerValue.Count > 0)
            {
                string response = _appService.PairAlarm(pairInfo, headerValue[0]);
                if (response == "Yes")
                {
                    return Created("", pairInfo);
                }
                return NotFound(response);
            }
            
            return BadRequest();
        }

        /// <summary>
        /// post request for stopping an alrm from the app
        /// </summary>
        /// <param name="alarmStop"></param>
        /// <returns></returns>
        [HttpPost(Name = "StopAlarm")]
        public IActionResult StopAlarm(AlarmStopPOGO alarmStop)
        {
            Request.Headers.TryGetValue("token", out StringValues headerValue);
            if (headerValue.Count>0)
            {
                bool response = _appService.StopAlarm(alarmStop.AlarmID, alarmStop.Username, headerValue[0]);
                if (response)
                {
                    return Ok();
                }
                return NotFound();
            }
            
            return BadRequest();
        }
    }
}
