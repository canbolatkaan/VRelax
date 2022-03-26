using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    private TextMeshPro playerTextOutput;

    // Start is called before the first frame update
    void Start()
    {
        playerTextOutput = GameObject.FindGameObjectWithTag("PlayerTextOutput").GetComponentInChildren<TextMeshPro>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var key = other.GetComponentInChildren<TextMeshPro>();

        if(key != null)
        {
            var keyFeedback = other.gameObject.GetComponent<KeyFeedback>();
            if (keyFeedback.keyCanBeHitAgain)
            {
                if (key.text == "SPACE")
                    playerTextOutput.text += " ";
                else if (key.text == "DEL")
                {
                    int len = playerTextOutput.text.Length;
                    playerTextOutput.text = playerTextOutput.text.Substring(0, len - 1);
                }
                else
                    playerTextOutput.text += key.text;

                keyFeedback.keyHit = true;
            }
        }
            
    }
}
