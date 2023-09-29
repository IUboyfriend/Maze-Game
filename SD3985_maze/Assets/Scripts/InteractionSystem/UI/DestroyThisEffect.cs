using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThisEffect : MonoBehaviour
{
    void Destroy()
    {
        Destroy(gameObject);
    }
}
