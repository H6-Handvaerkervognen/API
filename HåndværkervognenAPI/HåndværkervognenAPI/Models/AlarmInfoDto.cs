namespace HåndværkervognenAPI.Models
{
    public class AlarmInfoDto
    {
        //public TimeSpan StartTime { get; private set; }
        public string StartTime { get; private set; }
        //public TimeSpan EndTime { get; private set; }
        public string EndTime { get; private set; }
        public string AlarmId { get; private set; }
        public string Name { get; private set; }

        public AlarmInfoDto(string startTime, string endTime, string alarmId, string name)
        {
            StartTime = startTime;
            EndTime = endTime;
            AlarmId = alarmId;
            Name = name;
        }
   
    }
}
