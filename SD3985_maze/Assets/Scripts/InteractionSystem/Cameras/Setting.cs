using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Setting : MonoBehaviour
{
    public TextMeshProUGUI hint_text;
    //public Slider newVolume;
    // Start is called before the first frame update
    private void Start()
    {
        ChangeText(1);
        //newVolume = GetComponent<Slider>();
        //newVolume.onValueChanged.AddListener(OnNumSilderChange);
    }

    public void ChangeText(int page)
    {
        if (page == 1)
        {
            hint_text.text = @"
<color=#2fb9ff>Goal: Find the maze exit.</color>

<color=#d49577>Walk:</color> W A S D

<color=#d49577>Attack:</color> click left mouse key. Hold key to accumulate power.

<color=#d49577>Pick up stone or branch:</color> E

<color=#d49577>Put item back to ground:</color> Q

<color=#d49577>Enactive item in the UI item bar</color> from left to right: 1 2 3 4 

<color=#d49577>Pray infront of shrine:</color> E

                ";
        }
        else
        {
            hint_text.text = @"
<color=#d49577>Meat:</color> Consumable. Obtain from boar. It can expel the <color=#2fb9ff>Coldness</color>.

<color=#d49577>Potion:</color> Consumable. Drafted from beaten toad. It can recover your <color=red>HP</color>.

<color=#d49577>Stone:</color> Weapon. Click or long-press to throw a stone in the mouse direction.

<color=#d49577>Branch:</color>  Weapon. click or long-press to wave and deal damage to nearby enemy.
            ";
        }
    }
}