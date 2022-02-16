using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_StoreOwner.Controllers.StoreOwner
{
    public class BaseStoreOwnerController : BaseController
    {
        protected const string Role = "StoreOwner";
        protected const string StoreOwnerRoute = "api/" + Version + "/[controller]";
    }
}
