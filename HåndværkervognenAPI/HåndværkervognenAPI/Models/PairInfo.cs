namespace HåndværkervognenAPI.Model
{
    public class PairInfo
    {
		private string appId;

		private AlarmInfoDto alarmInfo;

		public AlarmInfoDto AlarmInfo { get; private set; }

        public string AppId { get; private set; }

        public PairInfo(AlarmInfoDto alarmInfo, string appId)
        {
			AlarmInfo = alarmInfo;
			AppId = appId;
        }
    }
}
