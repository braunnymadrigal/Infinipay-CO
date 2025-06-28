using System.Net.Mail;

namespace back_end.Infraestructure
{
  public interface IEmailCommandRepository
  {
    Task sendMail(SmtpClient client, MailMessage msg);
  }
}
