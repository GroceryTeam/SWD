using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAcessLayer.Models.Brand;

namespace BusinessLayer.RequestModels.SearchModels.SystemAdmin
{
    public class BrandSearchModel
    {
        public int? UserId { get; set; }
        public string SearchTerm { get; set; }
        public BrandStatus? Status { get; set; }
    }
}
