namespace back_end.Models
{
    public class UserModel
    {
        public UserModel()
        {
            this.Nickname = "";
            this.Role = "";
            this.PersonaId = "";
            this.Password = null;
        }
        public string Nickname { get; set; }
        public string Role { get; set; }
        public string PersonaId { get; set; }
        public byte[]? Password { get; set; }
    }
}
