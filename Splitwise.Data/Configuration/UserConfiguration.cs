using Splitwise.Models;
using System.Data.Entity.ModelConfiguration;

namespace Splitwise.Data.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            Property(g => g.Id).IsRequired();
            Property(g => g.Username).IsRequired();
            Property(g => g.Password).IsRequired();
            Property(g => g.Email).IsRequired();
            Property(g => g.Currency).IsRequired();
        }
    }
}
