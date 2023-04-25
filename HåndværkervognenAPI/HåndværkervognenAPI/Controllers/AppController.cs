
using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Model;

using Microsoft.AspNetCore.Mvc;


namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AppController : ControllerBase
    {
        private IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// post request for updateing timespan for a specific alarm
        /// </summary>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "UpdateTimespan")]
        public IActionResult UpdateTimespan(PairInfo pairInfo)
        {
            bool response = _appService.UpdateTimeSpan(pairInfo.AppId, pairInfo.AlarmInfo);
            if (response)
            {
                return Ok();
            }
            return BadRequest();

        }

        /// <summary>
        /// Get request for getting all alarms that belongs to a specific user
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns>list of alrms</returns>
        [HttpGet(Name = "GetAlarms")]
        public IActionResult GetAlarms(string AppID)
        {
            var alarms = _appService.GetAlarms(AppID);
            if (alarms == null || alarms.Count <= 0)
            {
                return NotFound();
            }
            return Ok(alarms);
        }

        /// <summary>
        /// post request for parring of alarm and user
        /// </summary>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        [HttpPost(Name = "PairAlarm")]
        public IActionResult PairAlarm(PairInfo pairInfo)
        {
            bool response = _appService.PairAlarm(pairInfo);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// post request for stopping an alrm from the app
        /// </summary>
        /// <param name="AlarmID"></param>
        /// <returns></returns>
        [HttpPost(Name = "StopAlarm")]
        public IActionResult StopAlarm(string AlarmID)
        {
            bool response = _appService.StopAlarm(AlarmID);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
