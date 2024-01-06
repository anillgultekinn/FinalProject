using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching;

public class CacheAspect : MethodInterception
{
    private int _duration;
    private ICacheManager _cacheManager; //burası bir aspect olduğu için injection devre dışı 

    public CacheAspect(int duration = 60) //eğer biz bir süre vermezsek veri 60 dk cache kalacak sonra sistem otomatik olarak cacheden silecek
    {
        _duration = duration;
        _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
    }

    public override void Intercept(IInvocation invocation)
    {
        var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
        var arguments = invocation.Arguments.ToList(); //metodun parametlerini listeye çevir
        var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})"; //eğer paramtetre değeri varsa Get metodunun içine koyar yoksa null
        if (_cacheManager.IsAdd(key)) //bellekte böyle bir cache anahtarı var mı 
        {
            invocation.ReturnValue = _cacheManager.Get(key); //metodun return değeri cachedeki data olsun 
            return;
        }
        invocation.Proceed();//metodu devam ettir
        _cacheManager.Add(key, invocation.ReturnValue, _duration);
    }
}
//ReflectedType => namespace al  => Business.Concrete
//ReflectedType.FullName => namespace + manager (classın ismini verir )   => IProductService
//invocation.ReturnValue => metodu hiç çalıştırmadan manuel olarak return döner

