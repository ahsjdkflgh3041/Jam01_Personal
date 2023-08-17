using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton_mj<GameManager>
{

    public DialogueManager dialogueMan;

    public List<GameObject> doors = new List<GameObject>();
    

    int doorEntered = 0;

    void Start()
    {
        dialogueMan.LoadSrc(); 
    }

    void Update()
    {
        // Camera Shaking
        if ( Input.GetKeyDown( KeyCode.S ) )
        {
            CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
            if (cameraShake != null)
            {
                cameraShake.TriggerShake();
            }
        }


        // Dialogue
        if ( Input.GetKeyDown( KeyCode.D ) ) // Dialogue Trigger
        {
            dialogueMan?.PlayDialogue("1_1clear");
            // dialogueMan?.PlayDialogue("1_2clear");
            // dialogueMan?.PlayDialogue("2_1clear"); // 자동종료
            // dialogueMan?.PlayDialogue("2_2clear");
            // dialogueMan?.PlayDialogue("ending");
            
        }
        if ( Input.GetKeyDown( KeyCode.F ) )
        {
            dialogueMan?.PlayDialogue("2_1clear"); // 몇초 후 자동종료
        }
    }


    public void ChangeSceneToStage(int doorAhead_)
    {
        Debug.Log(" Load Scene to Stage " + doorAhead_);

        switch (doorAhead_)
        {
            case 1: 
                doorEntered = 1;
                SceneManager.LoadScene("Stage1_minjae");
                //SceneManager.LoadScene("Stage1");
            break;
            case 2: 
                doorEntered = 2;
                SceneManager.LoadScene("Stage2");
            break;
            case 3: 
                doorEntered = 3;
                SceneManager.LoadScene("Stage3");
            break;
            default: 
            break;
        }
    }

    public void ChangeSceneToLobby()
    {
        Debug.Log(" Load Scene to Lobby ");

        SceneManager.LoadScene("LobbyScene");
    }


    public void SceneChanged()
    {
        Debug.Log( "SceneChanged" );

        switch (SceneManager.GetActiveScene().name)
        {
            case "LobbyScene":
                SetPlayerPosOnDoor();
                break;

            default:
            break;
        }
    }

    void SetPlayerPosOnDoor()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if(player == null){ Debug.Log("player is null"); return; }

        switch (doorEntered)
        {
            case 1:
                player.transform.position = doors[0].transform.position;
                break;
            case 2:
                player.transform.position = doors[1].transform.position;
                break;
            case 3:
                player.transform.position = doors[2].transform.position;
                break;

            default:
            break;
        }  
    }
    

}
