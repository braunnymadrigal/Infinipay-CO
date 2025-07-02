using back_end.Application;
using back_end.Models;
using back_end.Repositories;
using back_end.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using AutoFixture;
using System.Threading.Tasks;

namespace Tests
{
    public class CompanyBenefitCommandDeleteTest
    {
        private CompanyBenefitCommand _command;
        private Mock<ICompanyBenefitRepository> _benefitRepository;
        private Mock<IEmailCommand> _emailCommand;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _benefitRepository = new Mock<ICompanyBenefitRepository>();
            _emailCommand = new Mock<IEmailCommand>();
            _fixture = new Fixture();
            _command = new CompanyBenefitCommand(_benefitRepository.Object, _emailCommand.Object);
        }

        [Test]
        public async Task DeleteBenefit_Throws_WhenNicknameIsNullOrEmpty()
        {
            var id = Guid.NewGuid();

            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                _command.DeleteBenefit(id, ""));

            Assert.That(ex.Message, Is.EqualTo("Nickname del usuario que borra el beneficio es requerido"));
        }

        [Test]
        public async Task DeleteBenefit_Throws_WhenIdIsEmpty()
        {
            var nickname = "tester";

            var ex = Assert.ThrowsAsync<ArgumentException>(() =>
                _command.DeleteBenefit(Guid.Empty, nickname));

            Assert.That(ex.Message, Is.EqualTo("Id del beneficio es requerido"));
        }

        [Test]
        public async Task DeleteBenefit_SendsEmailAndDeletes_WhenEmployeesWithBenefitExist()
        {
            var id = Guid.NewGuid();
            var nickname = "tester";
            var benefitName = "Asociacion Solidarista";
            var employees = _fixture.Create<List<string>>();

            var benefit = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO { name = benefitName }
            };

            _benefitRepository.Setup(r => r.getBenefitById(id)).Returns(benefit);
            _benefitRepository.Setup(r => r.getEmployeesWithBenefit(id)).Returns(employees);
            _emailCommand.Setup(e => e.sendEmail(It.IsAny<EmailModel>(), It.IsAny<string?>())).ReturnsAsync("ok");
            _benefitRepository.Setup(r => r.DeleteBenefit(id, nickname)).Verifiable();

            await _command.DeleteBenefit(id, nickname);

            _emailCommand.Verify(e => e.sendEmail(It.Is<EmailModel>(m =>
                m.recipients.SequenceEqual(employees) &&
                m.subject.Contains("Beneficio eliminado:") &&
                m.message.Contains(benefitName)
            ), It.IsAny<string?>()), Times.Once);

            _benefitRepository.Verify(r => r.DeleteBenefit(id, nickname), Times.Once);
        }

        [Test]
        public async Task DeleteBenefit_DeletesWithoutEmail_WhenNoEmployeesWithBenefit()
        {
            var id = Guid.NewGuid();
            var nickname = "tester";
            var benefit = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO { name = "Beneficio sin empleados asignados" }
            };

            _benefitRepository.Setup(r => r.getBenefitById(id)).Returns(benefit);
            _benefitRepository.Setup(r => r.getEmployeesWithBenefit(id)).Returns(new List<string>());
            _benefitRepository.Setup(r => r.DeleteBenefit(id, nickname)).Verifiable();

            await _command.DeleteBenefit(id, nickname);

            _emailCommand.Verify(e => e.sendEmail(It.IsAny<EmailModel>(), It.IsAny<string?>()), Times.Never);
            _benefitRepository.Verify(r => r.DeleteBenefit(id, nickname), Times.Once);
        }
    }
}
