using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Interfaces
{
    public interface I2Repository <T> where T : class
    {
        IEnumerable<T> GetAll();
        //IEnumerable<T> Filter(Expression<Func<T, bool>> predicate);
        //T Find(Expression<Func<T, bool>> predicate);
        void Create(T item);
        T Get(int? id1, int? id2);
        void Update(T item);
        void Delete(int? id1, int? id2);
    }
}
