using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data.Infrastracture
{
    public interface IDbFactory : IDisposable
    {
        SplitwiseContext Init();
    }
}
