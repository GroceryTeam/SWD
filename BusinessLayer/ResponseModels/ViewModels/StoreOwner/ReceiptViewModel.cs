using DataAcessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.ViewModels.StoreOwner
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public DateTime DateCreated { get; set; }
        public int TotalCost { get; set; }

        public List<ReceiptDetailViewModel> ReceiptDetails { get; set; }
    }
}
