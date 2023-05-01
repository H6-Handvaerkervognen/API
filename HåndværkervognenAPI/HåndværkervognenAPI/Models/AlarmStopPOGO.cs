namespace HåndværkervognenAPI.Models
{
    public class AlarmStopPOGO
    {
		private string alarmID;
		private string username;

		

		public string AlarmID { get; private set; }
        public string Username { get; private set; }

        public AlarmStopPOGO(string alarmID, string username)
        {
            AlarmID = alarmID;
            Username = username;
        }
    }
}
