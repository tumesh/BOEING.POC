using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Collections.Generic;
namespace BOEING.POC.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private HttpResponseMessage _response;

        private const string ServiceBaseURL = "http://localhost:8081/";
        private HttpClient _client;
        [TestInitialize]
        public void ReInitializeTest()
        {
            _client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };

        }
        [TestCleanup]
        public void DisposeTest()
        {
            if (_response != null)
            {
                _response.Dispose();
            }

            if (_client != null)
                _client.Dispose();
        }

        [TestMethod]
        public void TestHelloWorld()
        {

            _response = _client.GetAsync("api/v1/BOEING/Services/HelloWorld").Result;
            if (_response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //actual
                var result = _response.Content.ReadAsStringAsync().Result;

                //Asset
                Assert.AreEqual("HelloWorld", result.Replace("\"", ""));

            }
        }
        [TestMethod]
        public void TestHelloUser()
        {
            _client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };
            string strMethodPath = "api/v1/BOEING/Services/HelloWorld/";
            string strParameterValue = "Tharanath";

            _response = _client.GetAsync(strMethodPath + strParameterValue).Result;
            if (_response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //actual
                var result = _response.Content.ReadAsStringAsync().Result;

                //Asset
                Assert.AreEqual(string.Format("Hello {0}!", strParameterValue), result.Replace("\"", ""));
            }
        }
    }
}
