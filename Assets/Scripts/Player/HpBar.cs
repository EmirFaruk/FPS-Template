using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HpBar : MonoBehaviour
{
    private ProgressBar healthBar;

    public HpBar(ProgressBar healthBar)
    {
        this.healthBar = healthBar;
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
    }

    async void UpdateHealthBar(float value)
    {
        if (healthBar.value > 0)
        {
            float speed = 3000;
            while (healthBar.value > value)
            {
                healthBar.value -= Time.deltaTime * speed;
                speed = Mathf.Max(100, speed * .75f);
                await Task.Delay(10);
            }

            if (healthBar.value <= .0f) PlayerHealth.OnDeath.Invoke();
        }
    }
}