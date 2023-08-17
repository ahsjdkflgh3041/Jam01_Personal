using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using TMPro;
using System.IO;
using System.Linq;
using System.Data;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text talkerTxt;
    public TMP_Text contentTxt;
    public GameObject dialogueWindow;

    public TMP_Text abiTxt;
    public TMP_Text abilcontentTxt;
    public GameObject abilityWindow;


    TextAsset        diaData;
    DialogueSequence diaSequence;
    List<Dialogue>   diaSorted;

    List<string>     diaCompleted;
    bool playing = false;


    void Start()
    {
        diaCompleted = new List<string>();
    }


    public void LoadSrc()
    {
        string filePath = Path.Combine("Dialogues", "dialogFile");
        diaData     = Resources.Load<TextAsset>(filePath);
        diaSequence = JsonConvert.DeserializeObject<DialogueSequence>(diaData.text);
    }


    public void EnableWindow()
    {
        if( !dialogueWindow.activeSelf ){
            dialogueWindow.SetActive(true);
        }
    }

    public void DisableWindow()
    {
        if( dialogueWindow.activeSelf ){
            dialogueWindow.SetActive(false);
        }
    }


    public void PlayDialogue(string eventPoint_)
    {
        if(playing){ return; }

        EnableWindow();
        diaSorted = diaSequence.dialogues.Where(item => item.eventPoint == eventPoint_).ToList();
        StartCoroutine( PlayDialogueCo(diaSorted) );
        diaCompleted.Add(eventPoint_);

    }


    IEnumerator PlayDialogueCo(List<Dialogue> diaSorted )
    {
        playing = true;

        foreach (Dialogue dialogue in diaSorted)
        {
            
            Debug.Log(dialogue.characterName + " : " + dialogue.dialogueText);

            talkerTxt.text  = dialogue.characterName;
            contentTxt.text = dialogue.dialogueText;


            // TODO: key restrict = true;
            if( dialogue.lineTimeout == 0 )
            {
                
                yield return new WaitUntil(() => Input.GetKeyDown( KeyCode.Return ));
                yield return new WaitForEndOfFrame(); // 키 중복입력 방지
            
            }
            else if( dialogue.lineTimeout > 0 ){
                yield return new WaitForSeconds(dialogue.lineTimeout);
            }
            // TODO: key restrict = false;

        }

        playing = false;
        DisableWindow();
    }



    public bool CheckComplete(string eventPoint_)
    {

        foreach(string eventPoint in diaCompleted)
        {
            if( eventPoint == eventPoint_){
                return true;
            }

        }


        return false;
    }
}


[System.Serializable]
public class Dialogue
{
    public string eventPoint;
    public string characterName;
    public string dialogueText;
    public float  lineTimeout;
}

[System.Serializable]
public class DialogueSequence
{
    public List<Dialogue> dialogues;
}

