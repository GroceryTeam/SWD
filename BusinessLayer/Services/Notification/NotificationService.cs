using BusinessLayer.Interfaces.Notification;
using BusinessLayer.ResponseModels.Firebase;
using CorePush.Google;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly FirebaseApp _firebaseApp;
        public NotificationService(FirebaseApp firebaseApp)
        {
            _firebaseApp = firebaseApp;
        }

        public async Task SendNotificationOutOfStockProduct(int productId, int brandId, string productName)
        {
            var notiModel = new OutOfStockFirebaseNotificationModel()
            {
                BrandId = brandId,
                ProductId = productId,
                ProductName = productName
            };
            string title = "Sản phẩm hết hàng";
            string body = $"Sản phẩm {productName} sắp hết hàng. Hãy nhập hàng ngay.";
            await SendNotification(notiModel,"OutOfStock", title, body);
        }
        public async Task SendNotificationStoreApproved(int storeId, int brandId, string storeName)
        {
            var notiModel = new StoreApprovedRejectedNotificationModel()
            {
                BrandId = brandId,
                StoreId = storeId,
                StoreName = storeName
            };
            string title = "Yêu cầu được phê duyệt";
            string body = $"Yêu cầu mở tiệm \"{storeName}\" của bạn đã được admin phê duyệt. Hãy bắt đầu quản lý ngay nào.";
            await SendNotification(notiModel, "StoreApproved", title,body);
        }
        public async Task SendNotificationStoreRejected(int storeId, int brandId, string storeName)
        {
            var notiModel = new StoreApprovedRejectedNotificationModel()
            {
                BrandId = brandId,
                StoreId = storeId,
                StoreName = storeName
            };
            string title = "Yêu cầu bị từ chối";
            string body = $"Yêu cầu mở tiệm \"{storeName}\" của bạn đã bị từ chối. Liên hệ chúng tôi để biết thêm chi tiết.";
            await SendNotification(notiModel, "StoreRejected",title, body);
        }
        private async Task SendNotification(object data,string topic, string title, string body)
        {
            try
            {
                var dataForMesssage = new Dictionary<string, string>();

                foreach (PropertyInfo prop in data.GetType().GetProperties())
                {
                    dataForMesssage.Add(prop.Name, prop.GetValue(data).ToString());
                }
                var message = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Body = body,
                        Title = title
                    },
                    Topic = "all",
                    Data = dataForMesssage,
                };
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("Successfully sent message: " + response);
            }catch (Exception e)
            {
                Console.WriteLine("Successfully sent message: " + e.Message);
            }
        }

    }
}
