using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class StoneInteractions : MonoBehaviour
{
    public float pickUpDistance = 2f;

    private bool isNearStone = false;
/*    public bool isHoldingStone = false;*/
    public int holdingNumber = 0;
    public static int maxHoldingNumber = 2;
    public static bool isPowering = false;
    public TextMeshProUGUI stoneNoText;

    private GameObject stoneObject;
    private Transform throwSound;
    public GameObject stonePrefab;
    public float throwBaseSpeed = 15f;

    private float timeHeld;
    public static float maxHoldTime = 2f;

    //power bar
    public Image PowerIndicator;
    public Image Mask;
    public Image stoneUIimage;
    float maxPowerBarValue = 1f;

    public static bool allowPowering = true;

    private SwitchWeapon weaponSwitcher;
    private void Start()
    {
        throwSound = transform.GetChild(0);
        weaponSwitcher = GetComponent<SwitchWeapon>();
    }


    void Update()
    { 
        if (holdingNumber > 0 && weaponSwitcher.weapon_in_hand == 1 && (FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave) )
        {
            if (Input.GetMouseButton(0) && (FirstLevelTutorial.completed || FirstLevelTutorial.allowThrowStones) && allowPowering)
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
                }
                timeHeld += Time.deltaTime;
                timeHeld = Mathf.Clamp(timeHeld, 0f, maxHoldTime);
            }
            //else if (Input.GetMouseButtonUp(0))
            else if (Input.GetMouseButtonUp(0) && (FirstLevelTutorial.completed || FirstLevelTutorial.allowThrowStones) &&  allowPowering)//weapon in hand have been switched to be stone
            {

                PowerIndicator.gameObject.SetActive(false);
                if(timeHeld >= maxHoldTime / 3)
                {
                    ThrowStone();
                }
                timeHeld = 0f;
                Mask.fillAmount = 0f;
                isPowering = false;
            }
            //else if (Input.GetKeyDown(KeyCode.Q))
            else if (isPowering ==  false && Input.GetKeyDown(KeyCode.Q) && (FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave))//weapon in hand have been switched to be stone
            {
                DropDown();
            }
        }

        if (holdingNumber < maxHoldingNumber && isNearStone && Input.GetKeyDown(KeyCode.E) && (FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave))
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
        if (other.gameObject.CompareTag("Stone"))
        {
            isNearStone = true;
            stoneObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Stone"))
        {
            isNearStone = false;
            stoneObject = null;
        }
    }

    private void PickUp()
    {
        // isHoldingStone = true;
        holdingNumber++;
        if(weaponSwitcher.weapon_in_hand == 0)
            weaponSwitcher.weapon_in_hand = 1;
        UpdateNumberText(holdingNumber);
        Destroy(stoneObject);
        stoneUIimage.gameObject.SetActive(true);
        // TODO: add code to hold the stone

        FirstLevelTutorial.hasPickedUpStone = true;
    }
    private void DropDown()
    {
        // isHoldingStone = false;
        holdingNumber--;
        UpdateNumberText(holdingNumber);
        GameObject newStone = Instantiate(stonePrefab, transform.position, Quaternion.identity);

        GameObject bonfireObject = GameObject.Find("Player");
        BonfireInteractions bonfireInteractions = bonfireObject.GetComponent<BonfireInteractions>();
        bonfireInteractions.buildBonfire();
    }


    private void ThrowStone()
    {
        throwSound.gameObject.GetComponent<AudioSource>().Play();
        // isHoldingStone = false;
        holdingNumber--;
        UpdateNumberText(holdingNumber);
        

        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.WorldToScreenPoint(transform.position+ Vector3.up);
        Vector2 throwDirection = ((Vector2)(mousePosition - objectPosition)).normalized;
        // objectPosition += new Vector3(throwDirection.x, throwDirection.y, 0f);
        
        if (throwDirection.magnitude > 0f)
        {
            float throwForce = (1 + timeHeld) * (1 + timeHeld) * throwBaseSpeed;
            GameObject stone = Instantiate(stonePrefab, transform.position + (Vector3)(throwDirection * 0.5f) + Vector3.up, Quaternion.identity);
            stone.GetComponent<Collider2D>().isTrigger = false; // allow collision
            Rigidbody2D stoneRb = stone.GetComponent<Rigidbody2D>();
            stoneRb.velocity = throwDirection * throwForce;
            stoneRb.gameObject.layer = LayerMask.NameToLayer("FlyingObject");
        }
    }

    public void UpdateNumberText(int number){
        if(holdingNumber != 0)
        {
            stoneNoText.text = holdingNumber.ToString() + "/" + maxHoldingNumber.ToString();
            stoneUIimage.gameObject.SetActive(true);
        }
        else
        {
            stoneNoText.text = "";
            stoneUIimage.gameObject.SetActive(false);
            weaponSwitcher.weapon_in_hand = 0;
        }
        weaponSwitcher.HighlightInUI(weaponSwitcher.weapon_in_hand);
    }
}



