using System;
using System.Collections.Generic;
using System.Text;

using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTest
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));
            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created_and_Get() 
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedSalary = 101010.1;
            var expectedStartDate = new DateTime(2021, 3, 10);

            var getEmployeeRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getEmployeeRequestTask.Result;
            var employeeFromCall = response.DeserializeContent<Employee>();

            var compensation = new Compensation()
            {
                employeeID = employeeId,
                employee = employeeFromCall,
                effectiveDate = expectedStartDate,
                salary = expectedSalary
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var compensationResponse = postRequestTask.Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, compensationResponse.StatusCode);
            var compensationResponseStructure = compensationResponse.DeserializeContent<Compensation>();

            Assert.AreEqual( employeeId, compensationResponseStructure.employeeID);
            Assert.AreEqual( expectedStartDate, compensationResponseStructure.effectiveDate);
            Assert.AreEqual( expectedSalary, compensationResponseStructure.salary );
            Assert.AreEqual( expectedFirstName, compensationResponseStructure.employee.FirstName);
            Assert.AreEqual( expectedLastName, compensationResponseStructure.employee.LastName);

           // Testing the get
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var getResults = getRequestTask.Result;
            var getCompensationResponseStructure = getResults.DeserializeContent<Compensation>();

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, getResults.StatusCode);
            Assert.AreEqual(employeeId, getCompensationResponseStructure.employeeID);
            Assert.AreEqual(expectedStartDate, getCompensationResponseStructure.effectiveDate);
            Assert.AreEqual(expectedSalary, getCompensationResponseStructure.salary);
            Assert.AreEqual(expectedFirstName, getCompensationResponseStructure.employee.FirstName);
            Assert.AreEqual(expectedLastName, getCompensationResponseStructure.employee.LastName);
        }
    }
}
