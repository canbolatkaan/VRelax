using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonScript : MonoBehaviour
{
    public void goScene1(){
        //Debug.Log("button clicked");
        SceneManager.LoadScene("Scene1");        
    }
    public void goScene2(){
        //Debug.Log("button clicked");
        SceneManager.LoadScene("Scene2");        
    }
}
