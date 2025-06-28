namespace back_end.Infraestructure
{
  public interface IEmailQueryRepository
  {
    public string getEmail(string loggedUserId);
  }
}
