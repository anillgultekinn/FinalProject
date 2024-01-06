using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Microsoft;

public class MemoryCacheManager : ICacheManager
{

    //sadece microsoftun memorycache eklemek değil eğer onu eklersek yarın öbürgün başka bir cache yönetimnde patalarız hardcode 
    //.netcore dan gelen kodu kendimize uyarlıyoruz 
    //
    //Adapter Pattern
    IMemoryCache _memoryCache;

    public MemoryCacheManager()
    {
        _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
    }


    //public MemoryCacheManager(IMemoryCache memoryCache)
    //{

    //    _memoryCache = memoryCache;

    //çalışmaz. çünkü zincir  webapi business  dataaccess şeklinde çalışıyor aspect bambaşka bir şeyin içinde  bağımlılık zincirinin içinde değil
    //bunun için servicetool yazdık

    //}
    //serviceCollection.AddMemoryCache();  CoreModule de bunu yazarak artık _memoryCache karşılığı olmuş oluyor yani injection



    public void Add(string key, object value, int duration)
    {
        _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        //ne kadar süre verirsek o kadar cache kalacak kodu yazdık
    }

    public T Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public object Get(string key)
    {
        return _memoryCache.Get(key);
    }

    public bool IsAdd(string key)
    {
        return _memoryCache.TryGetValue(key, out _);
        //key, out_  bellekte böyle bir anahtar var mı yok mu onu kontrol etmek istiyoruz datayı istemiyoruz  o yüzden out_ verdik 
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
    
    public void RemoveByPattern(string pattern)
    {
        //git belleğe bak bellekte MemoryCache türünde olan EntriesCollectionı bul 
        var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //definitionu  _memoryCache olanları bul
        var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
        List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

        foreach (var cacheItem in cacheEntriesCollection)
        {
            ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
            cacheCollectionValues.Add(cacheItemValue);
        }

        var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

        foreach (var key in keysToRemove)
        {
            _memoryCache.Remove(key);
        }
    }
}

//RemoveByPattern çalışma anında bellekten silmeye yarıyor.  bellekte elimizde bir sınıfın instence var çalışma anında müdahale etmek istiyoruz
//Reflection ile çalışma anında elimizde bulunan nesnelere müdahale etme gibi şeyleri reflection ile yapıyoruz