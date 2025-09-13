using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TimeZoneConverter;

namespace WebApi
{
    public class LocationService
    {
        private readonly HttpClient _client;

        // ESTE É O CONSTRUTOR QUE ESTAVA FALTANDO. ADICIONE-O.
        public LocationService(HttpClient client)
        {
            _client = client;
        }

        public async Task<LocationResult> GetLocationAsync()
        {
            string url = "http://ip-api.com/json";
            // MODIFIQUE A LINHA ABAIXO PARA USAR O _client
            var response = await _client.GetStringAsync(url);
            var json = JObject.Parse(response);

            string cidade = json["city"]?.ToString() ?? "Desconhecida";
            string pais = json["country"]?.ToString() ?? "Desconhecido";
            string fuso = json["timezone"]?.ToString();
            string status = json["status"]?.ToString();

            TimeZoneInfo tz = null;
            string horaLocalStr = "Hora local não disponível";

            if (!string.IsNullOrEmpty(fuso))
            {
                try
                {
                    tz = TZConvert.GetTimeZoneInfo(fuso);
                }
                catch
                {
                    try { tz = TimeZoneInfo.FindSystemTimeZoneById(fuso); }
                    catch { tz = null; }
                }

                if (tz != null)
                {
                    DateTime horaLocal = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tz);
                    horaLocalStr = horaLocal.ToString("HH:mm:ss");
                }
            }

            return new LocationResult
            {
                Cidade = cidade,
                Pais = pais,
                FusoHorario = fuso ?? "Desconhecido",
                HoraLocal = horaLocalStr,
                Status = status
            };
        }
    }

    public class LocationResult
    {
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public string FusoHorario { get; set; }
        public string HoraLocal { get; set; }
        public string Status { get; set; }
    }
}