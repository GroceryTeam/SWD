using BusinessLayer.Interfaces.Notification;
using CorePush.Google;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Notification
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotification()
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", true, true)
           .Build();

            FcmSettings settings = new FcmSettings()
            {
                SenderId = config["FcmNotification:SenderId"],
                ServerKey = config["FcmNotification:ServerKey"]
            };
            HttpClient httpClient = new HttpClient();

            string authorizationKey = string.Format("keyy={0}", settings.ServerKey);

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
            httpClient.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var fcm = new FcmSender(settings, httpClient);
            var fcmSendResponse = await fcm.SendAsync("Hello");
        }

    }
}
