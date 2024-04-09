using UnityEngine;

public class SoundMixerManager : MonoBehaviour
{
    public static readonly string[] sliderNames = new string[] { "MasterVolume", "MusicVolume", "SfxVolume" };

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
        value.Item2 = Mathf.Clamp(value.Item2, 0.0001f, 1f);
        AudioManager.AudioMixer.SetFloat(sliderNames[value.Item1], Mathf.Log10(value.Item2) * 20f);
    }

    private void GetVolumeOnStart()
    {
        for (int i = 0; i < sliderNames.Length; i++)
        {
            SetVolume((i, OptionsMenuPresenter.OnGetSlidersValue.Invoke(i)));
        }
    }

    private void Start()
    {
        GetVolumeOnStart();
    }
}
