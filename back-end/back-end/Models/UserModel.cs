namespace back_end.Models
{
    public class UserModel
    {
        public UserModel()
        {
            this.Nickname = "";
            this.Password = null;
            this.Role = null;
            this.PersonaId = "";
        }
        public string Nickname { get; set; }
        public byte[]? Password { get; set; }
        public string? Role { get; set; }
        public string PersonaId { get; set; }
    }
}
