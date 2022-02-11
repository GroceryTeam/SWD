using BusinessLayer.Interfaces.Cashier;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.ResponseModels.ViewModels;
using BusinessLayer.ResponseModels.ViewModels.Cashier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace SWD_GSM.Controllers.Cashier
{
    [Route(CashierRoute)]
    [ApiController]
    [ApiExplorerSettings(GroupName = Role)]
    public class LoginController : BaseCashierController
    {
        private readonly ICashierService _cashierService;

        public LoginController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (login == null)
            {
                return BadRequest();
            }
            var cashier = await _cashierService.Login(login);
            if (cashier != null)
            {
                string tokenString = CreateAuthenToken.GetToken(Role);
                return Ok(new BaseLoginViewModel<CashierViewModel>()
                {
                    Token = tokenString,
                    Information = cashier
                });
            }
            else
            {
                return Unauthorized();
            }
        }


    }
}
