using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JungleGameManager : MonoBehaviour
{

    public DialogueManager dialogueMan;

    public List<GameObject> doors = new List<GameObject>();
    public List<GameObject> resetObjects = new List<GameObject>();

    public JungleCamera JungleCamera;

    public CameraShake CameraShake;

    public GameObject finalEnemy;

    int doorEntered = 0;

    void Start()
    {
        dialogueMan.LoadSrc(); 
    }

    public void PlayDialogue(int index)
    {
        switch (index)
        {
            case 11:
                dialogueMan?.PlayDialogue("1_1clear");
                break;
            case 12:
                dialogueMan?.PlayDialogue("1_2clear");
                break;
            case 21:
                dialogueMan?.PlayDialogue("2_1clear");
                break;
            case 22:
                dialogueMan?.PlayDialogue("2_2clear");
                break;
            case 3:
                dialogueMan?.PlayDialogue("ending");
                break;
            default:
                break;
        }
    }

    public void Respawn()
    {
        foreach (var obj in resetObjects)
        {
            var ResetDestructive = obj.GetComponent<ResetDestructive>();
            if (ResetDestructive != null)
            {
                ResetDestructive.Reset();
            }
            
            var Fallingplatform = obj.GetComponent<FallingPlatform>();
            if (Fallingplatform != null)
            {
                Fallingplatform.Reset();
            }

            var Movingplatform = obj.GetComponent<Moving>();
            if (Movingplatform != null)
            {
                Movingplatform.ResetPosition();
            }

            var SitMoving = obj.GetComponent<SitMoving>();
            if (SitMoving != null)
            {
                SitMoving.ResetPosition();
            }

            var ResetSeesaw = obj.GetComponent<ResetSeesaw>();
            if (ResetSeesaw != null)
            {
                ResetSeesaw.ResetPosition();
            }
        }
    }

    public void ActiveBoss()
    {
        finalEnemy.SetActive(true);
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
