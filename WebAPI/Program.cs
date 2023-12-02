
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //Autofac,Ninject,CastleWindsor,StructureMap,LightInject,DryInject --> Ioc Container
            //AOP

            // AddSingleton arka planda referans oluþtur. (bizim yerimize newliyor) 
            //eðerki bir baðýmlýlýk görürsen (IProductService) karþýlýðý (ProductManager)
            // Singleton 1 tane productManager oluþturuyor 
            // isterse 1 milyon tane client gelsin hepsine ayný instence veriyor. 1milyon instence üretiminden kurtuluyoruz. hepsi ayný referansý kullanýyor.
            // ** singleton içerisinde data yoksa kullanýlýr
            //biri ctorda IProductService isterse ona arka planda new ProductManager ver
            builder.Services.AddSingleton<IProductService, ProductManager>();

            //Productmanager IProductDal a baðýmlý
            builder.Services.AddSingleton<IProductDal, EfProductDal>();




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}