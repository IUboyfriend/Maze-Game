using UnityEngine;
using System.Collections;


public class StoneController : MonoBehaviour
{
    private bool hasCollided = false;
    private Vector2 velocityBeforeCollision;
    private Rigidbody2D stoneRb ;

    void Start()
    {
       stoneRb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        velocityBeforeCollision = stoneRb.velocity;
        // if(stoneRb.velocity != Vector2.zero)
            // Debug.Log("velocity" + stoneRb.velocity);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // if ((collision.gameObject.CompareTag("Wall") ||  collision.gameObject.CompareTag("Bonfire")) && !hasCollided)
        if ( !hasCollided)
        {
            hasCollided = true;
            Vector2 collisionNormal = collision.contacts[0].normal;
            stoneRb.position -= stoneRb.velocity * Time.fixedDeltaTime;

            stoneRb.velocity = Vector2.Reflect(-velocityBeforeCollision * 0.3f, new Vector2(-collisionNormal.y, collisionNormal.x));
            

            StartCoroutine(SlowDownAfterBounce(stoneRb)); // Start coroutine to gradually reduce the stone's velocity
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy" && !hasCollided && stoneRb.velocity != Vector2.zero)
        {
            Debug.Log("hit enemy");
            //find parent of other
            Debug.Log(other.transform.parent.GetComponent<EnemyHealth>());
            other.transform.parent.GetComponent<EnemyHealth>().TakeDamage(Mathf.RoundToInt(stoneRb.velocity.magnitude));

            hasCollided = true;
            Vector2 collisionNormal = other.transform.position - transform.position;
            stoneRb.velocity = Vector2.Reflect(-stoneRb.velocity * 0.1f, new Vector2(-collisionNormal.y, collisionNormal.x)) ;
            StartCoroutine(SlowDownAfterBounce(stoneRb)); // Start coroutine to gradually reduce the stone's velocity
        }

    }


    IEnumerator SlowDownAfterBounce(Rigidbody2D stoneRb)
    {
        float startTime = Time.time; 
        Vector2 initialVelocity = stoneRb.velocity; 
        float duration = 0.7f; 

        GetComponent<Collider2D>().isTrigger = true;
        while (Time.time - startTime < duration)
        {
            if(Time.time - startTime > 0.1f)
                GetComponent<Collider2D>().isTrigger = false;
            float t = (Time.time - startTime) / duration; 
            stoneRb.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t); 
            yield return null;
        }

        stoneRb.velocity = Vector2.zero; 

        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }


    void OnBecameInvisible()
    {

        stoneRb.velocity = Vector2.zero;

        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }


}


/*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy")) && !hasCollided)
        {
            hasCollided = true;
            Rigidbody2D stoneRb = GetComponent<Rigidbody2D>();
            Vector2 collisionNormal = collision.contacts[0].normal;

            stoneRb.velocity = Vector2.Reflect(stoneRb.velocity, collisionNormal);
            Debug.Log(stoneRb.velocity);
            StartCoroutine(SlowDownAfterBounce(stoneRb));



        }
    }

*/