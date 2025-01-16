using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendShop.Core.Dto.Category;
using BackendShop.Core.Dto.SubCategory;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using BackendShop.Data.Data;
using BackendShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendShop.BackShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        // GET: api/SubCategory
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var list = await _subCategoryService.GetListAsync();
            return Ok(list);
        }

        // GET: api/SubCategory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _subCategoryService.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("byCategory/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(int categoryId)
        {
            var list = await _subCategoryService.GetByCategoryIdAsync(categoryId);
            return Ok(list);
        }

        // POST: api/SubCategory/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreateSubCategoryDto model)
        {
            try
            {
                await _subCategoryService.CreateAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/SubCategory/{id}
        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] EditSubCategoryDto model)
        {
            try
            {
                await _subCategoryService.EditAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/SubCategory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _subCategoryService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

