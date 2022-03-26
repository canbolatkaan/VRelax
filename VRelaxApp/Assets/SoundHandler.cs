using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public AudioClip keyClick;
    public AudioSource clickSource;

    // Start is called before the first frame update
    void Start()
    {
        clickSource = gameObject.AddComponent <AudioSource>();
    }

    public void PlayKeyClick()
    {
        clickSource.PlayOneShot(keyClick);
    }
}
