using FirebaseAdmin.Messaging;

namespace HåndværkervognenAPI.Notifiacation
{
    public class FcmNotification : INotifiaction
    {
        public async Task SendNotificationAsync(string alarmId)
        {
            // The topic name can be optionally prefixed with "/topics/".
            var topic = "Test";

            // See documentation on defining a message payload.
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
    {
        { "score", "850" },
        { "time", "2:45" },
    },
                Topic = topic,
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);

        }
    }
}
