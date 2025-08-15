using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using TimeZoneConverter;

namespace LocationTeller
{
    public class Form1 : Form
    {
        private Button btnLocalizar;
        private Label lblResultado;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Location Timer Teller";
            this.ClientSize = new System.Drawing.Size(420, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            btnLocalizar = new Button
            {
                Text = "Localizar",
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(100, 30)
            };
            btnLocalizar.Click += btnLocalizar_Click;

            lblResultado = new Label
            {
                Location = new System.Drawing.Point(15, 60),
                Size = new System.Drawing.Size(390, 100),
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.Add(btnLocalizar);
            this.Controls.Add(lblResultado);
        }

        private async void btnLocalizar_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "Obtendo informações...";
            try
            {
                var resultado = await ObterLocalizacaoAsync();
                lblResultado.Text = resultado;
            }
            catch (Exception ex)
            {
                lblResultado.Text = $"Erro: {ex.Message}";
            }
        }

        private async Task<string> ObterLocalizacaoAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://ip-api.com/json";
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);

                string cidade = json["city"]?.ToString() ?? "Desconhecida";
                string pais = json["country"]?.ToString() ?? "Desconhecido";
                string fuso = json["timezone"]?.ToString();

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

                return $"Localização aproximada: {cidade}, {pais}\nFuso horário: {fuso ?? "Desconhecido"}\nHora local: {horaLocalStr}";
            }
        }
    }
}
