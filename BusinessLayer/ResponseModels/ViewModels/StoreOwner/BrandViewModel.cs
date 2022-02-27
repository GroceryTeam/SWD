using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Brand;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public List<UserViewModel> UserList { get; set; }

        public List<StoreViewModel> StoreList { get; set; }
    }
}
