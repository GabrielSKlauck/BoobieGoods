using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadAnimalScene()
    {
        LoadingManager.sceneToLoad = "AnimalSelect";
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadScanearAnimalScene()
    {
        LoadingManager.sceneToLoad = "CameraRAScene";
        SceneManager.LoadScene("LoadingScene");
    }
}
