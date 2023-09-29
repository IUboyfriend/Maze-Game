using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    // public Gradient gradient;
    // public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        // fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = health.ToString();
        // fill.color = gradient.Evaluate(slider.normalizedValue); // Normalized value is a value between 0 and 1
    }
}
