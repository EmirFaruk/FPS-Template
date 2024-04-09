using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsMenuPresenter
{
    #region VARIABLES
    public static event Action OnBackButtonClicked = () => { };
    public static event Action<(int id, float value)> OnSliderValueChanged = _ => { };
    public static Func<int, float> OnGetSlidersValue;

    public static string[] savedNames = new string[] { "MasterVolume", "MusicVolume", "SfxVolume" };

    private const string BUTTON_BACK = "Button_Back";

    private Slider[] sliders = new Slider[3];
    #endregion

    public OptionsMenuPresenter(VisualElement root)
    {
        root.Q<Button>(BUTTON_BACK).clicked += () => OnBackButtonClicked.Invoke();

        var slidersElement = root.Q<VisualElement>("Sliders").Children();
        for (int i = 0; i < slidersElement.Count(); i++)
        {
            // Get the slider element and add it to the array
            sliders[i] = slidersElement.ElementAt(i).Q<Slider>();

            // Load the slider value when the scene starts
            sliders[i].value = LoadSliderValue(i).Item2;

            // Load the sound value when the scene starts
            OnGetSlidersValue += GetLoadedValues;
        }

        // Save the slider value when the slider value changes
        foreach (var item in sliders.Select((value, i) => (value, i)))
        {
            item.value.RegisterValueChangedCallback(slider => OnSliderValueChanged.Invoke(SaveSliderValue(item.i, slider.newValue)));
        }
    }

    private (int, float) SaveSliderValue(int index, float value)
    {
        PlayerPrefs.SetFloat(savedNames[index], value);
        return (index, value);
    }

    private (int, float) LoadSliderValue(int index)
    {
        if (!PlayerPrefs.HasKey(savedNames[index])) return (index, sliders[index].value);
        return (index, PlayerPrefs.GetFloat(savedNames[index]));
    }

    public float GetLoadedValues(int i)
    {
        return PlayerPrefs.HasKey(savedNames[i]) ? PlayerPrefs.GetFloat(savedNames[i]) : sliders[i].value;
    }
}
