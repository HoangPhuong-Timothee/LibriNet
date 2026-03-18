namespace Libri.DAL.Models.Domain
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<UserInfo>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<UserInfo> Users { get; set; }
    }
}
