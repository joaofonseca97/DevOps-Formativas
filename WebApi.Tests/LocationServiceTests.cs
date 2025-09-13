using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using WebApi;

namespace WebApi.Tests
{
    public class LocationServiceTests
    {


        [Fact]
        public async Task GetLocationAsync_Returns_Fake_Result()
        {
            // Arrange: Configura o ambiente de teste
            var testJson = @"{ ""city"":""CidadeTeste"", ""country"":""PaisTeste"", ""timezone"":""fusoTeste"", ""status"":""statusTeste"" }";
            var handler = new FakeHttpMessageHandler(testJson, HttpStatusCode.OK);
            var httpClient = new HttpClient(handler); // Não é mais necessário o disposeHandler: false

            // Instancia o serviço com o HttpClient falso
            var service = new LocationService(httpClient); // O construtor de 1 argumento será criado no próximo passo

            // Act: Executa o método a ser testado
            var result = await service.GetLocationAsync();

            // Assert: Verifica o resultado
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
            public FakeHttpMessageHandler(string json, HttpStatusCode statusCode) { _json = json; _statusCode = statusCode; }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
                => Task.FromResult(new HttpResponseMessage(_statusCode)
                { Content = new StringContent(_json, Encoding.UTF8, "application/json") });
        }
    }
}