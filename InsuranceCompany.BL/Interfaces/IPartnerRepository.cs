using InsuranceCompany.Model.Dtos;
using InsuranceCompany.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.BL.Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<PartnerCreateDto>> GetAllPartnersAsync();
        Task<PartnerCreateDto> GetPartnerAsync(int id);
        Task<int> CreatePartnerAsync(PartnerCreateDto partner);
        Task UpdatePartnerAsync(int id, PartnerCreateDto partner);
        Task DeletePartnerAsync(int id);
        Task<IEnumerable<PartnerTypeDto>> GetAllPartnerTypesAsync();
    }
}
