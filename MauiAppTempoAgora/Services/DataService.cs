using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "3a5a72f414b93203a5bfd58e0ac52305";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp;
                try
                {
                    resp = await client.GetAsync(url);
                }
                catch (HttpRequestException)
                {
                    throw new Exception("Sem conexão com a internet.");
                }

                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Cidade não encontrada.");
                }
                else if (!resp.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro ao buscar dados: {resp.StatusCode}");
                }

                string json = await resp.Content.ReadAsStringAsync();

                var rascunho = JObject.Parse(json);

                DateTime time = new();
                DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                t = new()
                {
                    lat = (double)rascunho["coord"]["lat"],
                    lon = (double)rascunho["coord"]["lon"],
                    description = (string)rascunho["weather"][0]["description"],
                    main = (string)rascunho["weather"][0]["main"],
                    temp_min = (double)rascunho["main"]["temp_min"],
                    temp_max = (double)rascunho["main"]["temp_max"],
                    speed = (double)rascunho["wind"]["speed"],
                    visibility = (int)rascunho["visibility"],
                    sunrise = sunrise.ToString(),
                    sunset = sunset.ToString(),
                };
            }

            return t;
        }
    }
}