using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Models;
using InsuranceCompany.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace InsuranceCompany.Web.Components.Pages.Product
{
    public partial class IndexProduct
    {
        [Inject]
        public ApiClient ApiClient { get; set; }
        public List<ProductModel> ProductModels { get; set; }
        public AppModal Modal { get; set; }
        public int DeleteId { get; set; }
        protected async override Task OnInitializedAsync()
        {
            //var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/products"); 

            //if (res != null && res.Success)
            //{
            //    ProductModels = JsonConvert.DeserializeObject<List<ProductModel>>(res.Data.ToString());
            //}

            await base.OnInitializedAsync();

            await LoadProductAsync();
        }

        protected async Task LoadProductAsync()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/products");

            if (res != null && res.Success)
            {
                ProductModels = JsonConvert.DeserializeObject<List<ProductModel>>(res.Data.ToString());
            }
        }
        protected async Task HandleDeleteAsync()
        {
            var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/products/{DeleteId}");

            if (res != null && res.Success)
            {
                //ToastService.ShowSuccess("Delete product successfully");
                await LoadProductAsync();
                Modal.Close();
            }
        }
    }
}
