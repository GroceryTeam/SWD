using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public class AddUserToBrandCreateModel
    {
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public int BrandId { get; set; }
    }
}
