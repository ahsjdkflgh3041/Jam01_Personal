using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField]
    private GameObject text;

    private bool isEndCutScene;

    public void EndCutScene()
    {
        text.SetActive(true);
        isEndCutScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEndCutScene && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
