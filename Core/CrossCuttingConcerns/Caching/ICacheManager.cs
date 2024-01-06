using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key); //ben sana bir key değeri vereyim sen bellekten o keye karşılık olan datayı bana ver
        object Get(string key);
        void Add(string key, object value, int duration);
        bool IsAdd(string key); //cache de var mı eğer varsa verileri cacheden getir
        void Remove(string key);
        void RemoveByPattern(string pattern);  //Ör: içinde get olanları sil  Ör: başı sonu önemli değil içinde category olan

    }
}
