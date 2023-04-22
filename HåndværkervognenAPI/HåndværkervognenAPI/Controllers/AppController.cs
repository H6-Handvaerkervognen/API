using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Managers;
using HåndværkervognenAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController
    {
        private IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpPost(Name = "UpdateTimespan")]
        public void UpdateTimespan(PairInfo pairInfo)
        {
            _appService.UpdateTimeSpan(pairInfo.AppId, pairInfo.AlarmInfo);
        }
        [HttpGet(Name = "DeleteParring")]
        public void GetAlarms(string AppID)
        {
            _appService.GetAlarms(AppID);
        }
        [HttpPost(Name = "PairAlarm")]
        public void PairAlarm(PairInfo pairInfo)
        {
            _appService.PairAlarm(pairInfo);
        }
        [HttpPost(Name = "StopAlarm")]
        public void StopAlarm(string AlarmID)
        {
            _appService.StopAlarm(AlarmID);
        }
    }
}
