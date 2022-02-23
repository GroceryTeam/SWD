using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.StoreOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces.StoreOwner
{
    public interface IReceiptService
    {
        Task<ReceiptViewModel> GetReceiptById(int storeId, int receiptId);
        Task<BasePagingViewModel<ReceiptViewModel>> GetReceiptList(int storeId, ReceiptSearchModel model, PagingRequestModel paging);
        Task AddReceipt(ReceiptCreateModel model);
    }
}
