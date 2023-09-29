using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireInteractions : MonoBehaviour
{
    public float bonfireDetectionRadius = 2f;
    public GameObject bonfirePrefab;
    public static int branchesRequired;
    public static int stonesRequired;

    public GameObject branchPrefab;
    public GameObject stonePrefab;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        branchesRequired = 2;
        stonesRequired = 2;
    }
    private void Start()
    {
        branchesRequired = 2;
        stonesRequired = 2;
    }
    // Update is called once per frame
    void Update()
    {

        if(FireGodStatue.couldDismantleBonfire == true && Input.GetKeyDown(KeyCode.Z))
        {
            dismantleBonfire();
        }
    }

    public void buildBonfire()
    {
        int currentAmount = GameObject.FindGameObjectsWithTag("Bonfire").Length - 1;
        if (currentAmount >= ShowBigMap.currentMaxAmount)
            return;

        int numStones = 0;
        int numBranches = 0;

        int layerMask = LayerMask.GetMask("Ignore Raycast");//default layer mask only contains default, if the bonfire is in this layer, we should make the mask for it.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, bonfireDetectionRadius, layerMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Stone"))
            {
                numStones++;
            }
            else if (hitCollider.CompareTag("Branch"))
            {
                numBranches++;
            }
        }
        

        if (numStones >= stonesRequired && numBranches >= branchesRequired)
        {
            Debug.Log("Bonfire built!!");

            Instantiate(bonfirePrefab, transform.position, Quaternion.identity);
            ShowBigMap.currentAmount++;
            if (!FirstLevelTutorial.completed)
            {
                FirstLevelTutorial.bonfire_built = true;
            }
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Stone") || hitCollider.CompareTag("Branch"))
                {
                    hitCollider.gameObject.SetActive(false);
                }
            }

        }
        else
        {
            Debug.Log("Stones" + numStones);
            Debug.Log("Branches" + numBranches);
            Debug.Log("Not enough materials to build a bonfire!");
        }
    }

    public void dismantleBonfire()
    {
        int layerMask = LayerMask.GetMask("Bonfire");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, bonfireDetectionRadius, layerMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Bonfire"))
            {
                Destroy(hitCollider.gameObject);
                Instantiate(branchPrefab, transform.position + new Vector3(0.5f,1f,0f), Quaternion.identity);
                Instantiate(branchPrefab, transform.position + new Vector3(-0.5f, -0.5f, 0f), Quaternion.identity);
                Instantiate(stonePrefab, transform.position + new Vector3(0.5f, -1f, 0f), Quaternion.identity);
                Instantiate(stonePrefab, transform.position + new Vector3(-0.5f, 1f, 0f), Quaternion.identity);
                FireGodStatue.couldDismantleBonfire = false;
            }

        }
    }

}
