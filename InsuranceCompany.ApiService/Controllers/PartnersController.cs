using InsuranceCompany.BL.Interfaces;
using InsuranceCompany.BL.Services;
using InsuranceCompany.Model.Dtos;
using InsuranceCompany.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceCompany.ApiService.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _partnerService;
        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PartnerCreateDto>>> GetAllPartners()
        {
            var partners = await _partnerService.GetAllPartnersAsync();

            return Ok(new BaseResponseModel
            {
                Success = true,
                Data = partners
            });
        }

     
        [HttpGet("{id}")]
        public async Task<ActionResult<PartnerCreateDto>> GetPartnerAsync(int id)
        {
            var partner = await _partnerService.GetPartnerAsync(id);

            if (partner == null)
                return NotFound("The partner is not found.");

            return Ok(new BaseResponseModel
            {
                Success = true,
                Data = partner
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreatePartnerAsync(PartnerCreateDto partner)
        {
            if (partner == null)
                return BadRequest();

            var createdId = await _partnerService.CreatePartnerAsync(partner);

            //return CreatedAtAction("GetPartner", new { Id = createdId }, partner);
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePartnerAsync(int id, PartnerCreateDto partner)
        {
            if (partner == null || partner.Id != id)
            {
                return BadRequest();
            }

            var existingPartner = await _partnerService.GetPartnerAsync(id);

            if (existingPartner == null)
                return NotFound();

            await _partnerService.UpdatePartnerAsync(id, partner);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePartnerAsync(int id)
        {
            var existingPartner = _partnerService.GetPartnerAsync(id);

            if (existingPartner == null)
                return NotFound();

            await _partnerService.DeletePartnerAsync(id);

            return NoContent();
        }

        [HttpGet("partnertypes")]
        public async Task<ActionResult<List<PartnerTypeDto>>> GetAllPartnerTypesAsync()
        {
            var partnerTypes = await _partnerService.GetAllPartnerTypesAsync();

            return Ok(new BaseResponseModel
            {
                Success = true,
                Data = partnerTypes
            });
        }

    }
}
