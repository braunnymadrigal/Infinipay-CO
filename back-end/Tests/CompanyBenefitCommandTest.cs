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
        CompanyBenefitCommand command;

        [SetUp]
        public void Setup()
        {
            command = new CompanyBenefitCommand(null);
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
            Assert.That(command.IsPercentageValid(dto), Is.True);
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
            Assert.That(command.IsPercentageValid(dto), Is.False);
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
            Assert.That(command.IsPercentageValid(dto), Is.False);
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
            Assert.That(command.IsDeductionTypeValid(dto), Is.True);
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
            Assert.That(command.IsDeductionTypeValid(dto), Is.False);
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
            Assert.That(command.IsElegibleEmployeesValid(dto), Is.True);
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
            Assert.That(command.IsElegibleEmployeesValid(dto), Is.False);
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
            Assert.That(command.IsMinEmployeeTimeValid(dto), Is.True);
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
            Assert.That(command.IsMinEmployeeTimeValid(dto), Is.False);
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
            Assert.That(command.IsDeductionTypeValid(dto), Is.False);
        }

    }
}
