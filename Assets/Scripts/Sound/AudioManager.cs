using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region VARIABLES
    public static SoundData SoundData { get; private set; }
    [SerializeField] private SoundData soundData;
    public static AudioMixer AudioMixer { get; private set; }
    [SerializeField] private AudioMixer audioMixer;

    public static Action<SoundData.SoundEnum> OnSFXCall = _ => { };
    public static Action<AudioSource, SoundData.SoundEnum> OnAudioSourceSet = (_, _) => { };
    public static Action<AudioSource, bool, string> OnSetAudioSourceAttributes = (_, _, _) => { };

    #endregion

    #region UNITY EVENT FUNCTIONS
    private void Awake()
    {
        SoundData = SoundData ?? soundData;
        AudioMixer = AudioMixer ?? audioMixer;
    }

    #region Enable/Disable
    private void OnEnable()
    {
        OnSFXCall += PlaySFX;
        OnAudioSourceSet += SetAudioSourceClip;
        OnSetAudioSourceAttributes += SetAudioSourceAttributes;
    }

    private void OnDisable()
    {
        OnSFXCall -= PlaySFX;
        OnAudioSourceSet -= SetAudioSourceClip;
        OnSetAudioSourceAttributes -= SetAudioSourceAttributes;
    }
    #endregion

    #endregion

    private void SetAudioSourceAttributes(AudioSource audioSource, bool loop, string group)
    {
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(group)[0];
    }

    #region SFX Methods

    public async void PlaySFX(SoundData.SoundEnum sfxClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = soundData.GetSFXClip(sfxClip);
        audioSource.Play();

        await Task.Delay(Math.Max(1000, ((int)audioSource.clip.length) * 1000));
        DestroyImmediate(audioSource);
    }

    public async void SetAudioSourceClip(AudioSource audioSource, SoundData.SoundEnum sfxClip)
    {

        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
        audioSource.clip = soundData.GetSFXClip(sfxClip);
        audioSource.Play();

        await Task.Delay(Math.Max(1000, ((int)audioSource.clip.length) * 1000));
        if (destroyCancellationToken.IsCancellationRequested) return;
        audioSource.clip = null;
    }
    #endregion
}

public static class AudioExtensions
{
    #region Music Methods

    public static AudioClip RandomMusic(this AudioSource audioSource, AudioClip clip = null, SoundData soundData = null)
    {
        soundData = soundData ?? AudioManager.SoundData;

        audioSource.clip = soundData.Musics[Random.Range(0, soundData.Musics.Length)];
        return audioSource.clip == clip ? audioSource.RandomMusic(clip) : audioSource.clip;
    }

    public static void PlayRandomMusic(this AudioSource audioSource, AudioClip clip = null, SoundData soundData = null)
    {
        soundData = soundData ?? AudioManager.SoundData;

        if (soundData.Musics.Length < 2)
        {
            if (audioSource.clip == null)
            {
                audioSource.clip = soundData.Musics[0];
                audioSource.Play();
            }
            return;
        }

        clip = clip ?? audioSource.clip;
        while (clip == audioSource.clip) audioSource.clip = soundData.Musics[Random.Range(0, soundData.Musics.Length)];
        audioSource.Play();
    }
    #endregion

    public static void SetAudioSourceAttributes(this AudioSource audioSource, bool loop, string group, AudioMixer audioMixer = null)
    {
        audioSource.loop = loop;

        audioMixer = audioMixer ?? AudioManager.AudioMixer;
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(group)[0];
    }

    public static void SetAudioSourceAttributes(this AudioSource audioSource, bool loop, string group)
        => AudioManager.OnSetAudioSourceAttributes.Invoke(audioSource, loop, group);
    public static void PlaySFX(this SoundData.SoundEnum sfx) => AudioManager.OnSFXCall.Invoke(sfx);
    public static void SetAudioSourceClip(this AudioSource audioSource, SoundData.SoundEnum sfx) => AudioManager.OnAudioSourceSet.Invoke(audioSource, sfx);
}