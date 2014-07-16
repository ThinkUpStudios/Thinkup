using System;
using System.Linq;
using System.Linq.Expressions;
using ThinkUp.Sdk.Data.Entities;

namespace ThinkUp.Sdk.Data
{
    public interface IRepository<T> where T : DataEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);

        T Get(Expression<Func<T, bool>> predicate = null);

        bool Exist(Expression<Func<T, bool>> predicate = null);

        ///<exception cref="DataException">DataException</exception>
        void Create(T dataEntity);

        ///<exception cref="DataException">DataException</exception>
        void Update(T dataEntity);

        ///<exception cref="DataException">DataException</exception>
        void Delete(T dataEntity);

        ///<exception cref="DataException">DataException</exception>
        void DeleteAll();
    }
}
