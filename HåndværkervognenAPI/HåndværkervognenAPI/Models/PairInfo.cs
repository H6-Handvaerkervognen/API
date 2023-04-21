namespace HåndværkervognenAPI.Model
{
    public class PairInfo
    {
		private string appId;

		private AlarmInfoDto alarmInfo;

		public AlarmInfoDto AlarmInfo
		{
			get { return alarmInfo; }
			set { alarmInfo = value; }
		}

		public string AppId
		{
			get { return appId; }
			set { appId = value; }
		}

        public PairInfo(AlarmInfoDto alarmInfo, string appId)
        {
			AlarmInfo = alarmInfo;
			AppId = appId;
        }
    }
}
