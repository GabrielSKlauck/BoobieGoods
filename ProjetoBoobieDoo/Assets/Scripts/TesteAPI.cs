using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class TesteAPI : MonoBehaviour
{
    [Header("Variavel do cubo a modificar")]
    [SerializeField]
    private GameObject cubo;
    static string apiKeyGemini = "AIzaSyBwF3flFgmRQ6NecfNmv5zonJEqSkT198A";
    static string modeloGemini = "gemini-2.5-flash";
    static string urlGemini = $"https://generativelanguage.googleapis.com/v1beta/models/{modeloGemini}:generateContent?key={apiKeyGemini}";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        Debug.Log("Startou");
        string corDesenho = await RequisicaoGemini("C:\\Users\\2xthe\\Downloads\\GalinhaPintada.jpg");
        Color newColor;
        if (ColorUtility.TryParseHtmlString(corDesenho, out newColor))
        {
            cubo.GetComponent<Renderer>().material.color = newColor;
        }

        //UnityEngine.Color color = Color.FromString(corDesenho);
        //cubo.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    static async Task<string> RequisicaoGemini(string caminho)
    {
        Debug.Log("Fazendo requisicao");
        string imagemBase64 = Convert.ToBase64String(File.ReadAllBytes(caminho));
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                contents = new[]
        {
                new
                {
                    parts = new object[]
                    {
                        new { text = "Analise esta imagem e me retorne qual a cor predomintante na figura pintada presente nela. Retorne somente a cor em formato hexadecimal" },
                        new {
                            inline_data = new {
                                mime_type = "image/jpeg",
                                data = imagemBase64
                            }
                        }
                    }
                }
            }
            };

            string json = JsonConvert.SerializeObject(requestBody, Newtonsoft.Json.Formatting.Indented);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(urlGemini, content);
            string result = await response.Content.ReadAsStringAsync();

            var jsonDoc = JObject.Parse(result);

            // Pega o valor de "text"
            string? respostaModelo = (string?)jsonDoc["candidates"]?[0]?["content"]?["parts"]?[0]?["text"];

            return respostaModelo;
        }
    }
}

