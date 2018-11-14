using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data.Configuration
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("Groups");
            Property(g => g.Id).IsRequired();
            Property(g => g.Name).IsRequired();
        }
    }
}
