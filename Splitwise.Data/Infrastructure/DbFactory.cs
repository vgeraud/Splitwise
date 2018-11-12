using Splitwise.Data.Infrastracture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splitwise.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        SplitwiseContext dbContext;

        public SplitwiseContext Init()
        {
            return dbContext ?? (dbContext = new SplitwiseContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
