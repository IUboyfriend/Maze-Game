using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPlay : MonoBehaviour
{
    public AudioSource bgm1;
    public AudioSource bgm2;
    public float interval = 15f; // time interval between music tracks
    private bool one_or_two;
    private float leng;
    public float waittime=0;

    void Start()
    {
        StartCoroutine(PlayRandomBGM());
    }

    IEnumerator PlayRandomBGM()
    {
        while (true)
        {
            yield return new WaitForSeconds(waittime);
            // randomly select which BGM to play
            if (Random.Range(0, 2) == 0)
            {
                bgm1.Play();
                one_or_two = true;
            }
            else
            {
                bgm2.Play();
                one_or_two = false;
            }

            // wait for the music to finish playing
            leng = one_or_two ? bgm1.clip.length : bgm2.clip.length; 
            yield return new WaitForSeconds(leng);

            // wait for the specified interval before playing another random BGM
            yield return new WaitForSeconds(interval);
        }
    }
}
