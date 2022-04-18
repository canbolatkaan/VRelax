using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDetector : MonoBehaviour
{
    static bool loginFlag=false, registerFlag=false;
    [SerializeField] private GameObject canvas;
    GameObject login, register;
    GameObject emailArea, userNameArea, passwordArea, confirmPasswordArea;
    private static TextMeshPro usernameTextOutput;
    private static TextMeshPro emailTextOutput;
    private static TextMeshPro hiddenPassword, passwordTextOutput;
    private static TextMeshPro hiddenPasswordc, passwordTextOutputc;
    static bool caps_on = false;
    static int entryField = 0;
    public enum Field_Type
    {
        USERNAME, EMAIL, PASSWD, HIDDENPSWD, PASSWDC, HIDDENPSWDC
    }

    private void Awake()
    {
        login = canvas.transform.Find("Login UI").gameObject;
        register = canvas.transform.Find("Register UI").gameObject;
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (login.activeSelf)
        {
            loginFlag = true;
            registerFlag = false;
            emailArea = GetChildObject(login.transform, "Email");
            emailTextOutput = emailArea.GetComponentInChildren<TextMeshPro>();

            passwordArea = GetChildObject(login.transform, "Password");
            hiddenPassword = passwordArea.GetComponentsInChildren<TextMeshPro>()[0];
            passwordTextOutput = passwordArea.GetComponentsInChildren<TextMeshPro>()[1];

        }

        else if (register.activeSelf)
        {
            loginFlag = false;
            registerFlag = true;
            userNameArea = GetChildObject(register.transform, "Username");
            usernameTextOutput = userNameArea.GetComponentInChildren<TextMeshPro>();

            emailArea = GetChildObject(register.transform, "Email");
            emailTextOutput = emailArea.GetComponentInChildren<TextMeshPro>();

            passwordArea = GetChildObject(register.transform, "Password");
            hiddenPassword = passwordArea.GetComponentsInChildren<TextMeshPro>()[0];
            passwordTextOutput = passwordArea.GetComponentsInChildren<TextMeshPro>()[1];

            confirmPasswordArea = GetChildObject(register.transform, "ConfirmPassword");
            hiddenPasswordc = confirmPasswordArea.GetComponentsInChildren<TextMeshPro>()[0];
            passwordTextOutputc = confirmPasswordArea.GetComponentsInChildren<TextMeshPro>()[1];

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
                if (key.text == "UP")
                    entryField--;

                else if (key.text == "ENTER")
                    entryField++;

                else if (key.text == "Login")
                {
                    entryField = 0;
                    FirebaseManager.instance.LoginButton();
                }

                else if (key.text == "Register")
                {
                    key.text = "SignUp";
                    entryField = 0;
                    AuthUIManager.instance.RegisterScreen();
                    UpdateUI();

                }
                else if (key.text == "BACK")
                {
                    if (registerFlag)
                    {
                        entryField = 0;
                        AuthUIManager.instance.LoginScreen();
                        UpdateUI();
                    }

                }
                else if (key.text == "SignUp")
                {
                    entryField = 0;
                    FirebaseManager.instance.RegisterButton();
                }
                else if (key.text == "SPACE")
                {
                    //PASS
                }
                else if (key.text == "CAPS LK")
                    caps_on = !caps_on;

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
                            case 3:
                                {
                                    int len = passwordTextOutputc.text.Length;
                                    passwordTextOutputc.text = passwordTextOutputc.text.Substring(0, len - 1);
                                    hiddenPasswordc.text = hiddenPasswordc.text.Substring(0, len - 1);
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
                else // ALL THE OTHER CHARACTERS
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
                            case 3:
                                {
                                    enterFields(key.text, Field_Type.HIDDENPSWDC);
                                    enterFields("*", Field_Type.PASSWDC);
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
                usernameTextOutput.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
            case Field_Type.EMAIL:
                emailTextOutput.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
            case Field_Type.PASSWD:
                passwordTextOutput.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
            case Field_Type.HIDDENPSWD:
                hiddenPassword.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
            case Field_Type.PASSWDC:
                passwordTextOutputc.text += caps_on ? input.ToUpper() : input.ToLower();
                break;
            case Field_Type.HIDDENPSWDC:
                hiddenPasswordc.text += caps_on ? input.ToUpper() : input.ToLower();
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
