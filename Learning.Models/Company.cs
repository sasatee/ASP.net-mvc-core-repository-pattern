using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class Company
    {

        
        public int CompanyId { get; set; }
        [Required]
        [DisplayName("Company Name")]
        [MaxLength(30)]
        public string CompanyName { get; set; }
        public string StreetAddress {  get; set; }  
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }


    }
}
