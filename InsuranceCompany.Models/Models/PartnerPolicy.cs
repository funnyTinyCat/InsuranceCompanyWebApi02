using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Model.Models
{
    public class PartnerPolicy
    {
        public int Id { get; set; }
        [Required]
        public int PartnerId { get; set; }
        [Required]
        public int PolicyId { get; set; }
    }
}
