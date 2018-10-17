using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WS.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        //IEnumerable<T> Filter(Expression<Func<T, bool>> predicate);
        //T Find(Expression<Func<T, bool>> predicate);
        void Create(T item);
        T Get(int? id);
        void Update(T item);
        void Delete(int? id);
    }
}
