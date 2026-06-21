using InsuranceCompany.BL.Interfaces;
using InsuranceCompany.Database.Data;
using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.BL.Repositories
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<ProductModel> CreateProductAsync(ProductModel productModel)
        {
            await context.Products.AddAsync(productModel);
            await context.SaveChangesAsync();

            return productModel;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
           var product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return null;
            }

            return product;
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<bool> ProductModelExistsAsync(int id)
        {
            return await context.Products.AnyAsync(x => x.Id == id);
        }

        public async Task UpdateProductAsync(int id, ProductModel productModel)
        {
            context.Entry(productModel).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
