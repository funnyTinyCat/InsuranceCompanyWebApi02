using InsuranceCompany.Model.Dtos;
using InsuranceCompany.Model.Entities;
using InsuranceCompany.Model.Models;
using InsuranceCompany.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace InsuranceCompany.Web.Components.Pages.Partner
{
    public partial class IndexPartner
    {
        [Inject]
        ApiClient ApiClient { get; set; }
        public List<PartnerModel> partnerModels { get; set; } = new List<PartnerModel>();
        public PartnerModel partnerModel { get; set; }
        public PartnerDetailDto partnerDetailModel { get; set; } // = new PartnerDetailDto();
        public List<PartnerListDto> partnerListModels { get; set; } = new List<PartnerListDto>();

        public List<PartnerType> PartnerTypeModels { get; set; } = new List<PartnerType>();
        public AppModalDetail Modal { get; set; }
        public int partnerId { get; set; }


        protected async override Task OnInitializedAsync()
        { 
            await base.OnInitializedAsync();

            await LoadPartnersAsync();
        }

        protected async Task LoadPartnersAsync()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/partners");

            if (res != null && res.Success)
            {
                partnerModels = JsonConvert.DeserializeObject<List<PartnerModel>>(res.Data.ToString());
            }

            foreach ( var model in partnerModels)
            {

                PartnerType partnerType = PartnerTypeModels.FirstOrDefault(x =>  x.Id == model.PartnerTypeId);

                PartnerListDto item = new PartnerListDto
                {
                    Id = model.Id,
                    FullName = model.FirstName + " " + model.LastName,
                    PartnerNumber = model.PartnerNumber,
                    CroatianPIN = model.CroatianPIN,
                    PartnerTypeId = model.PartnerTypeId,  
                    PartnerType = (model.PartnerType == null ? null : new PartnerType
                    {
                        Id = model.PartnerType.Id,
                        Type = model.PartnerType.Type,
                        Title = model.PartnerType.Title
                    } ),
                    CreatedAtUtc = model.CreatedAtUtc,
                    IsForeign = model.IsForeign,
                    Gender = model.Gender
                };



                partnerListModels.Add(item);
            }
            
        }

        protected async Task LoadPartnerDetailAsync(int id)
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/partners/{id}");

            if (res != null && res.Success)
            {
                partnerModel = JsonConvert.DeserializeObject<PartnerModel>(res.Data.ToString());
            }

            // get partner by id?
            //partnerModel = await ApiClient.GetFromJsonAsync<PartnerModel>($"/partners/{id}");

            partnerDetailModel = new PartnerDetailDto
            {
                Id = partnerModel.Id,
                FullName = $"{partnerModel.FirstName} {partnerModel.LastName}",
                Address = partnerModel.Address,
                PartnerNumber = partnerModel.PartnerNumber,
                CroatianPIN = partnerModel.CroatianPIN,
                PartnerTypeId = partnerModel.PartnerTypeId,
                PartnerType = (partnerModel.PartnerType == null ? null : new PartnerType
                {
                    Id = partnerModel.PartnerTypeId,
                    Type = partnerModel.PartnerType.Type,
                    Title = partnerModel.PartnerType.Title
                }),
                CreatedAtUtc = partnerModel.CreatedAtUtc,
                CreateByUser = partnerModel.CreateByUser,
                IsForeign = partnerModel.IsForeign,
                ExternalCode = partnerModel.ExternalCode,
                Gender = partnerModel.Gender
            };

            await InvokeAsync(StateHasChanged);
        }

    }
}
