using Splitwise.Data.Infrastracture;

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
