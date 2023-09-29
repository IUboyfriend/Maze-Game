using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BranchInteraction : MonoBehaviour
{
    public float pickUpDistance = 2f;

    public static bool isNearBranch = false;
    public bool isHoldingBranch = false;
    public static bool isPowering = false;

    private GameObject branchObject;
    private AudioSource waveSound;
    public GameObject branchPrefab;
    public TextMeshProUGUI branchNoText;

    private float timeHeld;
    public static float maxHoldTime = 2f;
    public int holdingNumber = 0;
    public static int maxHoldingNumber = 2;

    //power bar

    public Image PowerIndicator;
    public Image Mask;
    public Image branchUIimage;
    float maxPowerBarValue = 1f;

    private SwitchWeapon weaponSwitcher;
    public GameObject branchEffect;

    public static bool allowPowering = true;

    public float branchAttackRange = 1.5f;


    private bool canAttack = true;
    public float attackIntervalTime;



    private void Start()
    {

        waveSound = transform.Find("SE_WaveBranch").gameObject.GetComponent<AudioSource>();
        weaponSwitcher = GetComponent<SwitchWeapon>();
    }


    /*    private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position+ GetBranchOffset(7), 1f);
        }

    */
    void Update()
    {
/*
        Mask = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();*/
        // Debug.Log("holdingNumber: " + holdingNumber);
        if (holdingNumber > 0 && weaponSwitcher.weapon_in_hand == 2 && (FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave))
        {
            if (Input.GetMouseButton(0) && canAttack)
            {
                if (isPowering == true)
                {
                    float fill = timeHeld / maxHoldTime * maxPowerBarValue;
                    Mask.fillAmount = fill;
                    Debug.Log("fill amount" + Mask.fillAmount);
                }
                else
                {
                    isPowering = true;
                    PowerIndicator.gameObject.SetActive(true);
                }
                timeHeld += Time.deltaTime;
                timeHeld = Mathf.Clamp(timeHeld, 0f, maxHoldTime);
            }
            /*            else if (Input.GetMouseButtonUp(0) && canAttack)//weapon in hand have been switched to be branch
                        {
                            canAttack = false;
                            StartCoroutine(attackInterval());
                            PowerIndicator.gameObject.SetActive(false);

                            WaveBranch();
                            timeHeld = 0f;
                            Mask.fillAmount = 0f;
                            isPowering = false;
                            // Debug.Log("branch attack");
                        }*/

            else if (Input.GetMouseButtonUp(0))//weapon in hand have been switched to be branch
            {
                PowerIndicator.gameObject.SetActive(false);
                if(timeHeld >= maxHoldTime / 3)
                {
                    WaveBranch();
                }

                timeHeld = 0f;
                Mask.fillAmount = 0f;
                isPowering = false;
                // Debug.Log("branch attack");
            }
            //else if (Input.GetKeyDown(KeyCode.Q))
            else if (isPowering == false && Input.GetKeyDown(KeyCode.Q) && (FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave))//weapon in hand have been switched to be branch
            {
                DropDown();
            }
        }

        if (holdingNumber < maxHoldingNumber && isNearBranch && Input.GetKeyDown(KeyCode.E) && (FirstLevelTutorial.completed || FirstLevelTutorial.allowBranchWave))
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
        if (other.gameObject.CompareTag("Branch"))
        {
            isNearBranch = true;
            branchObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Branch"))
        {
            isNearBranch = false;
            branchObject = null;
        }
    }

    private void PickUp()
    {
        // isHoldingBranch = true;
        holdingNumber++;
        Destroy(branchObject);
        if (weaponSwitcher.weapon_in_hand == 0)
        {
            weaponSwitcher.weapon_in_hand = 2;
        }
        UpdateNumberText(holdingNumber);
        branchUIimage.gameObject.SetActive(true);

        FirstLevelTutorial.hasPickedUpBranch = true;
        // TODO: add code to hold the stone
    }

    private void DropDown()
    {
        // isHoldingBranch = false;
        holdingNumber--;
        UpdateNumberText(holdingNumber);
        GameObject newBranch = Instantiate(branchPrefab, transform.position, Quaternion.identity);
        if (holdingNumber == 0)
        {
            branchUIimage.gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
        }

        GameObject bonfireObject = GameObject.Find("Player");
        BonfireInteractions bonfireInteractions = bonfireObject.GetComponent<BonfireInteractions>();
        bonfireInteractions.buildBonfire();
    }



    private void WaveBranch()
    {
        waveSound.volume = Mathf.Lerp(0.6f, 1f, timeHeld / maxHoldTime);
        waveSound.pitch = Mathf.Lerp(4f, 1f, timeHeld / maxHoldTime);
        waveSound.Play();
        Animator animator = GetComponent<Animator>();
        //float horizontal = animator.GetFloat("Horizontal");
        //float vertical = animator.GetFloat("Vertical");

        //calculate the angle to determine the facing direction
        //01234567 from right counterclockwise
        float angle = Mathf.Atan2(animator.GetFloat("Vertical"), animator.GetFloat("Horizontal")) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;
        int facingDirection = Mathf.RoundToInt(angle / 45.0f) % 8;

        int layerMask = LayerMask.GetMask("Ignore Raycast");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + GetBranchOffset(facingDirection), branchAttackRange, layerMask);
        float area = 1 + 0.5f * timeHeld / maxHoldTime;
        branchEffect.gameObject.transform.localScale = new Vector3(area, area);
        /*        Instantiate(branchEffect,transform);*/


        GameObject effect = Instantiate(branchEffect, transform.position + BranchEffectPosition(facingDirection), Quaternion.identity);
        effect.transform.parent = null;


        // get the effect's Transform component and set its rotation based on the facing direction
        Transform effectTransform = effect.GetComponent<Transform>();
        /*        Quaternion rotation = BranchEffectRotation(facingDirection);*/
        Quaternion rotation = BranchEffectRotation(facingDirection);
        effectTransform.rotation = rotation;

        foreach (Collider2D collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
            if (collider.gameObject.tag == "Enemy")
            {
                float fill = timeHeld / maxHoldTime * maxPowerBarValue;
                collider.transform.parent.GetComponent<EnemyHealth>().TakeDamage(Mathf.RoundToInt(50 * fill));
            }
            else if(collider.gameObject.name.StartsWith("frog_spit_bullet"))
            {
                Debug.Log("collide with: " + collider.gameObject.name);
                Destroy(collider.gameObject);
            }
            // monkey expelliarmus
            if(collider.gameObject.name == "monkeyBody" && timeHeld / maxHoldTime == 1){
                collider.transform.parent.GetComponent<MonkeyMovement>().Expelliarmus();
            }
        }
    }


    private Vector3 GetBranchOffset(int direction)
    {
        switch (direction)
        {
            case 0: return new Vector3(0.6f, 0.6f, 0); // Facing right
            case 1: return new Vector3(0.5f, 1.1f, 0); // Facing up-right
            case 2: return new Vector3(0, 1.4f, 0); // Facing up
            case 3: return new Vector3(-1f, 1.1f, 0); // Facing up-left
            case 4: return new Vector3(-0.6f, 0.6f, 0); // Facing left
            case 5: return new Vector3(-0.5f, 0.3f, 0); // Facing down-left
            case 6: return new Vector3(0, 0.2f, 0); // Facing down
            case 7: return new Vector3(0.5f, 0.3f, 0); // Facing down-right
            default: return Vector3.zero;
        }
    }
    //public float a = 0f;
    //public float b = -.6f;
    private Vector3 BranchEffectPosition(int direction)
    {
        switch (direction)
        {
            case 0: return new Vector3(0.8f, 0.5f, 0f); // Facing right
            case 1: return new Vector3(0.7f, 1f, 0); // Facing up-right
            case 2: return new Vector3(0, 1.4f, 0); // Facing up
            case 3: return new Vector3(-1f, 1f, 0); // Facing up-left
            case 4: return new Vector3(-1f, 0.3f, 0); // Facing left
            case 5: return new Vector3(-1f, 0f, 0); // Facing down-left
            case 6: return new Vector3(-.2f, -.2f, 0); // Facing down
            case 7: return new Vector3(0.65f, 0, 0); // Facing down-right
            default: return Vector3.zero;
        }
    }



    private Quaternion BranchEffectRotation(int direction)
    {
        switch (direction)
        {
            case 0: return Quaternion.Euler(0, 0, 180f);// Facing right
            case 1: return Quaternion.Euler(0, 0, 220f); // Facing up-right
            case 2: return Quaternion.Euler(0, 0, 0f); // Facing up
            case 3: return Quaternion.Euler(0, 0, 0f); // Facing up-left
            case 4: return Quaternion.Euler(0, 0, 0f); // Facing left
            case 5: return Quaternion.Euler(0, 0, 20f); // Facing down-left
            case 6: return Quaternion.Euler(0, 0, 180f); // Facing down
            case 7: return Quaternion.Euler(0, 0, 180f); // Facing down-right
            default: return Quaternion.Euler(0, 0, 180f);
        }
    }



    public void UpdateNumberText(int number)
    {
        if (holdingNumber != 0)
            branchNoText.text = holdingNumber.ToString() + "/" + maxHoldingNumber.ToString();
        else
        {
            branchNoText.text = "";
            weaponSwitcher.weapon_in_hand = 0;
        }
        weaponSwitcher.HighlightInUI(weaponSwitcher.weapon_in_hand);
    }




    private IEnumerator attackInterval()
    {
        yield return new WaitForSeconds(attackIntervalTime);

        canAttack = true;
    }
}



