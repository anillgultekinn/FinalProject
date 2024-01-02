using Business.Constants;
using Castle.DynamicProxy;
using Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Core.Utilities.Interceptors;
using Microsoft.AspNetCore.Http;
using Core.Utilities.IoC;

namespace Business.BusinessAspects
{
    //JWT için
    public class SecuredOperation : MethodInterception
    {

        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;  //her istek için bir httpcontext oluşur 

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');//attribute da rolleri virgül ile ayırarak yazıyor. Bizde Split methodunu kullanarak virgül ile ayırıyoruz.
              //aspect olduğu için aspect enjekte edemiyoruz bizde diyoruzki .net autofac ile oluşturduğumuz service mimarimize ulaş (ServiceTool) GetService
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            //_productDal = ServiceTool.ServiceProvider.GetService<IProductService>();autofacte bizim yaptığımız injection değerlerini alacak


            //serviceTool, bizim injection altyapımızı aynen okuyabilmemizi sağlayan bir araçtır.
            //.net in service lerini al ve onları build et.
            //Kısaca bu kod webAPI de ya da autofac de oluşturduğumuz injectionları oluşturabilmemizi sağlıyor.
            //controller business çağırıyor   business dal çağırıyor o zincirin içerisinde bu yok o yüzden injection yapamayız o yüzden servicetool kullanırız
          
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles(); //kullanıcının claimroles bul
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role)) //eğer claim içerisinde ilgili rol varsa
                {
                    return; //metodu çalıştırmaya devam et (onbefore bitir hata verme)
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}