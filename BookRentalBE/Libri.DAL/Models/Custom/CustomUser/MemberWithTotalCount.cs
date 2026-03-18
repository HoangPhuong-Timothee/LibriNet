namespace Libri.DAL.Models.Custom.CustomUser
{
    public class MemberWithTotalCount
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; } = string.Empty;
        public int TotalCount { get; set; }
    }
}