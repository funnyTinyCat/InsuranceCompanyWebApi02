using InsuranceCompany.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Model.Entities
{
    public class PartnerListDto
    {
        public int Id { get; set; }
        // firstname + lastname
        public string FullName { get; set; } = string.Empty;
        public string PartnerNumber { get; set; } = string.Empty;
        public string CroatianPIN { get; set; } = string.Empty;
        public int PartnerTypeId { get; set; }
        // navigation property
        public PartnerType? PartnerType { get; set; }
        //[Required]
        public DateTime CreatedAtUtc { get; set; }
        public bool IsForeign { get; set; }
        public string Gender { get; set; } = string.Empty;
    }
}
