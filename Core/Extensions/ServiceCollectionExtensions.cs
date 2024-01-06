using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers
            (this IServiceCollection serviceCollection, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollection);
                module.Load(serviceCollection);
                //her bir modul için modulu yükle
                //yani birden fazla modulu ekleyebiliriz
            }
            return ServiceTool.Create(serviceCollection);
        }
    }
}
//bu kod bizim core katmanıda dahil olmak üzere ekleyeceğimiz bütün injectionları bir arada toplayabileceğmiz bir yapıya dönüştü

//IServiceCollection => bizim aspnet uygulamamızın yani kısacası apimizin service bağımlılıklarını eklediğimiz ya da araya girmesini istediğimiz servisleri eklediğimiz koleksiyonudur