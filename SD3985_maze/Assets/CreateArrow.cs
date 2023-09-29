using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateArrow : MonoBehaviour
{
    public GameObject words;
    public GameObject arrow;
    public Transform hintword_position;
    public Transform arrow_position;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Level1")
              StartCoroutine(arrowI());
    }
    IEnumerator arrowI()
    {

        words.GetComponent<TextMeshProUGUI>().text = "Click the map to enlarge!";
        GameObject g = Instantiate(words, gameObject.transform);
        yield return new WaitForSeconds(3f);
        Destroy(g);
        words.GetComponent<TextMeshProUGUI>().text = "See item introduction here";
        GameObject g1 = Instantiate(words, hintword_position);
        GameObject g2 = Instantiate(arrow, arrow_position);
        yield return new WaitForSeconds(3f);
        Destroy(g1);
        Destroy(g2);



    }
}
