using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreHub.Core.Services.Interfaces;
using StoreHub.Helpers;
using StoreHub.Models.DTOs;
using StoreHub.Validations;

namespace StoreHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService _categoryService )
        {
            categoryService = _categoryService;
        }

        [HttpGet("Get-All-Categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            if (categories == null)
                return NotFound(new ApiErrorResponse(401));

            return Ok(categories);
        }
             
        [HttpGet("Get-Category-By{Id:int}", Name = "CategoryDetails")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int Id)
        {
            var category = await categoryService.GetCategoryByIdAsync(Id);
            if (category == null) 
                return NotFound(new ApiErrorResponse(404));
            return Ok(category);

        }


        [HttpGet("Get-Category-With-Products-By{Id:int}")]
        public async Task<IActionResult> GetCategoryByIdWithProducts([FromRoute] int Id)
        {
            var category = await categoryService.GetCategoryByIdWithProductsAsync(Id);
            if (category == null)
                return NotFound(new ApiErrorResponse(404));

            return Ok(category);

        }

        [HttpGet("Get-All-Categories-With-Products")]
        public async Task<IActionResult> GetAllCategoriesWithProducts()
        {
            var Categories = await categoryService.GetAllCategoriesWithProductsAsync();
            if(Categories == null)
                return NotFound(new ApiErrorResponse(404));

            return Ok(Categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add-New-Category")]
        public async Task<IActionResult> AddNewCategory(CategoryDTO newCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetModelStateErrors();
                    return BadRequest(new { Errors = errors });
                }

                var CreatedCategory = await categoryService.AddNewCategoryAsync(newCategory);
                if (CreatedCategory == null)
                    return BadRequest(new ApiErrorResponse(500));

                var URL = Url.Link("CategoryDetails", new { Id = CreatedCategory?.ID });
                return Created(URL, CreatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update-Category-By{Id:int}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int Id, [FromBody] CategoryDTO Category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetModelStateErrors();
                    return BadRequest(new { Errors = errors });
                }

                bool isUpdated = await categoryService.UpdateCategoryAsync(Id, Category);
                if (!isUpdated)
                    return NotFound(new ApiErrorResponse(404));

                return Ok("Updated Succefully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-Catgeory-By{Id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int Id)
        {
            try
            {
                bool isDeleted = await categoryService.DeleteCategoryAsync(Id);
                if (!isDeleted)
                    return NotFound(new ApiErrorResponse(404));

                return Ok($"Category With Id: {Id} Deleted Succefully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}
