using back_end.Domain;

namespace back_end.Application
{
    public interface ILogin
    {
        string LogUser(LoginUserModel loginUserModel);
    }
}
