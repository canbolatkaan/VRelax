using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    private TextMeshPro emailTextOutput;
    bool caps_on = false;
    // Start is called before the first frame update
    void Start()
    {
        emailTextOutput = GameObject.FindGameObjectWithTag("Email").GetComponentInChildren<TextMeshPro>(); // must be fixed. it gives error know.
    }

    private void OnTriggerEnter(Collider other)
    {
        var key = other.GetComponentInChildren<TextMeshPro>();

        if(key != null)
        {
            var keyFeedback = other.gameObject.GetComponent<KeyFeedback>();
            if (keyFeedback.keyCanBeHitAgain)
            {
                if (key.text == "SPACE") // must be fixed when we are typing in email/password field.cannot contain space!!!
                    emailTextOutput.text += " ";
                else if (key.text == "DEL")
                {
                    int len = emailTextOutput.text.Length;
                    emailTextOutput.text = emailTextOutput.text.Substring(0, len - 1);
                }
                else if (key.text == "caps_on LK")
                    caps_on = !caps_on;
                
                else
                {
                    if(caps_on)
                        emailTextOutput.text += key.text.ToUpper();
                    else
                        emailTextOutput.text += key.text.ToLower();
                }
                    

                keyFeedback.keyHit = true;
            }
        }
            
    }
}
