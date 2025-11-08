using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class DetectColor : MonoBehaviour
{
    [Header("Variavel do objeto a modificar")]
    [SerializeField] private GameObject _objeto;

    [Header("Modelos 3D disponíveis (nome deve bater com o detectado)")]
    public GameObject galinhaModel;
    public GameObject porcoModel;
    public GameObject vacaModel;

    [Header("Imagem a ser analisada")]
    [SerializeField] private Texture2D _imagem;

    [Header("Configurações do modelo - Gemini")]
    static readonly string apiKeyGemini = "AIzaSyBwF3flFgmRQ6NecfNmv5zonJEqSkT198A";
    static readonly string modeloGemini = "gemini-2.5-flash";
    static readonly string urlGemini = $"https://generativelanguage.googleapis.com/v1beta/models/{modeloGemini}:generateContent?key={apiKeyGemini}";

    private GameObject modeloAtivo;

    async void Start()
    {
        if (_imagem == null)
        {
            Debug.LogError("Nenhuma imagem atribuída!");
            return;
        }

        await AnalisarEAplicarCores(_imagem);
    }

    public async Task AnalisarEAplicarCores(Texture2D img)
    {
        string imagemBase64 = Convert.ToBase64String(img.EncodeToJPG());

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
                            new
                            {
                                text = "Analise esta imagem de um desenho pintado por uma criança. Identifique qual animal aparece e as cores predominantes em suas partes principais. " +
                                       "Retorne apenas um JSON no formato: { \"animal\": \"nome\", \"partes\": { \"parte1\": \"#RRGGBB\", \"parte2\": \"#RRGGBB\" } }"
                            },
                            new
                            {
                                inline_data = new
                                {
                                    mime_type = "image/jpeg",
                                    data = imagemBase64
                                }
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(urlGemini, content);
            string result = await response.Content.ReadAsStringAsync();

            var jsonDoc = JObject.Parse(result);
            string? respostaTexto = (string?)jsonDoc["candidates"]?[0]?["content"]?["parts"]?[0]?["text"];

            if (string.IsNullOrEmpty(respostaTexto))
            {
                Debug.LogError("Nenhuma resposta válida do modelo.");
                return;
            }

            Debug.Log($"Resposta do Gemini: {respostaTexto}");

            try
            {
                var dados = JObject.Parse(respostaTexto);
                string animal = (string)dados["animal"];
                var partes = dados["partes"].ToObject<Dictionary<string, string>>();

                Debug.Log($"Animal detectado: {animal}");

                SelecionarModelo(animal);

                if (modeloAtivo != null)
                    AplicarCoresNasPartes(modeloAtivo, partes);
                else
                    Debug.LogWarning($"Nenhum modelo encontrado para o animal: {animal}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Erro ao interpretar a resposta: {e.Message}");
            }
        }
    }

    void SelecionarModelo(string nome)
    {
        if (modeloAtivo != null) modeloAtivo.SetActive(false);

        nome = nome.ToLower();
        switch (nome)
        {
            case "galinha": modeloAtivo = galinhaModel; break;
            case "porco": modeloAtivo = porcoModel; break;
            case "vaca": modeloAtivo = vacaModel; break;
            default:
                modeloAtivo = null;
                Debug.LogWarning($"Animal '{nome}' não reconhecido.");
                break;
        }

        if (modeloAtivo != null) modeloAtivo.SetActive(true);
    }

    void AplicarCoresNasPartes(GameObject modelo, Dictionary<string, string> partes)
    {
        foreach (var par in partes)
        {
            string parteNome = par.Key.ToLower();
            string corHex = par.Value;

            Transform parte = BuscarParte(modelo.transform, parteNome);
            if (parte == null)
            {
                Debug.LogWarning($"Parte '{parteNome}' não encontrada no modelo.");
                continue;
            }

            if (ColorUtility.TryParseHtmlString(corHex, out Color cor))
            {
                if (parte.TryGetComponent<Renderer>(out var renderer))
                {
                    renderer.material.color = cor;
                    Debug.Log($"Parte '{parteNome}' colorida com {corHex}");
                }
            }
        }
    }

    Transform BuscarParte(Transform pai, string nome)
    {
        foreach (Transform filho in pai)
        {
            if (filho.name.ToLower().Contains(nome))
                return filho;

            Transform achado = BuscarParte(filho, nome);
            if (achado != null)
                return achado;
        }
        return null;
    }
}

