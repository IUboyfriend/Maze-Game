using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpit : MonoBehaviour
{
    void Start()
    {
        // Destroy the game object after 1 second
        Invoke("DestroyGameObject", 0.5f);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
