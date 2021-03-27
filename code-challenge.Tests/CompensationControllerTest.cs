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

        //Helper functions
        private bool SameContents(Compensation first, Compensation second, bool checkCompensationID = true)
        {
            bool result = false;
            if (checkCompensationID)
            {
                result = (first == second);
            }
            else 
            {
                result = (first.employeeID == second.employeeID);
                result &= (first.effectiveDate == second.effectiveDate);
                result &= (first.salary == second.salary);
            }
            return result;
        }
        private HttpResponseMessage GetCompensationResult(string id)
        {
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{id}");
            return getRequestTask.Result;
        }
        private HttpResponseMessage CreatCompensationResult(Compensation objectToCreate) 
        {
            var requestContent = new JsonSerialization().ToJson(objectToCreate);
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            return postRequestTask.Result;
        }

        //Tests
        [TestMethod]
        public void CreateCompensation_Returns_Created_and_Get() 
        {
            // Arrange
            var compensationOriginalNoCompensationID = new Compensation()
            {
                employeeID = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                effectiveDate = new DateTime(2021, 3, 10),
                salary = 101010.1
            };

            // Execute
            var compensationResponse = CreatCompensationResult(compensationOriginalNoCompensationID);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, compensationResponse.StatusCode);
            var compensationResponseStructure = compensationResponse.DeserializeContent<Compensation>();

            Assert.AreNotEqual(null, compensationResponseStructure.compensationID);
            Assert.IsTrue(SameContents(compensationOriginalNoCompensationID, compensationResponseStructure, false));

            // Testing the get
            var getResults = GetCompensationResult(compensationResponseStructure.compensationID);
            var getCompensationResponseStructure = getResults.DeserializeContent<Compensation>();

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, getResults.StatusCode);
            Assert.IsTrue(SameContents(compensationResponseStructure, getCompensationResponseStructure));
        }
        [TestMethod]
        public void CreateCompensation_Bad_Employee_ID() 
        {
            // Arrange
            var compensationOriginalNoCompensationIDNoEmployeeID = new Compensation()
            {
                employeeID = null,
                effectiveDate = new DateTime(2021, 3, 10),
                salary = 101010.1
            };

            // Execute
            var compensationResponse = CreatCompensationResult(compensationOriginalNoCompensationIDNoEmployeeID);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, compensationResponse.StatusCode);
        }
        [TestMethod]
        public void GetCompensation_Bad_Compenstion_ID() 
        {
            String badCompensationID = "BadID";

            // Testing the get
            var getResults = GetCompensationResult(badCompensationID);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, getResults.StatusCode);
        }

    }
}
