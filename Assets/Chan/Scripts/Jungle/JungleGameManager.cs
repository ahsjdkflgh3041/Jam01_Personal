using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JungleGameManager : MonoBehaviour
{
    public DialogueManager dialogueMan;

    public List<GameObject> doors = new List<GameObject>();
    public List<GameObject> resetObjects = new List<GameObject>();

    public List<GameObject> collectionGameObject = new List<GameObject>();

    public JungleCamera JungleCamera;

    public CameraShake CameraShake;

    public GameObject finalEnemy;

    int doorEntered = 0;

    public Collection[] collections = new Collection[4];

    [SerializeField]
    private SignControl signController;

    [SerializeField]
    private JungleDoor hiddenDoor;

    void Start()
    {
        dialogueMan.LoadSrc();
        SetCollections();
        hiddenDoor.isActive = false;
    }

    private void SetCollections()
    {
        for (int i = 0; i < collections.Length; i++)
        {
            collections[i] = new Collection();
            collections[i].collectionObj = collectionGameObject[i];
        }
    }

    private bool CheckCollection()
    {
        foreach (Collection collection in collections)
        {
            if (!collection.isCollected)
            {
                return false;
            }
        }
        return true;
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
            case 0://보석 획득
                dialogueMan?.PlayDialogue("getJewel");
                break;
            default:
                break;
        }
    }

    public void Respawn()
    {
        foreach (var obj in resetObjects)
        {
            var resetTarget = obj.GetComponent<IResetable>();
            if (resetTarget != null)
            {
                resetTarget.Reset();
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

    public void AddCollection(int num)
    {
        collections[num].isCollected = true;
        collections[num].collectionObj.SetActive(true);
        signController.SetSign(num);
        hiddenDoor.isActive = CheckCollection();
    }
}

public class Collection
{
    public bool isCollected = false;
    public GameObject collectionObj;
}
