using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Autofac;
using Data;
using Data.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi;

namespace IntegrationTests
{
    [TestClass]
    public class GetCustomersTests
    {
        private IDatabaseContext _databaseContext;
        private HttpConfiguration _config;

        [TestInitialize]
        public void Initialize()
        {
            var container = Bootstrapper.Initialize();
            _config = new HttpConfiguration();
            WebApiConfig.Register(_config, container);

            _databaseContext = container.Resolve<IDatabaseContext>();
        }

        [TestMethod]
        public void When_there_are_no_customers_exist_it_should_return_empty_collection()
        {
            //Arrange
            var request = CreateRestRequest("api/customer/");

            //Act
            var response = Execute(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync().Result.Should().BeEmpty();

        }

        [TestMethod]
        public void When_there_any_customers_it_should_return_non_empty_collection()
        {
            //Arrange
            _databaseContext.Groups.Insert(new Group
            {
                Id = 1,
                Name = "Test users"
            });
            _databaseContext.People.Insert(new Person
            {
                Id = 1,
                Name = "Test",
                GroupId = 1
            });
            var request = CreateRestRequest("api/customer/");

            //Act
            var response = Execute(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync().Result.Should().NotBeEmpty();
        }

        private static HttpRequestMessage CreateRestRequest(string url)
        {
            var baseUrl = new Uri("http://localhost/");
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(baseUrl, url),
                Method = HttpMethod.Get
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return request;
        }

        private HttpResponseMessage Execute(HttpRequestMessage request)
        {
            using (var server = new HttpServer(_config))
            {
                var client = new HttpClient(server);

                using (var response = client.SendAsync(request).Result)
                {
                     return response;
                }
            }
        }
    }
}
