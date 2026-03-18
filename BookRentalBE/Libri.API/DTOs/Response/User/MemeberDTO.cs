namespace Libri.API.DTOs.Response.User
{
    public class MemeberDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Roles { get; set; } = string.Empty;
    }
}