using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data
{
    interface IGenericViewRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> Get(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    string includeProperties = "");
    }
}

