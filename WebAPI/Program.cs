using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //autofac kullan�m�
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutofacBusinessModule());
            });



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

            //builder.Services.AddSingleton<IProductService, ProductManager>();
            //Productmanager IProductDal a ba��ml�
            //builder.Services.AddSingleton<IProductDal, EfProductDal>();

            // Add services to the container.
            var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });
            ServiceTool.Create(builder.Services);


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

            app.UseAuthentication(); //middleware => aspnet ya�am d�ng�s�nde hangi yap�lar�n s�ras�yla devreye girece�ini s�yleriz


            app.MapControllers();

            app.Run();
        }
    }
}
