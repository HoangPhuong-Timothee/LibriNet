using System.ComponentModel.DataAnnotations;

namespace Libri.API.DTOs.Request
{
    public class ModifyAddressRequest
    {
        [Required]
        public string FullName { get; set; }
        
        [Required]
        public string Street { get; set; }
        
        [Required]
        public string Ward { get; set; }
        
        [Required]
        public string District { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string PostalCode { get; set; }
    }
}
