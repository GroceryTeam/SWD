using BusinessLayer.Interfaces.Notification;
using BusinessLayer.ResponseModels.Firebase;
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
        public async Task SendNotificationOutOfStockProduct(int productId, int brandId, string productName)
        {
            var notiModel = new OutOfStockFirebaseNotificationModel()
            {
                BrandId = brandId,
                ProductId = productId,
                ProductName = productName
            };
            await SendNotification(new GeneralFirebaseNotificationModel<OutOfStockFirebaseNotificationModel>()
            {
                Data = notiModel,
                Type = GeneralFirebaseNotificationModel<OutOfStockFirebaseNotificationModel>.NotiType.NearlyOutOfStock
            });
        }
        public async Task SendNotificationStoreApproved(int storeId, int brandId, string storeName)
        {
            var notiModel = new StoreApprovedRejectedNotificationModel()
            {
                BrandId = brandId,
                StoreId = storeId,
                StoreName = storeName
            };
            await SendNotification(new GeneralFirebaseNotificationModel<StoreApprovedRejectedNotificationModel>()
            {
                Data = notiModel,
                Type = GeneralFirebaseNotificationModel<StoreApprovedRejectedNotificationModel>.NotiType.StoreIsApproved
            });
        }
        public async Task SendNotificationStoreRejected(int storeId, int brandId, string storeName)
        {
            var notiModel = new StoreApprovedRejectedNotificationModel()
            {
                BrandId = brandId,
                StoreId = storeId,
                StoreName = storeName
            };
            await SendNotification(new GeneralFirebaseNotificationModel<StoreApprovedRejectedNotificationModel>()
            {
                Data = notiModel,
                Type = GeneralFirebaseNotificationModel<StoreApprovedRejectedNotificationModel>.NotiType.StoreIsRejected
            });
        }
        private async Task SendNotification(object _payload)
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
            var fcmSendResponse = await fcm.SendAsync(_payload);
        }

    }
}
