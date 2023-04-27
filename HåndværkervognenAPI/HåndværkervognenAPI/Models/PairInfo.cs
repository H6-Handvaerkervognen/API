namespace HåndværkervognenAPI.Models
{
    public class PairInfo
    {
        public AlarmInfoDto AlarmInfo { get; private set; }

        public string Username { get; private set; }

        public PairInfo(AlarmInfoDto alarmInfo, string username)
        {
            AlarmInfo = alarmInfo;
            Username = username;
        }
    }
}
