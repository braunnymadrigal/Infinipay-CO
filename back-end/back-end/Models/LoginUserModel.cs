namespace back_end.Models
{
    public class LoginUserModel
    {
        public LoginUserModel()
        {
            this.NicknameOrEmail = "";
            this.Password = "";
        }
        public string NicknameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
