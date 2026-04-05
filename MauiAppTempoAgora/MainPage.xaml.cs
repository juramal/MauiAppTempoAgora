using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = null;
                    try
                    {
                        t = await DataService.GetPrevisao(txt_cidade.Text);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Cidade não encontrada"))
                        {
                            lbl_res.Text = "Cidade não encontrada.";
                            return;
                        }
                        else if (ex.Message.Contains("Sem conexão"))
                        {
                            await DisplayAlert("Sem conexão", "Verifique sua conexão com a internet.", "OK");
                            return;
                        }
                        else
                        {
                            await DisplayAlert("Erro", ex.Message, "OK");
                            return;
                        }
                    }

                    if (t != null)
                    {
                        string dados_previsao = "";
                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Descrição: {t.description} \n" +
                                         $"Velocidade do vento: {t.speed} m/s \n" +
                                         $"Visibilidade: {t.visibility} m \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}