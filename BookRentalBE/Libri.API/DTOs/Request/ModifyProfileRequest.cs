using System.ComponentModel.DataAnnotations;
using Libri.API.DTOs.Response;
using Stripe;

namespace Libri.API.DTOs.Request;

public class ModifyProfileRequest
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
    
    [Required] 
    public string UserName { get; set; }
    
    [Required]    
    public string Gender { get; set; }
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
}