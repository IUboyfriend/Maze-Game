using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MeatInteraction : MonoBehaviour
{
    public float pickUpDistance = 2f;

    private bool isNearMeat = false;
    // public bool isHoldingMeat = false;
    public int holdingNumber = 0;
    public static int maxHoldingNumber = 2;
    public TextMeshProUGUI meatNoText;
    public Image meatUIimage;
    public static bool isPowering = false;

    private GameObject meatObject;
    private Transform chewingSound;
    private Transform eatingFinishSound;
    public GameObject meatPrefab;

    private float timeHeld = 0f;
    public float maxHoldTime;

    //power bar
    public Image PowerIndicator;
    public Image Mask;
    float maxPowerBarValue = 1f;

    private SwitchWeapon weaponSwitcher;
    public static bool allowPowering = true;


    private void Start()
    {

        eatingFinishSound = transform.Find("SE_EatingFinish");
        chewingSound = transform.Find("SE_Chewing");
        weaponSwitcher = GetComponent<SwitchWeapon>();
    }

    void Update()
    {
        
        if (weaponSwitcher.weapon_in_hand == 4 && holdingNumber != 0 && Input.GetMouseButton(0)&& timeHeld <maxHoldTime && allowPowering)
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
                chewingSound.gameObject.GetComponent<AudioSource>().Play();
            }
            timeHeld += Time.deltaTime;
            timeHeld = Mathf.Clamp(timeHeld, 0f, maxHoldTime);
        } 
        else if (Input.GetMouseButton(0) && timeHeld >= maxHoldTime && allowPowering)
        {


            chewingSound.gameObject.GetComponent<AudioSource>().Stop();
            PowerIndicator.gameObject.SetActive(false);
            EatMeat();
            timeHeld = 0f;
            Mask.fillAmount = 0f;
            isPowering = false;

        }

        else if (Input.GetMouseButtonUp(0) && allowPowering)// && weaponSwitcher.weapon_in_hand == 4
        {
            chewingSound.gameObject.GetComponent<AudioSource>().Stop();
            PowerIndicator.gameObject.SetActive(false);
            // isHoldingMeat= false;
            timeHeld = 0f;
            Mask.fillAmount = 0f;
            isPowering = false;
        }

        else if (isPowering == false && Input.GetKeyDown(KeyCode.Q) && weaponSwitcher.weapon_in_hand == 4)
        {
            DropDown();
        }
        
        if (holdingNumber < maxHoldingNumber && isNearMeat && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
        

        
        // if(maxHoldingNumber != 2)
        // {
        //     UpdateNumberText(holdingNumber);
        // }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Meat"))
        {
            isNearMeat = true;
            meatObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Meat"))
        {
            isNearMeat = false;
            meatObject = null;
        }
    }

    private void PickUp()
    {
        // isHoldingMeat = true;
        if(weaponSwitcher.weapon_in_hand == 0){
            weaponSwitcher.weapon_in_hand = 4;
        }
        holdingNumber++;
        UpdateNumberText(holdingNumber);
        Destroy(meatObject);
        //meatOverrider
        //meatUIimage.gameObject.SetActive(true);
    }

    private void DropDown()
    {
        // isHoldingMeat = false;
        holdingNumber--;
        UpdateNumberText(holdingNumber);
        GameObject newMeat = Instantiate(meatPrefab, transform.position, Quaternion.identity);
        //meatUIimage.gameObject.SetActive(false);
        //transform.GetChild(4).gameObject.SetActive(false);
    }



    private void EatMeat()
    {
        FirstLevelTutorial.hasTakenMeat = true;
        holdingNumber--;
        UpdateNumberText(holdingNumber);
        eatingFinishSound.gameObject.GetComponent<AudioSource>().Play();
        CharacterStatus.currentColdness = Mathf.Max(CharacterStatus.currentColdness - 40f,0f) ;

    }

    public void UpdateNumberText(int number){
        if(holdingNumber != 0){
            meatNoText.text = holdingNumber.ToString() + "/" + maxHoldingNumber.ToString();
            meatUIimage.gameObject.SetActive(true);
        }
        else
        {
            meatNoText.text = "";
            meatUIimage.gameObject.SetActive(false);
            weaponSwitcher.weapon_in_hand = 0;
        }
        weaponSwitcher.HighlightInUI(weaponSwitcher.weapon_in_hand);
    }

}



