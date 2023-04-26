namespace HåndværkervognenAPI.Notifiacation
{
    public interface INotification
    {
         Task SendNotificationAsync(string alarmId);
    }
}
