using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIMenuOption : MonoBehaviour
{

    
    public void ButtonExit()
    {
        Debug.Log("Clicked Exit Button");

        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
