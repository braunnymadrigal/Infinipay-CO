using Moq;
using back_end.Application;
using back_end.Infraestructure;
using AutoFixture;

namespace Tests
{
    public class CompanyDeletionTest
    {
        private Mock<ICompanyDeletionRepository> _companyDeletionRepository;
        private CompanyDeletion _companyDeletion;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _companyDeletionRepository = new Mock<ICompanyDeletionRepository>();
            _companyDeletion = new CompanyDeletion(_companyDeletionRepository.Object);
        }

        [Test]
        public void DeleteCompany_ReturnsCombinedEmails_WhenBothEmployerAndEmployeesExist()
        {
            var numberOfEmployees = 7;
            var employerEmail = "employer@gmail.com";
            var employeesEmails = _fixture.Build<string>()
                .CreateMany(numberOfEmployees)
                .ToList();

            _companyDeletionRepository.Setup(r => 
                r.getEmployerEmail(It.IsAny<string>()))
                .Returns(employerEmail);
            _companyDeletionRepository.Setup(r => 
                r.getEmployeesEmail(It.IsAny<string>()))
                .Returns(employeesEmails);
            _companyDeletionRepository.Setup(r => 
                r.deleteCompany(It.IsAny<string>()))
                .Verifiable();

            var result = _companyDeletion.deleteCompany(string.Empty);

            Assert.That(result.Count, Is.EqualTo(numberOfEmployees + 1));
            Assert.That(result[numberOfEmployees], Is.EqualTo(employerEmail));

            _companyDeletionRepository.VerifyAll();
        }

        [Test]
        public void DeleteCompany_ReturnsOnlyEmployerEmail_WhenNoEmployeesExist()
        {
            _companyDeletionRepository.Setup(r =>
                r.getEmployerEmail(It.IsAny<string>()))
                .Returns(string.Empty);
            _companyDeletionRepository.Setup(r =>
                r.getEmployeesEmail(It.IsAny<string>()))
                .Returns(new List<string>());
            _companyDeletionRepository.Setup(r =>
                r.deleteCompany(It.IsAny<string>()))
                .Verifiable();

            var result = _companyDeletion.deleteCompany(string.Empty);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(string.Empty));

            _companyDeletionRepository.VerifyAll();
        }
    }
}