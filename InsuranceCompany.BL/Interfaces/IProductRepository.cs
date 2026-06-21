using InsuranceCompany.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.BL.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetProductsAsync();
        Task<ProductModel> CreateProductAsync(ProductModel productModel);
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<bool> ProductModelExistsAsync(int id);
        Task UpdateProductAsync(int id, ProductModel productModel);

        Task DeleteProductAsync(int id);

    }
}
