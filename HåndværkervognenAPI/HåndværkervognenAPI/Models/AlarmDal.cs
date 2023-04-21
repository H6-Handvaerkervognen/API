namespace HåndværkervognenAPI.Models
{
    public class AlarmDal
    {
		private string starTime;

		private string endTime;

		private string alarmId;

		private string name;


		public string StartTime
		{
			get { return starTime; }
			set { starTime = value; }
		}

        public string EndTime
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

    }
}
