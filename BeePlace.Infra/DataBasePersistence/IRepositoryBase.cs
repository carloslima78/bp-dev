using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace BeePlace.Infra.DataBasePersistence
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        void Insert(ref TEntity entity);

        bool Update(TEntity entity);

        bool Delete(TEntity entity);

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetList(CommandType commandType, string sql, object parameters = null);
    }
}
