//using Blazored.Toast.Services;
using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Models;
using Microsoft.AspNetCore.Components;

namespace InsuranceCompany.Web.Components.Pages.Product
{
    public partial class CreateProduct
    {
        public ProductModel Model { get; set; } = new();
        [Inject]
        private ApiClient ApiClient { get; set; } 
        //[Inject]
        //private IToastService ToastService { get; set; } 
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public async Task Submit()
        {
            var res = await ApiClient.PostAsync<BaseResponseModel, ProductModel>("/api/products", Model);

            if (res != null && res.Success)
            {
                //ToastService.ShowSuccess("Create product successfully");
                NavigationManager.NavigateTo("/products");
            }
        }
    }
}
