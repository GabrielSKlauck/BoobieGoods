using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Vuforia;

public class VuforiaColorIntegration : MonoBehaviour
{
    public Texture2D imagemCapturada;
    public DetectColor detectColor;
    public ObserverBehaviour imageTarget;

    private bool _processando = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (imageTarget == null)
        {
            imageTarget = GetComponent<ObserverBehaviour>();
        }
        if (imageTarget)
        {
            imageTarget.OnTargetStatusChanged += OnObserverStatusChanged;
        }
    }

    private async void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED && !_processando)
        {
            Debug.Log("Imagem detectada pelo Vuforia");
            _processando = true;

            await CapturarEAnalisar();

            _processando = false;
        } else if (targetStatus.Status == Status.NO_POSE)
        {
            _processando = false;
        }
    }

    private async Task CapturarEAnalisar()
    {
        await Task.Yield();

        Texture2D frame = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
        frame.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        frame.Apply();

        Debug.Log("Enviando imagem capturada para análise");
        await detectColor.AnalisarEAplicarCores(frame);

        Debug.Log("Cores aplicadas no modelo!");
    }
}
