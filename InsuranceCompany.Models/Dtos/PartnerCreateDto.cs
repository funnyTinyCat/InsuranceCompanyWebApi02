using InsuranceCompany.Model.Enums;
using InsuranceCompany.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Model.Dtos
{
    public class PartnerCreateDto
    {
        public int Id { get; set; }
        [Required]
        [Length(2, 255, ErrorMessage = "FirstName must be between 2 and 255 characters.")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Length(2, 255, ErrorMessage = "LastName must be between 2 and 255 characters.")]
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{20}$", ErrorMessage = "The value must be a number exactly 20 digits long.")]
        public string PartnerNumber { get; set; } = string.Empty;
        [RegularExpression(@"^\d{11}$", ErrorMessage = "OIB must be a number exactly 11 digits long.")]
        public string? CroatianPIN { get; set; }
        [Required]
        public int PartnerTypeId { get; set; }
        // navigation property
        public PartnerType? PartnerType { get; set; }
        //[Required]
        public DateTime CreatedAtUtc { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string CreateByUser { get; set; } = string.Empty;
        [Required]
        public bool IsForeign { get; set; }
        [Length(10, 20, ErrorMessage = "External Code must be between 10 and 20 characters.")]
        public string ExternalCode { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        public List<Policy> Policies { get; set; } = new List<Policy>();
        public Gender Genders {  get; set; }
    }
}
