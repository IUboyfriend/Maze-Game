using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImusicController : MonoBehaviour
{
    public AudioSource music_player;
    public AudioClip uimusic;

    private bool clicked = false;
    //private float audiotime=;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        music_player.clip = uimusic;
    }
    IEnumerator playClip()
    {
        music_player.time = 0f;
        music_player.Play();
        yield return new WaitForSeconds(0.3f);
        music_player.Stop();
    }
    IEnumerator playClip_2()
    {
        music_player.time = 0.5f;
        music_player.Play();
        yield return new WaitForSeconds(0.3f);
        music_player.Stop();
    }
    private void OnClick()
    {
        if (!clicked)
        {
            StartCoroutine("playClip");
            clicked = !clicked;
        }
        else
        {
            StartCoroutine("playClip_2");
            clicked = !clicked;
        }
    }
}
