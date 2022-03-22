using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.SystemAdmin;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels.SystemAdmin;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_SystemAdmin.Controllers.SystemAdmin
{
    [Route(SystemAdminRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    [Authorize(Roles = Role)]

    public class BrandsController : BaseSystemAdminController
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var paging = PagingUtil.getDefaultPaging();
                var user = await _brandService.GetBrandById(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BrandSearchModel searchModel, [FromQuery] PagingRequestModel paging)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }
            try
            {
                paging = PagingUtil.checkDefaultPaging(paging);
                var users = await _brandService.GetBrandList(searchModel, paging);
                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandService.DisableBrand(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpPut("{id}/enable")]
        public async Task<IActionResult> EnableUser(int id)
        {
            try
            {
                await _brandService.EnableBrand(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}

