using System.Data.Entity;
using AlignUp.Domain.Model.User;

namespace AlignUp.BusinessLogic.DBModel
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=AlignUpConnection")
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public DbSet<UserDbTable> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
