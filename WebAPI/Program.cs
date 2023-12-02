
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

            // AddSingleton arka planda referans olu�tur. (bizim yerimize newliyor) 
            //e�erki bir ba��ml�l�k g�r�rsen (IProductService) kar��l��� (ProductManager)
            // Singleton 1 tane productManager olu�turuyor 
            // isterse 1 milyon tane client gelsin hepsine ayn� instence veriyor. 1milyon instence �retiminden kurtuluyoruz. hepsi ayn� referans� kullan�yor.
            // ** singleton i�erisinde data yoksa kullan�l�r
            //biri ctorda IProductService isterse ona arka planda new ProductManager ver
            builder.Services.AddSingleton<IProductService, ProductManager>();

            //Productmanager IProductDal a ba��ml�
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