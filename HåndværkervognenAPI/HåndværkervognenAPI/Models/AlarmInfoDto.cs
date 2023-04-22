namespace HåndværkervognenAPI.Model
{
    public class AlarmInfoDto
    {
		private TimeSpan startTime;

		private TimeSpan endTime;

		private string alarmId;

		private string name;

		public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public string AlarmId { get; private set; }
        public string Name { get; private set; }

        public AlarmInfoDto(TimeSpan startTime, TimeSpan endTime, string alarmId, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            AlarmId = alarmId;
            Name = name;
        }
    }
}
