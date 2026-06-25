using InsuranceCompany.BL.Interfaces;
using InsuranceCompany.Model.Dtos;
using InsuranceCompany.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.BL.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly IPartnerRepository _partnerRepository;
        public PartnerService(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }
        public async Task<IEnumerable<PartnerCreateDto>> GetAllPartnersAsync()
        {
            return await _partnerRepository.GetAllPartnersAsync();
        }

        public async Task<PartnerCreateDto?> GetPartnerAsync(int id)
        {
            return await _partnerRepository.GetPartnerAsync(id);
        }

        public async Task<int> CreatePartnerAsync(PartnerCreateDto partner)
        {
            return await _partnerRepository.CreatePartnerAsync(partner);
        }

        public async Task UpdatePartnerAsync(int id, PartnerCreateDto partner)
        {
            await _partnerRepository.UpdatePartnerAsync(id, partner);
        }

        public async Task DeletePartnerAsync(int id)
        {
            await _partnerRepository.DeletePartnerAsync(id);
        }

        public async Task<IEnumerable<PartnerTypeDto>> GetAllPartnerTypesAsync()
        {
            return await _partnerRepository.GetAllPartnerTypesAsync();
        }
    }
}
