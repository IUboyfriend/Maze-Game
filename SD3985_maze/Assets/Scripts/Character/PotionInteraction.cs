using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PotionInteraction : MonoBehaviour
{
    public float pickUpDistance = 2f;

    private bool isNearPotion = false;
    // public bool isHoldingPotion = false;

    public int holdingNumber = 0;

    public static int maxHoldingNumber = 2;

    public TextMeshProUGUI potionNoText;
    public Image potionUIimage;
    public static bool isPowering = false;

    private GameObject potionObject;
    private Transform drinkingSound;
    private Transform drinkingFinishSound;
    public GameObject potionPrefab;

    private float timeHeld;
    public float maxHoldTime;

    //power bar
    public Image PowerIndicator;
    public Image Mask;
    /*    public Image MeatUIImage;*/
    float maxPowerBarValue = 1f;

    private SwitchWeapon weaponSwitcher;
    public static bool allowPowering = true;

    private void Start()
    {
        drinkingFinishSound = transform.Find("SE_DrinkingFinish");
        drinkingSound = transform.Find("SE_Drinking");
        weaponSwitcher = GetComponent<SwitchWeapon>();
    }


    void Update()
    {
        // take the potion
        if (weaponSwitcher.weapon_in_hand == 3 && holdingNumber != 0 && CharacterStatus.currentHealth != 100 && allowPowering)
        {
            if (Input.GetMouseButton(0) && timeHeld < maxHoldTime)
            {
                if (isPowering == true)
                {
                    float fill = timeHeld / maxHoldTime * maxPowerBarValue;
                    Mask.fillAmount = fill;
                }
                else
                {
                    isPowering = true;
                    PowerIndicator.gameObject.SetActive(true);
                    drinkingSound.gameObject.GetComponent<AudioSource>().Play();
                }
                timeHeld += Time.deltaTime;
                timeHeld = Mathf.Clamp(timeHeld, 0f, maxHoldTime);
            }
            else if (Input.GetMouseButton(0) && timeHeld >= maxHoldTime)
            {
                drinkingSound.gameObject.GetComponent<AudioSource>().Stop();
                PowerIndicator.gameObject.SetActive(false);
                drinkPotion();
                timeHeld = 0f;
                Mask.fillAmount = 0f;
                isPowering = false;
            }

            else if (Input.GetMouseButtonUp(0))// && weaponSwitcher.weapon_in_hand == 3
            {
                drinkingSound.gameObject.GetComponent<AudioSource>().Stop();
                // isHoldingPotion = false;
                PowerIndicator.gameObject.SetActive(false);
                timeHeld = 0f;
                Mask.fillAmount = 0f;
                isPowering = false;
            }
        }
        else if (weaponSwitcher.weapon_in_hand == 3 && holdingNumber != 0 && CharacterStatus.currentHealth >= 100 && allowPowering)
        {
            if (Input.GetMouseButton(0)){
                potionUIimage.GetComponent<Animator>().enabled = true;
                potionUIimage.GetComponent<Animator>().Play("ption",0,0);
                potionUIimage.GetComponent<AudioSource>().Play();
            }
        }

        if (isPowering == false && Input.GetKeyDown(KeyCode.Q) && weaponSwitcher.weapon_in_hand == 3)
        {
            DropDown();
        }
        //pick up
        if (holdingNumber < maxHoldingNumber && isNearPotion && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }


        // if (maxHoldingNumber != 2)
        // {
        //     UpdateNumberText(holdingNumber);
        // }

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Potion"))
        {
            isNearPotion = true;
            potionObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Potion"))
        {
            isNearPotion = false;
            potionObject = null;
        }
    }

    private void PickUp()
    {
        // isHoldingPotion = true;
        Debug.Log("pick up");
        if (weaponSwitcher.weapon_in_hand == 0)
        {
            weaponSwitcher.weapon_in_hand = 3;
        }
        holdingNumber++;
        UpdateNumberText(holdingNumber);
        Destroy(potionObject);
        //meatOverrider
        //meatUIimage.gameObject.SetActive(true);
    }

    private void DropDown()
    {
        // isHoldingPotion = false;
        holdingNumber--;
        UpdateNumberText(holdingNumber);
        GameObject newPotion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
        //meatUIimage.gameObject.SetActive(false);
        //transform.GetChild(4).gameObject.SetActive(false);
    }

    private void drinkPotion()
    {
        drinkingFinishSound.gameObject.GetComponent<AudioSource>().Play();
        FirstLevelTutorial.hasTakenPotion = true;
        CharacterStatus.currentHealth = Mathf.Min(CharacterStatus.currentHealth + 20, 100);
        holdingNumber--;
        UpdateNumberText(holdingNumber);
    }

    public void UpdateNumberText(int number)
    {
        if (holdingNumber != 0)
        {
            potionNoText.text = holdingNumber.ToString() + "/" + maxHoldingNumber.ToString();
            potionUIimage.gameObject.SetActive(true);
        }
        else
        {
            potionNoText.text = "";
            potionUIimage.gameObject.SetActive(false);
            weaponSwitcher.weapon_in_hand = 0;
        }
        weaponSwitcher.HighlightInUI(weaponSwitcher.weapon_in_hand);
    }


}



