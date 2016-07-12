using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Data;
using DataAccess.Helper;

namespace DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        SqlConnection _sqlConnection;
        SqlTransaction _transAction;

        public SqlDataAccess(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();

        }

        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            _transAction.Commit();
        }
        /// <summary>
        /// 
        /// </summary>
        public void RollBack()
        {
            _transAction.Rollback();
        }





        public T Insert<T>(T entity) where T : BaseEntity, new()
        {
            try
            {
                _transAction = _sqlConnection.BeginTransaction();
                string commandText = "insert into ";
                StringBuilder sqlBuilder = new StringBuilder(commandText);
                var tableName = typeof(T).Name;
                sqlBuilder.Append($" {tableName} ");
                var properties = entity.GetType().GetProperties();

                Dictionary<string, DbType> fieldNames = new Dictionary<string, DbType>();

                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(true);

                    foreach (var attribute in attributes)
                    {
                        if (typeof(KeyAttribute).IsAssignableFrom(attribute.GetType()))
                        {
                            var keyAttribute = attribute as KeyAttribute;
                            if (keyAttribute.IsIdentity) break;
                        }
                        if (typeof(FieldAttribute).IsAssignableFrom(attribute.GetType()))
                        {
                            var fieldAttribute = attribute as FieldAttribute;
                            fieldNames.Add(fieldAttribute.Name, fieldAttribute.DbType);
                        }
                    }
                }
                var sqlFieldFormatted = string.Join(",", fieldNames.Keys);
                sqlBuilder.Append($"({sqlFieldFormatted})");
                var sqlValueFormatted = string.Join(",", fieldNames.Keys.Select(s => "@" + s));
                sqlBuilder.Append($"values({sqlValueFormatted});\n");

                sqlBuilder.Append($"select scope_identity()");
                var sqlCommand = _sqlConnection.CreateCommand();

                sqlCommand.Transaction = _transAction;
                sqlCommand.CommandText = sqlBuilder.ToString();

                foreach (var fieldName in fieldNames)
                {
                    var parameter = sqlCommand.CreateParameter();
                    parameter.ParameterName = fieldName.Key;
                    parameter.Value = entity.GetType().GetProperty(fieldName.Key).GetValue(entity);
                    parameter.DbType = fieldName.Value;
                    sqlCommand.Parameters.Add(parameter);
                }
                entity.Id = Convert.ToInt64(sqlCommand.ExecuteScalar());
            }
            catch
            {
                _transAction.Rollback();
                throw;
            }

            return entity;

        }



        #region IDisposable
        /// <summary>
        /// 
        /// </summary>
        /// 
        ~SqlDataAccess()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        private void Dispose(bool v)
        {
            if (v)
            {
                if (_sqlConnection != null)
                {
                    _sqlConnection.Close();
                    _sqlConnection.Dispose();
                    _sqlConnection = null;
                }
            }
        }
        /// <summary>
        /// Todo:Umut
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : BaseEntity, new()
        {
            try
            {
                _transAction = _sqlConnection.BeginTransaction();
                string commandText = "update ";
                StringBuilder sqlBuilder = new StringBuilder(commandText);
                var tableName = typeof(T).Name;
                sqlBuilder.Append($" {tableName} ");
                var properties = entity.GetType().GetProperties();

                Dictionary<string, DbType> fieldNames = new Dictionary<string, DbType>();
                Dictionary<string, DbType> keyNames = new Dictionary<string, DbType>();


                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(true);
                    foreach (var attribute in attributes)
                    {
                        if (typeof(KeyAttribute).IsAssignableFrom(attribute.GetType()))
                        {


                            var keyAttribute = attribute as KeyAttribute;

                            keyNames.Add(keyAttribute.Name, keyAttribute.DbType);
                            break;
                        }
                        if (typeof(FieldAttribute).IsAssignableFrom(attribute.GetType()))
                        {
                            var fieldAttribute = attribute as FieldAttribute;
                            fieldNames.Add(fieldAttribute.Name, fieldAttribute.DbType);
                        }
                    }

                }
                var sqlFieldFormatted = string.Join(",", fieldNames.Keys.Select(s => s + " = " + "@" + s));
                sqlBuilder.Append($"set {sqlFieldFormatted}\n");
                var sqlKeyFormatted = string.Join(" and ", keyNames.Keys.Select(s => s + " = " + "@" + s));
                sqlBuilder.Append($"where {sqlKeyFormatted}\n");


                var sqlCommand = _sqlConnection.CreateCommand();

                sqlCommand.Transaction = _transAction;
                sqlCommand.CommandText = sqlBuilder.ToString();
                keyNames.ToList().ForEach(fr => fieldNames.Add(fr.Key, fr.Value));
                foreach (var fieldName in fieldNames)
                {
                    var parameter = sqlCommand.CreateParameter();
                    parameter.ParameterName = fieldName.Key;
                    parameter.Value = entity.GetType().GetProperty(fieldName.Key).GetValue(entity);
                    parameter.DbType = fieldName.Value;
                    sqlCommand.Parameters.Add(parameter);
                }
                sqlCommand.ExecuteNonQuery();

            }
            catch
            {
                _transAction.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Todo:Devrim
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : BaseEntity, new()
        {
           
            try
            {
                _transAction = _sqlConnection.BeginTransaction();
                string commandText = "delete from ";
                StringBuilder sqlBuilder = new StringBuilder(commandText);
                var tableName = typeof(T).Name;
                sqlBuilder.Append($" {tableName} ");
                var properties = entity.GetType().GetProperties();

                Dictionary<string, DbType> fieldNames = new Dictionary<string, DbType>();

                foreach (var property in properties)
                {
                    //Todo:osman plan vb
                    var attributes = property.GetCustomAttributes(true);

                    foreach (var attribute in attributes)
                    {
                        if (typeof(KeyAttribute).IsAssignableFrom(attribute.GetType()))
                        {
                            var keyAttribute = attribute as KeyAttribute;
                            fieldNames.Add(keyAttribute.Name, keyAttribute.DbType);
                        }
                    }
                }


                var sqlFieldFormatted = string.Join(" and ", fieldNames.Keys.Select(s => s + " = " + "@" + s));
                sqlBuilder.Append($" where {sqlFieldFormatted}\n");

                var sqlCommand = _sqlConnection.CreateCommand();

                sqlCommand.Transaction = _transAction;
                sqlCommand.CommandText = sqlBuilder.ToString();


                /*
                keyNames.ToList().ForEach(fr => fieldNames.Add(fr.Key, fr.Value));
                foreach (var fieldName in fieldNames)
                {
                    var parameter = sqlCommand.CreateParameter();
                    parameter.ParameterName = fieldName.Key;
                    parameter.Value = entity.GetType().GetProperty(fieldName.Key).GetValue(entity);
                    parameter.DbType = fieldName.Value;
                    sqlCommand.Parameters.Add(parameter);
                }
                sqlCommand.ExecuteNonQuery();
                */





                foreach (var fieldName in fieldNames)
                {
                    var parameter = sqlCommand.CreateParameter();
                    parameter.ParameterName = fieldName.Key;
                    parameter.Value = entity.GetType().GetProperty(fieldName.Key).GetValue(entity);
                    parameter.DbType = fieldName.Value;
                    sqlCommand.Parameters.Add(parameter);
                }
                var effectedRecordCount = sqlCommand.ExecuteNonQuery();

                if (effectedRecordCount == 0)
                    throw new Exception("It effected no rows");
                if (effectedRecordCount > 1)
                    throw new Exception("It effected more then one rows. Effected row count is " + effectedRecordCount);
            }
            catch
            {
                _transAction.Rollback();
                throw;
            }


        }

        /// <summary>
        /// Todo:Ekrem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : BaseEntity, new()
        {
            string commandText = "select ";
            StringBuilder sqlBuilder = new StringBuilder(commandText);


            var properties = typeof(T).GetProperties();

            Dictionary<string, DbType> fieldNames = new Dictionary<string, DbType>();
            Dictionary<string, KeyValuePair<string, Type>> tr = new Dictionary<string, KeyValuePair<string, Type>>();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);

                foreach (var attribute in attributes)
                {
                    if (typeof(FieldAttribute).IsAssignableFrom(attribute.GetType()))
                    {
                        var fieldAttribute = attribute as FieldAttribute;
                        fieldNames.Add(fieldAttribute.Name, fieldAttribute.DbType);
                        tr.Add(property.Name, new KeyValuePair<string, Type>(fieldAttribute.Name, property.PropertyType));
                    }
                }
            }

            var sqlFieldFormatted = string.Join(",", fieldNames.Keys);
            sqlBuilder.Append($"{sqlFieldFormatted}");

            var tableName = typeof(T).Name;
            sqlBuilder.Append($" from {tableName} ");
            var sqlCommand = _sqlConnection.CreateCommand();

            sqlCommand.Transaction = _transAction;
            sqlCommand.CommandText = sqlBuilder.ToString();

            List<T> result = new List<T>();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    var targetType = Activator.CreateInstance<T>();
                    foreach (var key in tr)
                    {
                        targetType.GetType().GetProperty(key.Key).SetMethod.Invoke(targetType, new[] { reader[key.Value.Key] !=DBNull.Value ? reader[key.Value.Key]:key.Value.Value.GetDefaultValue() });
                    }
                    result.Add(targetType);
                }
                reader.Close();
            }

            return result;
        }

        private object GetDefaultValue(Type value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
