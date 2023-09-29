using UnityEngine;

public class Cursor : MonoBehaviour
{
    AudioSource clip;
    private float time;
    private float short_or_long=0.2f;
    //private bool clicked=false;

    private void Start()
    {
        clip = GetComponent<AudioSource>();
        time = 0;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            time += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (time < short_or_long)
            {
                clip.Play();
            }
            time = 0;
        }
    }
}
