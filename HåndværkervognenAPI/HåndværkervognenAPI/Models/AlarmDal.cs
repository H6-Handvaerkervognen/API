namespace HåndværkervognenAPI.Models
{
    public class AlarmDal
    {
		

        public byte[] StartTime { get; private set; }

        public byte[] EndTime { get; private set; }

        public string AlarmId { get; private set; }
        public byte[] Name { get; private set; }
   

        public AlarmDal(byte[] startTime, byte[] endTime, string alarmId, byte[] name)
        {
            StartTime = startTime;
            EndTime = endTime;
            AlarmId = alarmId;
            Name = name;
        }
    }
}
