using AlignUp.Domain.Model.User;

namespace AlignUp.BusinessLogic.Interface
{
    public interface IAuth
    {
        UserLoginResponseDTO UserLogin(UserLoginDTO data);
        string UserAuthWithLogic(UserLoginDTO data);
        bool UserRegister(UserRegisterDTO data);
        bool ValidateUserToken(string token);
        string UserAuthLogic(UserLoginDTO loginDataForLogic);
    }
}