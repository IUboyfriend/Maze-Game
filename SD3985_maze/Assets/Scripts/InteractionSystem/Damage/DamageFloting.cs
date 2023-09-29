using UnityEngine;
using System.Collections;
using TMPro;

public class DamageFloting : MonoBehaviour
{
    float pos_move_to;
    int sign;
    Vector3 pos;
    private void Start()
    {
        pos = new Vector3(0, 0, 0);
        sign = Random.Range(0, 2) * 2 - 1;
        pos_move_to = Random.Range(2, 4);
        StartCoroutine(Move());
    }
    public int abs(int a) { return (a ^ (a >> 31)) - (a >> 31); }

    private IEnumerator Move()
    {
        for (; ; )
        {
            {
                pos.x = Mathf.Lerp(pos.x, pos_move_to * sign, 0.9f);
                transform.position += pos * 0.02f;
            }
            yield return null;
        }
    }
    public void Stop()
    {
        if (transform.parent.parent.parent.name == "BoarBody")
        {
            transform.parent.parent.parent.GetChild(2).gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}
