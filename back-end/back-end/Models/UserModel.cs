namespace back_end.Models
{
    public class UserModel
    {
        public UserModel()
        {
            this.NicknameOrEmail = "";
            this.Password = null;
            this.Role = "";
            this.PersonaId = "";
        }
        public string NicknameOrEmail { get; set; }
        public byte[]? Password { get; set; }
        public string Role { get; set; }
        public string PersonaId { get; set; }
    }
}
