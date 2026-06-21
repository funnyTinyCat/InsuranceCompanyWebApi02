//using Blazored.Toast.Services;
using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace InsuranceCompany.Web.Components.Pages.Product
{
    public partial class UpdateProduct : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        public ProductModel Model { get; set; } = new();
        [Inject]
        private ApiClient ApiClient { get; set; }
        //[Inject]
        //private IToastService ToastService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/products/{Id}");

            if (res != null && res.Success)
            {
                Model = JsonConvert.DeserializeObject<ProductModel>(res.Data.ToString());
            }
        }

        public async Task Submit()
        {
            var res = await ApiClient.PutAsync<BaseResponseModel, ProductModel>($"/api/products/{Id}", Model);

            if (res != null && res.Success)
            {
                //ToastService.ShowSuccess("Update product successfully");
                NavigationManager.NavigateTo("/products");
            }
        }


    }
}
