namespace Libri.DAL.Models.Custom.CustomUser;

public class CurrentUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string ImageUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
}
