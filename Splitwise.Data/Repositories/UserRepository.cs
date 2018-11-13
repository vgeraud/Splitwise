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
