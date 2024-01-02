using Core.Entities.Concrete;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;//Configuration
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Core.Extensions;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }//API deki appsetting.json dosyasını okur
        private readonly TokenOptions _tokenOptions;//dosyadan okuduklarını nesne olarak tutar.
        private DateTime _accessTokenExpiration;//accessToken ne zaman geçersizleşecek 
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
             _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();


            //section->.json dosyasında yazdığımız iki süslü parantez arasındaki section oluyor.
            //burada diyoruz ki appsetting.json un içindeki "TokenOptions" bölümünü al, ve TokenOptions sınıfındaki değerleri kullanarak map le.
            //kısacası appsetting.json daki değerleri TokenOptions class ındaki değerler ile eşle.
            //configuration görünce appsetting
        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            //kullanıcı için bir tane token üretiyoruz. (girilen user ve claim bilgisine göre token oluşturuyoruz)

            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration); //tokenin biteceği zaman ( şu andan itibaren _tokenOptions.AccessTokenExpiration verilen süre kadar )
            //appsettings.json da 10 verdik 
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);//token ı oluşturmak için bize bir key gerek. Bu keyi de yazdığımız SecurityKeyHelper sayesinde oluşturduk
            var signingCredentials = SigninCredentialsHelper.CreateSigningCredentials(securityKey);//hangi algoritmayı kullanacağımızı ve anahtarımızın ne olduğunu belirttik. 
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
             var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims) 
        {
            var jwt = new JwtSecurityToken(// token oluşturmaya yarıyor
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now, //eğer tokenin expiration bilgisi şu andan önce ise geçerli değil
                claims: SetClaims(user, operationClaims),  
                signingCredentials: signingCredentials 
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            //claimler için bir method yaptık, burası önemli
            //claim yetki oluyor. Sadece yetki değil kullanıcıya karşılık gelen bir çok bilgi vardır
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");//başına $ eklersek tırnak içerisine kod yazabiliyoruz. iki stringi yan yana getirmek için
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}