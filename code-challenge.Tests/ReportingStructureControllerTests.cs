﻿using challenge.Controllers;
using challenge.Data;
using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using code_challenge.Tests.Integration.Extensions;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using code_challenge.Tests.Integration.Helpers;
using System.Text;


namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
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
        public void GetReportingStructure_Ok()
        {
            // Arrange
            var employeeIdJohn = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            int expectedDirectReportCount = 4;
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/Reports/{employeeIdJohn}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var newReportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.IsNotNull(newReportingStructure.numberOfReports);
            Assert.AreEqual(expectedDirectReportCount, newReportingStructure.numberOfReports);

        }

        [TestMethod]
        public void GetReportingStructure_NotFound()
        {
            // Arrange
            var employeeIdMissing = "";
            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/Reports/{employeeIdMissing}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            // Arrange
            var employeeIdBad = "notValidID";
            // Execute
            var getRequestTaskNotValid = _httpClient.GetAsync($"api/Reports/{employeeIdBad}");
            var responseNotValid = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, responseNotValid.StatusCode);
        }

    }
}
