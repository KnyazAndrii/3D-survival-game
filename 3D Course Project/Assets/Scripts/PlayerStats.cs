using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float Health = 100;
    private float _healthMax = 100;

    public float Hunger = 100;
    private float _hungerMax = 100;

    private float _healthMinus = 0.01f;
    private float _hungerMinus = 0.01f;

    public Image HealthImage;
    public Image HungerImage;

    private void Update()
    {
        HealthImage.fillAmount = Health / _healthMax;
        HungerImage.fillAmount = Hunger / _hungerMax;
    }

    private void FixedUpdate()
    {
        Hunger -= _hungerMinus;

        Health = Mathf.Clamp(Health, 0, _healthMax);
        Hunger = Mathf.Clamp(Hunger, 0, _hungerMax);

        if (Hunger == 0)
        {
            Health -= _healthMinus;
        }
        else if(Hunger > _hungerMax / 100 * 80)
        {
            Health += _healthMinus;
        }
    }
}
