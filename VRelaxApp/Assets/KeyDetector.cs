using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    static bool loginFlag=false, registerFlag=false;
    [SerializeField] private GameObject canvas;
    GameObject login, register;
    private TextMeshPro usernameTextOutput;
    private TextMeshPro emailTextOutput;
    private TextMeshPro hiddenPassword, passwordTextOutput;
    bool caps_on = false;
    static int entryField = 0;
    public enum Field_Type
    {
        USERNAME, EMAIL, PASSWD, HIDDENPSWD
    }

    private void Awake()
    {
        login = canvas.transform.Find("Login UI").gameObject;
        register = canvas.transform.Find("Register UI").gameObject;
        if (login.activeSelf)
        {
            loginFlag = true;
            registerFlag = false;
            GameObject emailArea = GetChildObject(canvas.transform, "Email");
            emailTextOutput = emailArea.GetComponentInChildren<TextMeshPro>();
            GameObject passwordArea = GetChildObject(canvas.transform, "Password");
            hiddenPassword = passwordArea.GetComponentsInChildren<TextMeshPro>()[0];
            passwordTextOutput = passwordArea.GetComponentsInChildren<TextMeshPro>()[1];  

        }
    }
    private void UpdateUI()
    {
        if (register.activeSelf)
        {
            loginFlag = false;
            registerFlag = true;
            GameObject userNameArea = GetChildObject(canvas.transform, "Username");
            usernameTextOutput = userNameArea.GetComponentInChildren<TextMeshPro>();
            GameObject emailArea = GetChildObject(canvas.transform, "Email");
            emailTextOutput = emailArea.GetComponentInChildren<TextMeshPro>();
            GameObject passwordArea = GetChildObject(canvas.transform, "Password");
            hiddenPassword = passwordArea.GetComponentsInChildren<TextMeshPro>()[0];
            passwordTextOutput = passwordArea.GetComponentsInChildren<TextMeshPro>()[1];

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var key = other.GetComponentInChildren<TextMeshPro>();

        if(key != null)
        {
            var keyFeedback = other.gameObject.GetComponent<KeyFeedback>();
            

            if (keyFeedback.keyCanBeHitAgain)
            {
                if (key.text == "Login")
                {
                    entryField = 0;
                    FirebaseManager.instance.LoginButton();
                }

                else if (key.text == "Register")
                {
                    entryField = 0;
                    AuthUIManager.instance.RegisterScreen();
                    UpdateUI();

                }
                else if (key.text == "ENTER")
                    entryField++;
                else if (key.text == "SPACE")
                {
                    //PASS
                }
                else if (key.text == "DEL")
                {
                    if (registerFlag)
                        switch (entryField)
                        {
                            case 0:
                                {
                                    int len = usernameTextOutput.text.Length;
                                    usernameTextOutput.text = usernameTextOutput.text.Substring(0, len - 1);
                                    break;
                                }
                            case 1:
                                {
                                    int len = emailTextOutput.text.Length;
                                    emailTextOutput.text = emailTextOutput.text.Substring(0, len - 1);
                                    break;
                                }
                            case 2:
                                {
                                    int len = passwordTextOutput.text.Length;
                                    passwordTextOutput.text = passwordTextOutput.text.Substring(0, len - 1);
                                    hiddenPassword.text = hiddenPassword.text.Substring(0, len - 1);
                                    break;
                                }

                        }
                    else if (loginFlag)
                    {
                        switch (entryField)
                        {
                            case 0:
                                {
                                    int len = emailTextOutput.text.Length;
                                    emailTextOutput.text = emailTextOutput.text.Substring(0, len - 1);
                                    break;
                                }
                            case 1:
                                {
                                    int len = passwordTextOutput.text.Length;
                                    passwordTextOutput.text = passwordTextOutput.text.Substring(0, len - 1);
                                    hiddenPassword.text = hiddenPassword.text.Substring(0, len - 1);
                                    break;
                                }

                        }
                    }
                }
                else
                {
                    if (registerFlag)
                    {
                        switch (entryField)
                        {
                            case 0:
                                {
                                    enterFields(key.text, Field_Type.USERNAME);
                                    break;
                                }
                            case 1:
                                {
                                    enterFields(key.text, Field_Type.EMAIL);
                                    break;
                                }
                            case 2:
                                {
                                    enterFields(key.text, Field_Type.HIDDENPSWD);
                                    enterFields("*", Field_Type.PASSWD);
                                    break;
                                }

                        }
                    }
                        
                    else if (loginFlag)
                    {
                        switch (entryField)
                        {
                            case 0:
                                {
                                    enterFields(key.text, Field_Type.EMAIL);
                                    break;
                                }
                            case 1:
                                {
                                    enterFields(key.text, Field_Type.HIDDENPSWD);
                                    enterFields("*", Field_Type.PASSWD);
                                    break;
                                }
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
            case Field_Type.USERNAME:
                usernameTextOutput.text += input;
                break;
            case Field_Type.EMAIL:
                emailTextOutput.text += input;
                break;
            case Field_Type.PASSWD:
                passwordTextOutput.text += input;
                break;
            case Field_Type.HIDDENPSWD:
                hiddenPassword.text += input;
                break;
        }
    }
    public GameObject GetChildObject(Transform parent, string _tag)
    {
        GameObject result = null;
        Transform child;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (result != null)
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
