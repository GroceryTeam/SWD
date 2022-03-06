using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.RequestModels.SearchModels;
using BusinessLayer.RequestModels.SearchModels.StoreOwner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD_GSM_StoreOwner.Controllers.StoreOwner
{
    [Route(StoreOwnerRoute)]
    [ApiController]
    //[ApiExplorerSettings(GroupName = Role)]
    //[Authorize(Roles = Role)]
    public class BrandsController : BaseStoreOwnerController
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int UserId)
        {
            try
            {
                var brands = await _brandService.GetBrandList(UserId);
                return Ok(brands);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("user")]
        public async Task<IActionResult> AddUserToBrand([FromBody] AddUserToBrandCreateModel model)
        {
            try
            {
                var result = await _brandService.AddUserToBrand(model.PhoneNo, model.Email, model.BrandId);
                if (result == true)
                {
                    return Ok();

                }
                else
                {
                    return Conflict("Email/Phone number doesn't exist");
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewBrand([FromBody] BrandCreateModel model)
        {
            try
            {
                await _brandService.AddBrand(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
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
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUserFromBrand(int id, int BrandId)
        {
            try
            {
                await _brandService.DeleteUserFromBrand(id, BrandId);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}

