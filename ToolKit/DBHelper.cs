using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Linq.Expressions;
using EntityFramework.Extensions;

namespace ToolKit
{
    public class DBHelper<Context, TModel>
        where Context : DbContext, new()
        where TModel : class,new()
    {
        public virtual int Insert(TModel model)
        {
            using (Context db = new Context())
            {
                db.Set<TModel>().Add(model);
                return db.SaveChanges();
            }
        }

        public virtual int Update(TModel model)
        {
            using (Context db = new Context())
            {
                if (db.Entry<TModel>(model).State == EntityState.Detached)
                {
                    db.Set<TModel>().Attach(model);
                    db.Entry<TModel>(model).State = EntityState.Modified;
                }
                return db.SaveChanges();
            }
        }

        public virtual int Update(Expression<Func<TModel, bool>> filterExpression, Expression<Func<TModel, TModel>> updateExpression)
        {
            using (Context db = new Context())
            {
                return db.Set<TModel>().Update(filterExpression, updateExpression);
            }
        }

        public virtual int Delete(TModel model)
        {
            using (Context db = new Context())
            {
                db.Set<TModel>().Remove(model);
                return db.SaveChanges();
            }
        }

        public int Delete(Expression<Func<TModel, bool>> filterExpression)
        {
            using (Context db = new Context())
            {
                return db.Set<TModel>().Delete(filterExpression);
            }
        }

        public virtual List<TModel> GetList(Expression<Func<TModel, bool>> filterExpression)
        {
            using (Context db = new Context())
            {
                return db.Set<TModel>().Where(filterExpression).ToList();
            }
        }

        public virtual TModel GetData(Expression<Func<TModel, bool>> filterExpression)
        {
            using (Context db = new Context())
            {
                return db.Set<TModel>().Where(filterExpression).FirstOrDefault();
            }
        }
    }
}
