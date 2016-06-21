using System;
using System.Linq;

namespace Common.Entity
{
    public interface IRepository
    {
        void Insert<T>(T item) where T : IEntity, new();
        void Update<T>(T item) where T : IEntity, new();
        void Delete<T>(T item) where T : IEntity, new();
        IQueryable<T> GetAll<T>() where T : IEntity, new();
        IQueryable<T> Find<T>(Func<T, bool> predicate) where T : IEntity, new();
        T Single<T>(Func<T, bool> predicate) where T : IEntity, new();
    }
}
