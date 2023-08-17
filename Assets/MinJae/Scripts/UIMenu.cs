using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{

    public GameObject panelOption;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ButtonStart()
    {
        Debug.Log("Clicked Start Button");
        SceneManager.LoadScene("LobbyScene 1");
    }

    public void ButtonContinue()
    {
        Debug.Log("Clicked Continue Button");
    }

    public void ButtonOption()
    {
        Debug.Log("Clicked Option Button");

        if( panelOption.activeSelf )
        {
            panelOption.SetActive(false);
        }else
        {
            panelOption.SetActive(true);
        }
    }

    public void ButtonEnd()
    {
        Debug.Log("Clicked End Button");
    }
}
