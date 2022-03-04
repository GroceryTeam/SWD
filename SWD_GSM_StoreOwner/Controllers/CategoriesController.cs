using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.StoreOwner;
using BusinessLayer.RequestModels;
using BusinessLayer.RequestModels.CreateModels.StoreOwner;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD_GSM_StoreOwner.Controllers.StoreOwner;
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
    public class CategoriesController : BaseStoreOwnerController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService service)
        {
            _categoryService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int BrandId, [FromQuery] PagingRequestModel paging)
        {
            paging = PagingUtil.checkDefaultPaging(paging);
            var categories = await _categoryService.GetCategoryList(BrandId, paging);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCategory([FromBody] CategoryCreateModel model)
        {
            try
            {
                var id = await _categoryService.AddCategory(model.BrandId, model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryCreateModel model)
        {
            try
            {
                if (model.Name.Length == 0)
                {
                    return BadRequest();
                }
                await _categoryService.UpdateCategory(id, model.Name);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

    }
}
