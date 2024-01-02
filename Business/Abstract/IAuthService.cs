using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);//sign in de olabilmeli, (kayıt)
        IDataResult<User> Login(UserForLoginDto userForLoginDto);//login de olabilmeli
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}