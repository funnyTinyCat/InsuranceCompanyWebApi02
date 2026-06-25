using InsuranceCompany.Model.Dtos;
using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Enums;
using InsuranceCompany.Model.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace InsuranceCompany.Web.Components.Pages.Partner
{
    public partial class CreatePartner
    {
        //public PartnerModel Model { get; set; } = new();
        public PartnerCreateDto Model { get; set; } = new();
        public List<PartnerTypeDto> PartnerTypeModels { get; set; } = new();
        //public Gender Genders { get; set; }
        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        // methods

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadPartnerTypesAsync();

            Model.PartnerTypeId = 2;
            Model.Gender = "M";

            await InvokeAsync(StateHasChanged);
        }

        protected async Task LoadPartnerTypesAsync()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/partners/partnertypes");

            if (res != null && res.Success)
            {
                PartnerTypeModels = JsonConvert.DeserializeObject<List<PartnerTypeDto>>(res.Data.ToString());
            }


            //foreach (var model in partnerTypes)
            //{
            //    PartnerTypeDto item = new PartnerTypeDto
            //    {
            //        Id = model.Id,
            //        Type = model.Type,
            //        Title = model.Title
            //    };

            //    partnerListModels.Add(item);
            //}

        }

        public async Task Submit()
        {
            var res = await ApiClient.PostAsync<BaseResponseModel, PartnerCreateDto>("/api/partners", Model);

            if (res != null && res.Success)
            {
                //ToastService.ShowSuccess("Create product successfully");
                NavigationManager.NavigateTo("/partners");
            }
        }
    }
}
