namespace HåndværkervognenAPI.Models
{
    public class AlarmDal
    {
		private string starTime;

		private string endTime;

		private string alarmId;

		private string name;

        private byte[] salt;


        public string StartTime { get; private set; }

        public string EndTime { get; private set; }

        public string AlarmId { get; private set; }
        public string Name { get; private set; }
        public byte[] Salt { get; private set; }

        public AlarmDal(string startTime, string endTime, string alarmId, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            AlarmId = alarmId;
            Name = name;
        }
    }
}
