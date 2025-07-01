using back_end.Application;
using back_end.Domain;
using back_end.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests
{
    class CompanyBenefitCommandTest
    {
        private CompanyBenefitCommand command;

        [SetUp]
        public void Setup()
        {
            this.command = new CompanyBenefitCommand(null, null);
        }

        [Test]
        public void Test_IsPercentageValid_ValidRange()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    deductionType = "porcentaje",
                    paramOneAPI = "45"
                }
            };
            Assert.That(this.command.IsPercentageValid(dto), Is.True);
        }

        [Test]
        public void Test_IsPercentageValid_InvalidString()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    deductionType = "porcentaje",
                    paramOneAPI = "abc"
                }
            };
            Assert.That(this.command.IsPercentageValid(dto), Is.False);
        }

        [Test]
        public void Test_IsPercentageValid_OutOfRange()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    deductionType = "porcentaje",
                    paramOneAPI = "150"
                }
            };
            Assert.That(this.command.IsPercentageValid(dto), Is.False);
        }

        [Test]
        public void Test_IsDeductionTypeValid_Valid()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    deductionType = "api"
                }
            };
            Assert.That(this.command.IsDeductionTypeValid(dto), Is.True);
        }

        [Test]
        public void Test_IsDeductionTypeValid_Invalid()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    deductionType = "descuento"
                }
            };
            Assert.That(this.command.IsDeductionTypeValid(dto), Is.False);
        }

        [Test]
        public void Test_IsElegibleEmployeesValid_Valid()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    elegibleEmployees = "tiempoCompleto"
                }
            };
            Assert.That(this.command.IsElegibleEmployeesValid(dto), Is.True);
        }

        [Test]
        public void Test_IsElegibleEmployeesValid_Invalid()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    elegibleEmployees = "interno"
                }
            };
            Assert.That(this.command.IsElegibleEmployeesValid(dto), Is.False);
        }

        [Test]
        public void Test_IsMinEmployeeTimeValid_Valid()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    minEmployeeTime = 2
                }
            };
            Assert.That(this.command.IsMinEmployeeTimeValid(dto), Is.True);
        }

        [Test]
        public void Test_IsMinEmployeeTimeValid_Invalid()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    minEmployeeTime = -1
                }
            };
            Assert.That(this.command.IsMinEmployeeTimeValid(dto), Is.False);
        }

        [Test]
        public void Test_IsDeductionTypeValid_CaseInsensitive()
        {
            var dto = new CompanyBenefitDTO
            {
                benefit = new BenefitDTO
                {
                    deductionType = "API"
                }
            };
            Assert.That(this.command.IsDeductionTypeValid(dto), Is.False);
        }

    }
}
