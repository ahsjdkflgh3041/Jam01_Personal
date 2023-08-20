using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField]
    private int collectionNumber;

    [SerializeField]
    private JungleGameManager gameManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.AddCollection(collectionNumber);
            gameObject.SetActive(false);
            gameManager.PlayDialogue(0);
        }
    }
}