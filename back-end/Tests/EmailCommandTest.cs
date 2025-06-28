using AutoFixture;
using back_end.Application;
using back_end.Infraestructure;
using Microsoft.Extensions.Configuration;
using Moq;
using back_end.Models;
using System.Net.Mail;
using AutoFixture.AutoMoq;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Tests
{
  class EmailCommandTest
  {

    private Mock<IEmailQueryRepository> emailQueryRepository;
    private Mock<IEmailCommandRepository> emailCommandRepository;
    private Mock<IConfiguration> emailConfiguration;
    private EmailCommand emailCommand;

    private Fixture fixture;

    [SetUp]
    public void Setup()
    {
      emailQueryRepository = new Mock<IEmailQueryRepository>();
      emailCommandRepository = new Mock<IEmailCommandRepository>();
      emailConfiguration = new Mock<IConfiguration>();

      emailConfiguration.Setup(c => c["Email:address"])
        .Returns("noreply@test.com");
      emailConfiguration.Setup(c => c["Email:password"])
        .Returns("secret123");

      emailCommand = new EmailCommand(emailQueryRepository.Object
        , emailCommandRepository.Object, emailConfiguration.Object);

      fixture = new Fixture();
      fixture.Customize(new AutoMoqCustomization());
    }

    [Test]
    public async Task SendEmail_ReturnSuccess_WhenThereIsRecipient()
    {
      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, "Unit‑test subject")
      .With(e => e.message, "Hello from the test!")
      .With(e => e.recipients, new List<string>
      { "user1@example.com"})
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel);

      Assert.That(result, Is.EqualTo("Correo enviado!"));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Once);

      Assert.That(capturedMessage, Is.Not.Null);
      Assert.That(capturedMessage.Subject, Is.EqualTo(emailModel.subject));
      Assert.That(capturedMessage.Body, Is.EqualTo(emailModel.message));
      Assert.That(capturedMessage.Bcc.Select(x => x.Address),
        Is.EquivalentTo(emailModel.recipients));

      Assert.That(capturedClient, Is.Not.Null);
    }

    [Test]
    public async Task SendEmail_ReturnSuccess_WhenThereIsRecipients()
    {
      var expectedErrorMsg = "Correo enviado!";

      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, "Unit‑test subject")
      .With(e => e.message, "Hello from the test!")
      .With(e => e.recipients, new List<string>
      {
        "user1@example.com",
        "user2@example.com",
        "user3@example.com"
      })
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Once);

      Assert.That(capturedMessage, Is.Not.Null);
      Assert.That(capturedMessage.Subject, Is.EqualTo(emailModel.subject));
      Assert.That(capturedMessage.Body, Is.EqualTo(emailModel.message));
      Assert.That(capturedMessage.Bcc.Select(x => x.Address),
        Is.EquivalentTo(emailModel.recipients));

      Assert.That(capturedClient, Is.Not.Null);
    }

    [Test]
    public async Task SendEmail_ReturnErrorMessage_WhenThereIsNoSubject()
    {
      var expectedErrorMsg = "Sujeto del correo es requerido. ";

      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, string.Empty)
      .With(e => e.message, "Hello from the test!")
      .With(e => e.recipients, new List<string>
      {
        "user1@example.com",
      })
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Never);

      Assert.That(capturedMessage, Is.Null);
      Assert.That(capturedClient, Is.Null);
    }

    [Test]
    public async Task SendEmail_ReturnErrorMessage_WhenThereIsNoMessage()
    {
      var expectedErrorMsg = "Mensaje del correo es requerido. ";

      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, "Unit‑test subject")
      .With(e => e.message, string.Empty)
      .With(e => e.recipients, new List<string>
      {
        "user1@example.com",
      })
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Never);

      Assert.That(capturedMessage, Is.Null);
      Assert.That(capturedClient, Is.Null);
    }

    [Test]
    public async Task SendEmail_ReturnErrorMessage_WhenThereIsNoUserEmail()
    {
      var expectedErrorMsg = "No se pudo obtener el correo electrónico del" +
        " usuario. Destinatario(s) del correo es requerido. ";

      var loggedUserId = "user123";

      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, "Unit‑test subject")
      .With(e => e.message, "Hello from the test!")
      .With(e => e.recipients, new List<string>())
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailQueryRepository
        .Setup(r => r.getEmail(loggedUserId))
        .Returns(string.Empty);

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel, loggedUserId);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Never);

      Assert.That(capturedMessage, Is.Null);
      Assert.That(capturedClient, Is.Null);
    }

    [Test]
    public async Task
      SendEmail_ReturnErrorMessage_WhenThereIsNoRecipientsNoUserId()
    {
      var expectedErrorMsg = "Destinatario(s) del correo es requerido. ";

      var loggedUserId = string.Empty;

      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, "Unit‑test subject")
      .With(e => e.message, "Hello from the test!")
      .With(e => e.recipients, new List<string>())
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailQueryRepository
        .Setup(r => r.getEmail(loggedUserId))
        .Returns(string.Empty);

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel, loggedUserId);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Never);

      Assert.That(capturedMessage, Is.Null);
      Assert.That(capturedClient, Is.Null);
    }

    [Test]
    public async Task
      SendEmail_ReturnErrorMessage_WhenThereIsNoValidCredentials()
    {
      var emptyConfig = new Mock<IConfiguration>();
      emptyConfig.Setup(c => c["Email:address"]).Returns(string.Empty);
      emptyConfig.Setup(c => c["Email:password"]).Returns(string.Empty);

      var sut = new EmailCommand(
             emailQueryRepository.Object,
             emailCommandRepository.Object,
             emptyConfig.Object);

      var expectedErrorMsg = "No se puedo validar credenciales de la" +
        " dirección de correo. No se pudo validar credenciales de contraseña" +
        " del correo.";

      var emailModel = fixture.Build<EmailModel>()
      .With(e => e.subject, "Unit‑test subject")
      .With(e => e.message, "Hello from the test!")
      .With(e => e.recipients, new List<string>
      { "user1@example.com"})
      .With(e => e.attachments, new List<IFormFile>())
      .Create();

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await sut.sendEmail(emailModel);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Never);

      Assert.That(capturedMessage, Is.Null);
      Assert.That(capturedClient, Is.Null);
    }

    [Test]
    public async Task SendEmail_ReturnErrorMessage_WhenThereIsNoEmailModel()
    {
      var expectedErrorMsg = "Form invalido. ";

      EmailModel emailModel = null;

      SmtpClient capturedClient = null;
      MailMessage capturedMessage = null;

      emailCommandRepository
          .Setup(r => r.sendMail(It.IsAny<SmtpClient>()
          , It.IsAny<MailMessage>()))
          .Callback<SmtpClient, MailMessage>((smtp, mail) =>
          {
            capturedClient = smtp;
            capturedMessage = mail;
          })
          .Returns(Task.CompletedTask);

      var result = await emailCommand.sendEmail(emailModel);

      Assert.That(result, Is.EqualTo(expectedErrorMsg));

      emailCommandRepository.Verify(
          r => r.sendMail(It.IsAny<SmtpClient>(), It.IsAny<MailMessage>()),
          Times.Never);

      Assert.That(capturedMessage, Is.Null);
      Assert.That(capturedClient, Is.Null);
    }
  }
}
