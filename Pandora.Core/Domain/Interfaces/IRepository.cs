using Microsoft.EntityFrameworkCore.Query;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Paging;
using Pandora.Core.Persistence.Dynamic;
using System.Linq.Expressions;

namespace Pandora.Core.Domain.Interfaces;

public interface IRepository<TEntity, TEntityId> : IQuery<TEntity> where TEntity : Entity<TEntityId>
{
    TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true);

    IPaginate<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true);

    IPaginate<TEntity> GetListByDynamic(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true);

    bool Any(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false);

    TEntity Add(TEntity entity);

    ICollection<TEntity> AddRange(ICollection<TEntity> entities);

    TEntity Update(TEntity entity);

    ICollection<TEntity> UpdateRange(ICollection<TEntity> entities);

    TEntity Delete(TEntity entity, bool permanent = false);

    ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false);
}