using UnityEngine;
using UnityEngine.UIElements;

public class GaugesPresenter : VisualElement
{
    private ProgressBar healthBar;
    private HpBar hpBar;

    public GaugesPresenter(VisualElement root)
    {
        healthBar = root.Q<ProgressBar>("HealthBar");

        hpBar = new(healthBar);
    }

    public void HealthBarUpdate(float value)
    {
        healthBar.value += value;
        Debug.Log("Health : " + healthBar.value + "    ||    Value : " + value);
    }
}
