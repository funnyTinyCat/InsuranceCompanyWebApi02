using InsuranceCompany.BL.Interfaces;
using InsuranceCompany.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.BL.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public async Task<ProductModel> CreateProductAsync(ProductModel productModel)
        {
            return await productRepository.CreateProductAsync(productModel);
        }

        public async Task DeleteProductAsync(int id)
        {
            await productRepository.DeleteProductAsync(id);
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            return await productRepository.GetProductByIdAsync(id);
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            return await productRepository.GetProductsAsync();
        }

        public Task<bool> ProductModelExistsAsync(int id)
        {
            return productRepository.ProductModelExistsAsync(id);
        }

        public Task UpdateProductAsync(int id, ProductModel productModel)
        {
            return productRepository.UpdateProductAsync(id, productModel); 
        }
    }
}
