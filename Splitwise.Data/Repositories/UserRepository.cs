using Splitwise.Data.Infrastracture;
using Splitwise.Data.Infrastructure;
using Splitwise.Models;

namespace Splitwise.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory) {

        }

    }

    public interface IUserRepository : IRepository<User>
    {

    }
}
