using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spitBullet : MonoBehaviour
{
    public float speed = 1f;
    public float trackingTime = 2.0f;

    private GameObject player;
    private bool isTracking = true;
    private Vector3 direction;
    private CharacterStatus characterStatus;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        characterStatus = player.GetComponent<CharacterStatus>();
        Invoke("StopTracking", trackingTime);
        Destroy(gameObject, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isTracking)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        transform.position += direction * speed * Time.deltaTime;
    }

    private void StopTracking()
    {
        isTracking = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            // Debug.Log("hit player");
            characterStatus.FrogSpitDamage(2);
            characterStatus.AddPoisoningTime(2f);
        }
    }

}
