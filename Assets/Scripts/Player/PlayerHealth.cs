using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float maxHealth = 100;
    public float MaxHealth => maxHealth;
    private float currentHealth;

    //Damage
    public static Action<float> OnTakeDamage = _ => { };
    public static Action<float> OnHealthChanged = _ => { };
    public static Action ActivateTakeDamageEffect = () => { };
    public static Action OnDeath = () => { };

    #endregion

    #region UNITY EVENT FUNCTIONS    

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    #endregion

    #region METHODS

    public void ApplyDamage(float damage)
    {
        currentHealth = Math.Max(0, currentHealth + damage);

        ActivateTakeDamageEffect.Invoke();

        OnHealthChanged.Invoke(currentHealth);

        //  Helper.Camera.DOShakeRotation(.5f, 30, 5);                 
    }

    #endregion
}