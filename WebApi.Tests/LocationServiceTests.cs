using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi;
using Xunit;

namespace WebApi.Tests
{
    public class LocationServiceTests
    {
        [Fact]
        public async Task GetLocationAsync_Returns_Fake_Result()
        {
            var testJson = @"{
          ""city"": ""CidadeTeste"",
          ""country"": ""PaisTeste"",
          ""timezone"": ""fusoTeste"",
          ""status"": ""statusTeste""
        }";

            var handler = new FakeHttpMessageHandler(testJson, HttpStatusCode.OK);
            var service = new LocationService(handler);

            var result = await service.GetLocationAsync();

            Assert.NotNull(result);
            Assert.Equal("CidadeTeste", result.Cidade);
            Assert.Equal("PaisTeste", result.Pais);
            Assert.Equal("fusoTeste", result.FusoHorario);
            Assert.Equal("statusTeste", result.Status);
            Assert.Equal("Hora local não disponível", result.HoraLocal);
        }

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly string _json;
            private readonly HttpStatusCode _statusCode;

            public FakeHttpMessageHandler(string json, HttpStatusCode statusCode)
            {
                _json = json;
                _statusCode = statusCode;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(_statusCode)
                {
                    Content = new StringContent(_json, Encoding.UTF8, "application/json")
                };
                return Task.FromResult(response);
            }
        }
    }
}