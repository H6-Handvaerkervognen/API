namespace HåndværkervognenAPI.Model
{
    public class AlarmInfoDto
    {
		private TimeSpan startTime;

		private TimeSpan endTime;

		private string alarmId;

		private string name;

		public TimeSpan StartTime
		{
			get { return startTime; }
			set { startTime = value; }
		}
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public string AlarmId
        {
            get { return alarmId; }
            set { alarmId = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public AlarmInfoDto(TimeSpan startTime, TimeSpan endTime, string alarmId, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            AlarmId = alarmId;
            Name = name;
        }
    }
}
