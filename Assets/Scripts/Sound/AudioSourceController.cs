using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    private AudioSource musicAudioSource;

    private void OnEnable()
    {
        MainMenuPresenter.OnStartButtonClicked += () => musicAudioSource.PlayRandomMusic();
    }

    private void OnDisable()
    {
        MainMenuPresenter.OnStartButtonClicked -= () => musicAudioSource.PlayRandomMusic();
    }

    private void Awake()
    {
        musicAudioSource = musicAudioSource ?? Instantiate(new GameObject("MusicAudioSource"), transform).AddComponent<AudioSource>();
    }

    void Start()
    {
        musicAudioSource.SetAudioSourceAttributes(true, "Music");
        musicAudioSource.PlayRandomMusic();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) musicAudioSource.PlayRandomMusic(musicAudioSource.clip);
    }
}
