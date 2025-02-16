using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jelly.Abstractions
{
    public interface IDataSource
    {
        public void Create<T>(T record) where T : class;

        public void Update<T>(T record) where T : class;

        public void Delete<T>(T record) where T : class;

        public List<T> FindSet<T>(List<Expression<Func<T, bool>>> predicate = null) where T : class;

        public T FindFirst<T>(List<Expression<Func<T, bool>>> predicate = null) where T : class;

        public T Get<T>(List<Expression<Func<T, bool>>> predicate) where T : class;

        public int Count<T>(List<Expression<Func<T, bool>>> predicate = null) where T : class;

        public byte[] CalculateField<T>(List<Expression<Func<T, bool>>> predicate, Expression<Func<T, Object>> outExpr) where T : class;

        public decimal Sum<T>(Expression<Func<T, decimal>> field, List<Expression<Func<T, bool>>> predicate = null) where T : class;

        public bool CanHandleEntity<T>();

        public List<T> Next<T>(List<Expression<Func<T, bool>>> predicate = null) where T : class;

        public int Priority { get; }
    }
}
