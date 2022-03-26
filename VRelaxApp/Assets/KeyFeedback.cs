using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFeedback : MonoBehaviour
{
    private SoundHandler soundHandler;
    public bool keyHit = false;
    public bool keyCanBeHitAgain = false;
    private float originalYPos;

    // Start is called before the first frame update
    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<SoundHandler>();
        originalYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyHit)
        {
            soundHandler.PlayKeyClick();
            keyHit = false;
            keyCanBeHitAgain = false;
            transform.position += new Vector3(0, -0.3f, 0);
        }

        if (transform.position.y < originalYPos)
            transform.position += new Vector3(0, 0.005f, 0);
        else
            keyCanBeHitAgain = true;
    }
}
