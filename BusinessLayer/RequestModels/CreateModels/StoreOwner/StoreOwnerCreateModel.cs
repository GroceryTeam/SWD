using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.User;

namespace BusinessLayer.RequestModels.CreateModels.StoreOwner
{
    public class StoreOwnerCreateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public UserStatus Status { get; set; }
    }
}
