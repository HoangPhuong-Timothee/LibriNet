namespace Libri.DAL.Models.Domain
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            Roles = new HashSet<Role>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ImagePublicId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual UserAuth? UserAuth { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
