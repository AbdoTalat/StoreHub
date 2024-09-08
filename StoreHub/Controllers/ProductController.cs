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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Get-All-Products")]
        public async Task<IActionResult> GetAllProduct()
        {
            var Products = await _productService.GetAllProductsAsync();
            if (Products == null)
                return NotFound(new ApiErrorResponse(404));

            return Ok(Products);
        }


        [HttpGet("Get-Product-By{Id:int}", Name = "ProductDetails")]
        public async Task<IActionResult> GetProdcutById([FromRoute] int Id)
        {
            var product = await _productService.GetProductByIdAsync(Id);
            if (product == null)
                return NotFound(new ApiErrorResponse(404));

            return Ok(product);
        }

        [HttpGet("Searching-And-Paging-For-All-Products")]
        public async Task<IActionResult> SearchCategoriesWithPaging(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? categoryName = null,
            [FromQuery] string? productName = null)
        {
            var (totalItems, products) = await _productService.SearchProductsWithPaging(pageNumber, pageSize, categoryName ?? "", productName ?? "");
            if ((totalItems, products) == (0, null))
                return BadRequest(new ApiErrorResponse(400));

            var result = new
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Products = products
            };
            return Ok(result);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("Add-new-Product")]
        public async Task<IActionResult> AddNewProduct(ProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetModelStateErrors();
                    return BadRequest(new { Errors = errors });
                }
                var CreatedProduct = await _productService.AddNewProductWithImageAsync(product);
                if (CreatedProduct == null)
                    return BadRequest(new ApiErrorResponse(500));

                string? URL = Url.Link("ProductDetails", new { Id = CreatedProduct?.ID });
                return Created(URL, CreatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update-Product-By{Id:int}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int Id, [FromBody] ProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.GetModelStateErrors();
                    return BadRequest(new { Errors = errors });
                }
                bool isUpdated = await _productService.UpdateProductAsync(Id, product);
                if (!isUpdated)
                    return NotFound(new ApiErrorResponse(404));

                return Ok(product);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-Product-By{Id:int}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int Id)
        {
            try
            {
                bool IsDeleted = await _productService.DeleteProductWithImageAsync(Id);
                if (!IsDeleted)
                    return NotFound(new ApiErrorResponse(404));

                return Ok("Deleted succefully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
