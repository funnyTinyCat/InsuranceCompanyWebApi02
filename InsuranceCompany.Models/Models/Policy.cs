using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Model.Models
{
    public class Policy
    {
        public int Id { get; set; }
        [Required]
        [Length(10, 15, ErrorMessage = "PolicyNumber Length must be between 10 and 15 characters.")]
        public string PolicyNumber { get; set; } = string.Empty;
        [Required]
        public decimal PolicyAmount { get; set; }

        public List<Partner> Partners { get; set; } = new List<Partner>();
    }
}
