using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip introMusic;
    public AudioClip loopMusic;

    [Header("Volume")]
    public float introVolume;
    public float loopVolume;

    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (introMusic != null)
        {
            audioSource.volume = introVolume;
            audioSource.clip = introMusic;
            audioSource.loop = false;
            audioSource.Play();
        }
        else if (loopMusic != null)
        {
            PlayLoopMusic();
        }
    }

    void PlayLoopMusic()
    {
        if (loopMusic != null)
        {
            audioSource.volume = loopVolume;
            audioSource.clip = loopMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
