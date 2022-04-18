using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppointmentKeyDetector : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    GameObject patientNameArea, dateArea;
    private static TextMeshPro patientNameTextOutput;
    private static TextMeshPro dateAreaTextOutput; // date format??
    static bool caps_on = false;
    static int entryField = 0;

    public enum Field_Type
    {
        PATIENTNAME, DATE
    }

    private void Awake()
    {
        patientNameArea = GetChildObject(canvas.transform, "PatientName");
        patientNameTextOutput = patientNameArea.GetComponentInChildren<TextMeshPro>();

        dateArea = GetChildObject(canvas.transform, "Date");
        dateAreaTextOutput = dateArea.GetComponentInChildren<TextMeshPro>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var key = other.GetComponentInChildren<TextMeshPro>();

        if(key != null)
        {
            var keyFeedback = other.gameObject.GetComponent<KeyFeedback>();
            

            if (keyFeedback.keyCanBeHitAgain)
            {
                if (key.text == "UP")
                    entryField--;

                else if (key.text == "ENTER")
                    entryField++;

                else if (key.text == "SAVE")
                {
                    if(FirebaseManager.instance != null)
                    {
                        FirebaseManager.instance.saveAppointment(patientNameTextOutput.text, dateAreaTextOutput.text);
                        SceneManager.LoadScene(14);
                    }
                        

                    //Clean ui and show a message!
                }
                else if (key.text == "BACK")
                {
                    // if saved -> return to clinic
                }
                
                else if (key.text == "SPACE")
                {
                    //PASS
                }
                else if (key.text == "CAPS LK")
                    caps_on = !caps_on;

                else if (key.text == "DEL")
                {
                    switch (entryField)
                    {
                        case 0:
                            {
                                int len = patientNameTextOutput.text.Length;
                                patientNameTextOutput.text = patientNameTextOutput.text.Substring(0, len - 1);
                                break;
                            }
                        case 1:
                            {
                                int len = dateAreaTextOutput.text.Length;
                                dateAreaTextOutput.text = dateAreaTextOutput.text.Substring(0, len - 1);
                                break;
                            }
                    }
                    
                }
                else // ALL THE OTHER CHARACTERS
                {
                    switch (entryField)
                    {
                        case 0:
                            {
                                enterFields(key.text, Field_Type.PATIENTNAME);
                                break;
                            }
                        case 1:
                            {
                                enterFields(key.text, Field_Type.DATE);
                                break;
                            }
                    }
                }
                keyFeedback.keyHit = true;
            }
        }
            
    }

    public void enterFields(string input, Field_Type type)
    {
        switch (type)
        {
            case Field_Type.PATIENTNAME:
                patientNameTextOutput.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
            case Field_Type.DATE:
                dateAreaTextOutput.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
        }
    }
    public GameObject GetChildObject(Transform parent, string _tag)
    {
        GameObject result = null;
        Transform child;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (result != null && result.activeSelf)
            {
                return result;
            }
            child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                return child.gameObject;
            }
            if (child.childCount > 0)
            {
                result = GetChildObject(child, _tag);
            }
        }
        return result;
    }

}
