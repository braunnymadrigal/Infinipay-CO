namespace back_end.Domain
{
    public class UserModel
    {
        public UserModel()
        {
            this.Nickname = "";
            this.Role = "";
            this.PersonId = "";
            this.Password = null;
            this.NumAttempts = 0;
            this.LastBlock = DateTime.MinValue;
        }

        public string Nickname { get; set; }
        public string Role { get; set; }
        public string PersonId { get; set; }
        public byte[]? Password { get; set; }
        public int NumAttempts { get; set; }
        public DateTime LastBlock { get; set; }
    }
}
