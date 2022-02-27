using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Firebase
{
    public class StoreApprovedRejectedNotificationModel
    {
        public string StoreName { get; set; }
        public int StoreId { get; set; }

        public int BrandId { get; set; }
    }
}
