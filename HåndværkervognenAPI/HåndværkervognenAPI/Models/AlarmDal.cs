namespace HåndværkervognenAPI.Models
{
    public class AlarmDal
    {
		private string starTime;

		private string endTime;

		private string alarmId;

		private string name;


		public string StartTime { get; private set; }

        public string EndTime { get; private set; }

        public string AlarmId { get; private set; }
        public string Name { get; private set; }

    }
}
