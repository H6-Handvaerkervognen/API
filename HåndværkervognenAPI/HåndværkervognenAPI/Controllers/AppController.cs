
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
        [HttpGet(Name = "GetAlarms")]
        public IActionResult GetAlarms(string AppID)
        {
            var alarms = _appService.GetAlarms(AppID);
            if (alarms == null || alarms.Count >= 0)
            {
                return NotFound();
            }
            return Ok();
        }
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
