using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToadAttack : MonoBehaviour
{
    public GameObject spitInstance;
    private bool isSpitting = false;
    private GameObject player;
    private Animator anim;
    public GameObject venomResidue;

    // Start is called before the first frame update
    void Start()
    {
        //spitInstance = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/frog_spit_venom.prefab");
        //venomResidue = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/venom_residue.prefab");
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        // Instantiate(venomResidue, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    

    void OnTriggerStay2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !isSpitting)
        {
            StartCoroutine(ToadSpitRoutine());
        }
        // Debug.Log("ifspitting: " + isSpitting);
    }

    private IEnumerator ToadSpitRoutine()
    {
        isSpitting = true;

        anim.SetFloat("Look X", player.transform.position.x - transform.position.x);
        //the middle point betwenn player and toad
        Vector3 middlePoint = (transform.position + player.transform.position) / 2;
        // The direction from the toad to the player
        Vector3 direction = transform.position - player.transform.position;
        // Rotate the spitInstance to point in the direction of the player
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        // Debug.Log("rotation: " + rotation);
        // Scale the spitInstance based on the distance between the toad and the player
        float distance = Vector3.Distance(transform.position, player.transform.position) * 0.2f;
        Vector3 scale = new Vector3(1f, distance, 1f);
        // Debug.Log("scale: " + scale);
        GameObject prefabInstance = Instantiate(spitInstance, middlePoint, rotation);
        prefabInstance.transform.localScale = scale;

        Vector3 currentPlayerPosition = player.transform.position;
        yield return new WaitForSeconds(0.5f);

        Instantiate(venomResidue, currentPlayerPosition, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        isSpitting = false;
    }
}
