using back_end.Domain;

namespace back_end.Infraestructure
{
    public interface ILoginRepository
    {
        UserModel GetUserByNickname(string nickname);
        UserModel GetUserByEmail(string email);
        void UpdateExactBlockDateInUser(string personId);
        void UpdateNumAttemptsInUser(string personId, int numAttempts);
    }
}
