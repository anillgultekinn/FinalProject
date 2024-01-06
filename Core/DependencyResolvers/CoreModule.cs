using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Core.DependencyResolvers;

public class CoreModule : ICoreModule
{
    public void Load(IServiceCollection serviceCollection)
    {
        serviceCollection.AddMemoryCache();  // arkaplanda hazır bir ICacheManager instence oluşturuyor
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
        //serviceCollection.AddSingleton<ICacheManager, RediCach eManager>(); eğer  redis kullanmak istersek buraya redis yazmamız yeterli injection yapıyor
        serviceCollection.AddSingleton<Stopwatch>();

    }
}

//, bu kod, ASP.NET Core uygulamasında HttpContext gibi servislere erişim sağlamak için gerekli olan bir bağımlılığı kaydetmeye yönelik bir bağımlılık enjeksiyonu modülünü temsil eder.