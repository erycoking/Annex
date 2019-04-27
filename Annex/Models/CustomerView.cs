using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Annex.Models
{
    public class CustomerView
    {
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string OtherNames { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 1)]
        public string Address { get; set; }
        [Required]
        public int NationalId { get; set; }
        [Required]
        public int MobileNo { get; set; }
        [Required]
        public HttpPostedFile Photo { get; set; }
    }
}