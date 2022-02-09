using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.User;

namespace BusinessLayer.RequestModels.SearchModels.StoreOwner
{
    public class UserSearchModel
    {
        public string SearchTerm { get; set; }
        public UserStatus? Status { get; set; }
    }
}
