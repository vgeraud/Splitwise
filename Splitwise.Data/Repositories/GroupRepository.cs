using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Models;

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
