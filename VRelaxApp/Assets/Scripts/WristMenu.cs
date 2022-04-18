using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class WristMenu : MonoBehaviour
{
    public GameObject wristUI;
    public bool activeWristUI = true ;
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        DisplayWristUI();
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();

        }
        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
    public void GoToLobby()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToApartment()
    {
        SceneManager.LoadScene(2);
    }
    public void GoToAnonimSohbetler()
    {
        SceneManager.LoadScene(3);
    }
    public void GoToCoffeShop()
    {
        SceneManager.LoadScene(4);
    }
    public void GoToFitness()
    {
        SceneManager.LoadScene(5);
    }
    public void GoToHikingCanyon()
    {
        SceneManager.LoadScene(6);
    }
    public void GoKovboySalonu()
    {
        SceneManager.LoadScene(7);
    }
    public void GoLowPoly()
    {
        SceneManager.LoadScene(8);
    }
    public void GoLowPolyKoy()
    {
        SceneManager.LoadScene(9);
    }
    public void GoOrmanYuruyusu()
    {
        SceneManager.LoadScene(11);
    }
    public void GoSciFi_Bowling()
    {
        SceneManager.LoadScene(10);
    }
    public void GoYabaniOrman()
    {
        SceneManager.LoadScene(12);
    }
    public void GoClinic()
    {
        SceneManager.LoadScene(14);
    }
    public void GoOyunAlanı()
    {
        SceneManager.LoadScene(15);
    }
    public void GoAppointment()
    {
        SceneManager.LoadScene(16);
    }

    public void MenuPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DisplayWristUI();
        }
    }
    public void DisplayWristUI()
    {
        if (activeWristUI)
        {
            wristUI.SetActive(false);
            activeWristUI = false;
        }
        else if (!activeWristUI)
        {
            wristUI.SetActive(true);
            activeWristUI = true;
        }
    }
}
