using TMPro;
using UnityEngine;
using System;

public class StateBar : MonoBehaviour
{
    private Transform health; 
    private Transform coldness;

    private float health_percent;
    private float cold_percent;
    void Start()
    {
        health=this.transform.GetChild(0);
        coldness=this.transform.GetChild(1);
        health.gameObject.GetComponent<RectTransform>().SetRight(-100);
        coldness.gameObject.GetComponent<RectTransform>().SetRight(320);
    }

    void FixedUpdate()
    {
        {
            float crt_h = (float)CharacterStatus.currentHealth;
            // Calculate health and coldness percentages
            health_percent = crt_h/ (float)CharacterStatus.maxHealth;
            cold_percent = 1f - CharacterStatus.currentColdness / CharacterStatus.maxColdness;

            // Update the health and coldness bars
            float healthBarLength = Mathf.Lerp(200f, -100f, health_percent);
            health.gameObject.GetComponent<RectTransform>().SetRight(healthBarLength);
            transform.parent.Find("UIText").Find("HPtext").GetComponent<TextMeshProUGUI>().text = $"<color=#FD4244>HP</color> " + Math.Max((int)crt_h,0) + "/" + CharacterStatus.maxHealth;

            float coldnessBarLength = Mathf.Lerp(12f, 320f, cold_percent);
            coldness.gameObject.GetComponent<RectTransform>().SetRight(coldnessBarLength);
        }
    }
}
