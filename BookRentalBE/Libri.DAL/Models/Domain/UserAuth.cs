namespace Libri.DAL.Models.Domain
{
    public partial class UserAuth
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public DateTime? LastLoggedIn { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int UserInfoId { get; set; }
        public virtual UserInfo UserInfo { get; set; } = null!;
    }
}
