namespace HåndværkervognenAPI.Notifiacation
{
    public interface INotifiaction
    {
         Task SendNotificationAsync(string alarmId);
    }
}
