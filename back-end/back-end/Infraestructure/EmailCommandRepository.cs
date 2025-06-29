using System.Net.Mail;

namespace back_end.Infraestructure
{
  public class EmailCommandRepository : IEmailCommandRepository
  {
    public EmailCommandRepository() { }

    public async Task sendMail(SmtpClient client, MailMessage msg)
    {
      try
      {
        await client.SendMailAsync(msg);
      }
      catch (Exception ex)
      {
        throw new Exception("Error enviando correo.");
      }
    }
  }
}
