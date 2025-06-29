using System.Collections;
using System.Net;
using System.Net.Mail;
using back_end.Infraestructure;
using back_end.Models;
using Microsoft.IdentityModel.Tokens;


namespace back_end.Application
{
  public class EmailCommand : IEmailCommand
  {
    private readonly IEmailQueryRepository emailQueryRepository;
    private readonly IEmailCommandRepository emailCommandRepository;

    private readonly string appEmail;
    private readonly string appPassword;
    private readonly string MEDIA_TYPE = "application/pdf";
  
    public EmailCommand(IEmailQueryRepository emailQueryRepository
      , IEmailCommandRepository emailCommandRepository, IConfiguration config) 
    {
      this.emailQueryRepository = emailQueryRepository;
      this.emailCommandRepository = emailCommandRepository;

      this.appEmail = config["Email:address"] ?? string.Empty;
      this.appPassword = config["Email:password"] ?? string.Empty;
    }

    private string setupRecipientEmail(EmailModel email
      , string loggedUserId)
    {
      var errorMsg = string.Empty;

      try
      {
        var userEmail = emailQueryRepository.getEmail(loggedUserId);

        if (userEmail.IsNullOrEmpty())
        {
          errorMsg += "No se pudo obtener el correo electrónico del usuario. ";
        } else
        {
          email.recipients.Add(userEmail);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error obteniendo correo electrónico");
      }

      return errorMsg;
    }

    private string validateCredentials()
    {
      var errorMsg = string.Empty;

      if (this.appEmail.IsNullOrEmpty())
      {
        errorMsg += "No se puedo validar credenciales de la dirección de" +
          " correo. ";
      }

      if (this.appPassword.IsNullOrEmpty())
      {
        errorMsg += "No se pudo validar credenciales de contraseña del correo.";
      }

      return errorMsg;
    }

    private string validateEmail(EmailModel emailModel
      , string? loggedUserId = null)
    {
      var errorMsg = string.Empty;

      try
      {
        if (emailModel == null)
        {
          errorMsg += "Form invalido. ";
        } 
        else
        {
          if (emailModel.subject.IsNullOrEmpty())
          {
            errorMsg += "Sujeto del correo es requerido. ";
          }

          if (emailModel.message.IsNullOrEmpty())
          {
            errorMsg += "Mensaje del correo es requerido. ";
          }

          if (emailModel.recipients.IsNullOrEmpty()
            && loggedUserId.IsNullOrEmpty())
          {
            errorMsg += "Destinatario(s) del correo es requerido. ";
          } 
          else if (emailModel.recipients.IsNullOrEmpty()
            && !loggedUserId.IsNullOrEmpty())
          {
            errorMsg += setupRecipientEmail(emailModel, loggedUserId);
            if (emailModel.recipients.IsNullOrEmpty())
            {
              errorMsg += "Destinatario(s) del correo es requerido. ";
            }
          }
        }
      }
      catch (Exception ex)
      {
        errorMsg += "Error validando correo. ";
        throw new Exception(errorMsg);
      }

      return errorMsg;
    }

    private SmtpClient setupClient()
    {
      return new SmtpClient
      {
        Host = "smtp.gmail.com",
        Port = 587,
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(
          appEmail
        , appPassword)
      };
    }

    private MailMessage setupEmailMessage(SmtpClient client, EmailModel email)
    {
      try
      {
        var msg = new MailMessage
        {
          From = new MailAddress(appEmail),
          Subject = email.subject,
          Body = email.message
        };

        if (!email.attachments.IsNullOrEmpty())
        {
          foreach (var attachment in email.attachments)
          {
            msg.Attachments.Add(new Attachment(attachment.OpenReadStream()
              , attachment.FileName, MEDIA_TYPE));
          }
        }

        foreach (var recipient in email.recipients)
        {
          msg.Bcc.Add(recipient);
        }

        return msg;
      }
      catch (Exception ex)
      {
        throw new Exception("Error creando correo");
      }

    }

    public async Task<string> sendEmail(EmailModel email
      , string? loggedUserId = null)
    {
      var errorMsg = string.Empty;
      try
      {
        errorMsg += validateCredentials();
        errorMsg += validateEmail(email, loggedUserId);

        if (errorMsg.IsNullOrEmpty())
        {
          var client = setupClient();
          var msg = setupEmailMessage(client, email);
          await emailCommandRepository.sendMail(client, msg);
        }
      }
      catch (Exception ex)
      {
        errorMsg += ex.ToString();
        throw new Exception(errorMsg);
      }

      if (errorMsg.IsNullOrEmpty())
      {
        errorMsg = "Correo enviado!";
      }

      return errorMsg;
    }
  }
}
