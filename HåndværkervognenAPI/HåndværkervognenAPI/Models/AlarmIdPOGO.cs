﻿namespace HåndværkervognenAPI.Models
{
    public class AlarmIdPOGO
    {
        private string alarmID;

        public string AlarmID { get; private set; }

        public AlarmIdPOGO(string alarmID)
        {
            AlarmID = alarmID;
        }
    }
}