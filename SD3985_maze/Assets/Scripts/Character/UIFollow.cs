using UnityEngine;
using System.Collections;

public class UIFollow : MonoBehaviour
{

    private Transform target; 

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        Vector3 targetPos = Camera.main.WorldToScreenPoint(target.position);
        transform.position = new Vector3(targetPos.x, targetPos.y + 150, 0);
    }
}
