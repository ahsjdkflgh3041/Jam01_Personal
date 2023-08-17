using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float speedOrigin = 10.0f;
    float speed = speedOrigin;

    int doorAhead = 0;


    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0, 0);
        transform.position = transform.position + new Vector3(movement.x * speed * Time.deltaTime, 0, 0);


        if ( Input.GetKeyDown(KeyCode.UpArrow) )
        {
            Debug.Log("Button Down UpArrow " );

            if(doorAhead != 0)
            {
                GameManager.I.ChangeSceneToStage(doorAhead);
            }

        }

    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Trigger enter " + other.gameObject.name);

        switch(other.gameObject.name)
        {
            case "Door1":
                doorAhead = 1;
            break;
            case "Door2":
                doorAhead = 2;
            break;
            case "Door3":
                doorAhead = 3;
            break;
            // case "Shaman":
            //     if( !GameManager.I.dialogueMan.CheckComplete("start") ){
            //          GameManager.I.dialogueMan.PlayDialogue("start");
            //     }
            //break;
            default:
            break;
        }


    }
    
    private void OnTriggerExit2D(Collider2D other) 
    {
        Debug.Log("Trigger exit " + other.gameObject.name);


        switch(other.gameObject.name)
        {
            case "Shaman":
                if( !GameManager.I.dialogueMan.CheckComplete("start") ){
                     GameManager.I.dialogueMan.PlayDialogue("start");
                }
            break;
            default:
            break;
        }

        doorAhead = 0;
    }
}
