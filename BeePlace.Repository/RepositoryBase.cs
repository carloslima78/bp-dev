using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data;
using Dapper;
using System.Linq;
using Dommel;

namespace BeePlace.Repository
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        // Dapper com fluentMap
        // https://medium.com/filipececcon/c-usando-dapper-com-fluentmap-linq-e-lambda-para-consultas-6f3131bff244

        protected string connectionString;

        protected RepositoryBase()
        {
            connectionString = ConfigurationManager.ConnectionStrings["BeePlace"].ConnectionString;
        }

        public virtual IEnumerable<TEntity> GetAll() 
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    return connection.GetAll<TEntity>();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public virtual TEntity GetById(int id) 
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var a = connection.Get<TEntity>(id);
                    return a;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public virtual void Insert(ref TEntity entity) 
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var id = connection.Insert(entity);
                    entity = GetById((int)id);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public virtual bool Update(TEntity entity) 
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    return connection.Update(entity);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public virtual bool Delete(TEntity entity) 
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    return connection.Delete(entity);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    return connection.Select(predicate);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<TEntity> GetList(CommandType commandType, string sql, object parameters = null) 
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    return connection.Query<TEntity>(sql, parameters, commandType: commandType);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
