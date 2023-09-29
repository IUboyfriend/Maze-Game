using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider2D boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.bounds.Intersects(boxCollider.bounds))
        {
            other.gameObject.GetComponent<CharacterStatus>().takeDamage(20);
        }
    }


}
