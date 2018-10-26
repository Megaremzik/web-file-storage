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
        IEnumerable<T> GetAll(string s=null);
        //IEnumerable<T> Filter(Expression<Func<T, bool>> predicate);
        //T Find(Expression<Func<T, bool>> predicate);
        void Create(T item);
        T Get(int? id);
        T Get(string id1, int? id2);
        void Update(T item);
        void Delete(int? id);
        void Delete(string id1, int? id2);

    }
}
