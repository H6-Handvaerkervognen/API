using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Notifiacation;
using HåndværkervognenAPI.Security;

namespace HåndværkervognenAPI.Managers
{
    public class NotificationAlarmManager : IAlarmService
    {
        private IDatabase _database;
        private IEncryption _encryption;
        private INotification _notifiaction;

        public NotificationAlarmManager(IDatabase database, IEncryption encryption, INotification notifiaction)
        {
            _database = database;
            _encryption = encryption;
            _notifiaction = notifiaction;
        }

        /// <summary>
        /// sends a notification to the user and starts the alrm in the database
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public bool AlertUser(string alarmId)
        {
            try
            {
                _notifiaction.SendNotificationAsync(alarmId);
                _database.StartAlarm(alarmId);
            }
            catch (Exception)
            {

                return false;

            }
            return true;
        }

        public bool CheckIfAlarmExist(string alarmId)
        {
            return _database.CheckIfAlarmExists(alarmId);
        }


        /// <summary>
        /// deletes the paring for a user
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public bool DeletePairing(string alarmId)
        {
            //todo
            try
            {
                _database.DeletePairing(alarmId);
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        /// <summary>
        /// gets info for specific alarm
        /// </summary>
        /// <param name="alarmid"></param>
        /// <returns>AlarmInfoDto</returns>
        public AlarmInfoDto GetAlarmInfo(string alarmid)
        {
            AlarmDal alarmDal = _database.GetAlarmInfo(alarmid);
            if (alarmDal != null)
            {
                return new AlarmInfoDto(_encryption.DecryptData(alarmDal.StartTime, alarmid), _encryption.DecryptData(alarmDal.EndTime, alarmid), alarmid, _encryption.DecryptData(alarmDal.Name, alarmid));

            }
            return null;
        }

        /// <summary>
        /// Gets alarm status
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public bool GetStatus(string alarmId)
        {
            
            return _database.CheckAlarmStatus(alarmId);
        }
    }
}
