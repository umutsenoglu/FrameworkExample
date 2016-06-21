using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Common
{
    public interface  IDataAccess:IDisposable
    {
        T Insert<T>(T entity)where T:BaseEntity,new ();
        void Update<T>(T entity) where T : BaseEntity, new();
        void Delete<T>(T entity) where T : BaseEntity, new();
        IEnumerable<T> GetAll<T>() where T : BaseEntity, new();
        void RollBack();
        void Commit();
    }
    
    public class BaseEntity
    {
        [Key(IsIdentity =true)]
        [Field(Name ="Id",IsNullable =false,DbType =DbType.Int64)]
        public virtual long Id { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
        public bool IsIdentity { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
        public DbType DbType { get; set; }
        public bool IsNullable { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string TableName { get; set; }
    }
}
