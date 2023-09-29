using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapon : MonoBehaviour
{
    private StoneInteractions stone;
    private BranchInteraction branch;
    private PotionInteraction potion;
    private MeatInteraction meat;
    public Image ItemUI;

    public int weapon_in_hand;// 1 means stone, 2 means branch
    //int item_in_hand;  // 3 means portion, 4 means meat.

    //private Vector3 origialPostion = new Vector3(0, 0, 0);
    //private Vector3 downPosition = new Vector3(0, -0.01f, 0);
    private Color32 Half_Transparent = new Color32(200, 200, 200, 100);
    private AudioSource switchsound;

    void Start()
    {
        //origialPostion = ItemUI.transform.GetChild(0).gameObject.transform.position;//stone
        stone = GetComponent<StoneInteractions>();
        branch = GetComponent<BranchInteraction>();
        potion = GetComponent<PotionInteraction>();
        meat = GetComponent<MeatInteraction>();
        weapon_in_hand = 0;
        switchsound = transform.Find("SE_SwitchWeapon").GetComponent<AudioSource>();
        //item_in_hand = 0;
    }
    //int i = 0;
    private void Update()
    {
        // if (stone.holdNumber > 0 || branch.holdNumber > 0)
        // {
        //     if (!(stone.holdNumber > 0) && weapon_in_hand != 2)//taking branch (2) only
        //     {
        //         weapon_in_hand = 2;
        //         HighlightInUI(weapon_in_hand);
        //     }
        //     else if (!(branch.holdNumber > 0) && weapon_in_hand != 1)//taking stone (1) only
        //     {
        //         weapon_in_hand = 1;
        //         HighlightInUI(weapon_in_hand);
        //     }
        //     else //when taking both
        //     {
        // Debug.Log("taking both");
        // for(; i < 1; i++)
        // {
        //     HighlightInUI(weapon_in_hand - 1);
        // }
        // do nothing, so the player will keep the former until press number key.
/*        if ((FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave))
        {*/
            if (!Input.GetMouseButton(0))
            {
                if (stone.holdingNumber != 0 && Input.GetKeyDown(KeyCode.Alpha1) && weapon_in_hand != 1) //press 1 when not taking stone (1)
                {
                    weapon_in_hand = 1;
                    Debug.Log("Change to stone.");
                    HighlightInUI(weapon_in_hand);

                    FirstLevelTutorial.hasPress1 = true;

                }
                else if (branch.holdingNumber != 0 && Input.GetKeyDown(KeyCode.Alpha2) && weapon_in_hand != 2) //press 2 when not taking branch (2)
                {
                    weapon_in_hand = 2;
                    Debug.Log("Change to branch.Postion:" + ItemUI.transform.GetChild(1).gameObject.transform.position);
                    HighlightInUI(weapon_in_hand);
                    Debug.Log("After, Postion:" + ItemUI.transform.GetChild(1).gameObject.transform.position);
                }
                else if (potion.holdingNumber != 0 && Input.GetKeyDown(KeyCode.Alpha3) && weapon_in_hand != 3)
                {
                    weapon_in_hand = 3;
                    HighlightInUI(weapon_in_hand);
                }
                else if (meat.holdingNumber != 0 && Input.GetKeyDown(KeyCode.Alpha4) && weapon_in_hand != 4)
                {
                    weapon_in_hand = 4;
                    HighlightInUI(weapon_in_hand);
                }
            }
/*        }*/
        // }
    }
    // public int abs(int a) { return (a ^ (a >> 31)) - (a >> 31); }

    public void HighlightInUI(int weapon)// stone use 0, branch use 1 (hierarchy)
    {
        switchsound.Play();
        if (weapon == 2) transform.Find("BranchOverrider").gameObject.SetActive(true);
        else transform.Find("BranchOverrider").gameObject.SetActive(false);
        weapon -= 1;

        for (int i = 0; i < 4; i++)
        {
            if (i == weapon)
            {
                //ItemUI.transform.GetChild(i).gameObject.transform.position = origialPostion;
                ItemUI.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                //ItemUI.transform.GetChild(i).gameObject.transform.position = origialPostion;
                ItemUI.transform.GetChild(i).gameObject.GetComponent<Image>().color = Half_Transparent;
            }
        }
    }
}
