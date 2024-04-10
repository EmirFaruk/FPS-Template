using UnityEngine;

public class SoundMixerManager : MonoBehaviour
{
    public static readonly string[] volumesNames = new string[] { "MasterVolume", "MusicVolume", "SfxVolume" };

    private void OnEnable()
    {
        OptionsMenuPresenter.OnSliderValueChanged += SetVolume;
    }

    private void OnDisable()
    {
        OptionsMenuPresenter.OnSliderValueChanged -= SetVolume;
    }

    private void SetVolume((int, float) value)
    {
        var volume = Mathf.Log10(Mathf.Clamp(value.Item2, 0.0001f, 1f));
        AudioManager.AudioMixer.SetFloat(volumesNames[value.Item1], volume * 20f);
    }

    private void GetVolumeOnStart()
    {
        for (int i = 0; i < volumesNames.Length; i++)
        {
            SetVolume((i, OptionsMenuPresenter.OnGetSlidersValue.Invoke(i)));
        }
    }

    private void Start()
    {
        GetVolumeOnStart();
    }
}
