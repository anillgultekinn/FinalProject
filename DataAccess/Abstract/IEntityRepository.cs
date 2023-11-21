using Entities.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    //generic constraint 
    //where T:class =>  referans tip olabilir <> içerisine int vs gelemez
    //where T: class,IEntity  =>   IEntity ya IEntity olabilir ya da IEntity den implemente edilen bir nesne olabilir (Entities içerisinkide classlar)
    //new () : new'lenebilir olmalı. IEntity interface olduğu için kullanamayız
    public interface IEntityRepository<T> where T : class, IEntity, new()

    {
        //expression linq ile filtre yaparak datanın belirli bir kısmını getirmek için kullanırız
        //filter = null => tüm datayı istiyor.
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
