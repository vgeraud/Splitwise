using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data.Repositories
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(IDbFactory dbFactory)
            : base(dbFactory) {

        }

    }

    public interface IGroupRepository : IRepository<Group>
    {

    }
}
