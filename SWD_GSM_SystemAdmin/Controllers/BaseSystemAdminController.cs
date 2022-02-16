using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_SystemAdmin.Controllers.SystemAdmin
{
    public class BaseSystemAdminController : BaseController
    {
        protected const string Role = "SystemAdmin";
        protected const string SystemAdminRoute = "api/" + Version + "/[controller]";
    }
}
