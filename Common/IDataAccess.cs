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
    
    /// <summary>
    /// Todo:Haziran - delete getall yazılsın
    /// Todo:Haziran - getall için filtre operasyonlarını hep birlikte tartışalım
    /// Todo:Temmuz - xunit deneyelim
    /// </summary>

    public abstract class BaseEntity
    {
        [Key(IsIdentity = true,Name = "Id", IsNullable = false, DbType = DbType.Int64)]
        public virtual object Id { get; set; }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : FieldAttribute
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
