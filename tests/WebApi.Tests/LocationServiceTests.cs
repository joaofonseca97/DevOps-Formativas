public class LocationServiceTests
{
    [Fact]
    public async Task GetLocationAsync_Returns_Fake_Result()
    {
        // Arrange
        var testJson = @"{
            ""city"": ""CidadeTeste"",
            ""country"": ""PaisTeste"",
            ""timezone"": ""fusoTeste"",
            ""status"": ""statusTeste""
        }";

        var handler = new FakeHttpMessageHandler(testJson, HttpStatusCode.OK);
        var httpClient = new HttpClient(handler)
        {
            // BaseAddress is optional here; the service calls an absolute URL.
            Timeout = TimeSpan.FromSeconds(5)
        };

        var service = new LocationService(httpClient);

        // Act
        var result = await service.GetLocationAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CidadeTeste", result.Cidade);
        Assert.Equal("PaisTeste", result.Pais);
        Assert.Equal("fusoTeste", result.FusoHorario);
        Assert.Equal("statusTeste", result.Status);

        // Since "fusoTeste" is not a valid TZ, HoraLocal should remain the fallback string.
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
            // Optional: validate the requested URL is the expected endpoint.
            // Assert.Equal("http://ip-api.com/json", request.RequestUri!.ToString());

            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_json, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}