using InsuranceCompany.BL.Interfaces;
using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCompany.ApiService.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetProductsAsync()
        {
            var products = await productService.GetProductsAsync();

            return Ok(new BaseResponseModel
                    {
                        Success = true,
                        Data = products
                    }
                );
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct(ProductModel productModel)
        {
            await productService.CreateProductAsync(productModel);

            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModel>> GetProductByIdAsync(int id)
        {
            var productModel = await productService.GetProductByIdAsync(id);

            if (productModel == null)
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }

            return Ok(new BaseResponseModel { Success = true, Data = productModel });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductModel productModel)
        {
            if (id != productModel.Id || !await productService.ProductModelExistsAsync(id))
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
            }

            await productService.UpdateProductAsync(id, productModel);

            return Ok(new BaseResponseModel { Success = true });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            if (!await productService.ProductModelExistsAsync(id))
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }

            await productService.DeleteProductAsync(id);

            return Ok(new BaseResponseModel { Success = true });
        }
    }
}
