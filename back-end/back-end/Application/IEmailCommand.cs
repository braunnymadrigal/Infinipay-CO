using back_end.Models;
namespace back_end.Application
{
  public interface IEmailCommand
  {
    Task<string> sendEmail(EmailModel email, string? loggedUserId = null);
  }
}
