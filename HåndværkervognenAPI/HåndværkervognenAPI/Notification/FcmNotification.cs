using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace HåndværkervognenAPI.Notifiacation
{
    public class FcmNotification : INotification
    {
        /// <summary>
        /// Sends a notification to a specified topic, which is the <paramref name="alarmId"/>
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        public async Task SendNotificationAsync(string alarmId)
        {
            //the channel to send the notification to
            string topic = alarmId;
            
            //The message we are sending to the applications
            Message message = new Message()
            {
                Topic = topic,
                Notification= new Notification()
                { 
                    Title = "der er inbrud i din bil", 
                    Body = "skynd dig ud til din bil"
                }
            };

            // Send a message to the devices subscribed to the provided topic.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            // Response is a message ID string.
            //Console.WriteLine("Successfully sent message: " + response);

        }
    }
}
