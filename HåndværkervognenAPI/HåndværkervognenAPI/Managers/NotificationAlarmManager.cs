using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Model;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Notifiacation;
using HåndværkervognenAPI.Security;

namespace HåndværkervognenAPI.Managers
{
    public class NotificationAlarmManager : IAlarmService
    {
        private IDatabase _database;
        private IEncryption _encryption;
        private INotifiaction _notifiaction;

        public NotificationAlarmManager(IDatabase database, IEncryption encryption, INotifiaction notifiaction)
        {
            _database = database;
            _encryption = encryption;
            _notifiaction = notifiaction;
        }

        public bool AlertUser(string alarmId)
        {
            try
            {
                _notifiaction.SendNotificationAsync(alarmId);
            }
            catch (Exception)
            {

               return false;

            }
            return true;
           
        }

        public bool DeletePairing(string alarmId)
        {
            //todo
            try
            {
              //  _database.DeletePairing(alarmId);
            }
            catch (Exception)
            {

               return false;
            }
           
            return true;
        }

        public AlarmInfoDto GetAlarmInfo(string alarmid)
        {
            AlarmDal alarmDal = _database.GetAlarmInfo(alarmid);
            AlarmInfoDto alarmInfo = new AlarmInfoDto(TimeSpan.Parse( _encryption.DecryptData(alarmDal.StartTime)),TimeSpan.Parse( _encryption.DecryptData(alarmDal.EndTime)),alarmid, _encryption.DecryptData(alarmDal.Name));
            return alarmInfo;
        }
    }
}
