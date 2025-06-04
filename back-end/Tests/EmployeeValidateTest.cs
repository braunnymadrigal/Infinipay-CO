using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;

namespace Tests
{
    class EmployeeValidateTest
    {
        private EmployeeRepository _repository;

    
        [SetUp]
        public void Setup()
        {
            _repository = new EmployeeRepository();
        }
        [Test]
        public void Get_hasEmployeeBeenPayedAlready_withRandomUserId_ReturnsExpectedResult()
        {
            var userId = Guid.NewGuid(); // Test user id
            var expectedResult = false;
            var transaction = _repository.OpenTransaction();

            var result = _repository.hasEmployeeBeenPayedAlready(userId, transaction);
            transaction.Rollback();
            _repository.CloseConnection(transaction);

            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void Get_hasEmployeeBeenPayedAlready_WithValidUserId_ReturnsExpectedResult()
        {
            var userId = Guid.Parse("F08F5483-9ADD-4758-8CF8-95970998FE8A"); // Test user id
            var expectedResult = false;
            var transaction = _repository.OpenTransaction();

            var result = _repository.hasEmployeeBeenPayedAlready(userId, transaction);
            transaction.Rollback();
            _repository.CloseConnection(transaction);

            Assert.AreEqual(expectedResult, result);
        }
        // [Test]
        // public void Get_hasEmployeeBeenPayedAlready_WithPayedEmployeeUserId_ReturnsExpectedResult()
        // {
        //     var userId = Guid.NewGuid(); // Test user id
        //     var expectedResult = true;
        //     var transaction = _repository.OpenTransaction();

        //     var result = _repository.hasEmployeeBeenPayedAlready(userId, transaction);
        //     transaction.Rollback();
        //     _repository.CloseConnection();

        //     Assert.AreEqual(expectedResult, result);
        // }

    }
}