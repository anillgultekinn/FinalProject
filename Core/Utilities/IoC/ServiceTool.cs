using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.IoC
{

    //.net in service lerini al ve onları build et.
    //Kısaca bu kod webAPI de ya da autofac de oluşturduğumuz injectionları oluşturabilmemizi sağlıyor.
    //istediğimiz herhangi interface karşılığını (servicedeki karşılığını) bu tool ile alabiliiriz
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}