using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleCheckPoint : MonoBehaviour
{
    public JunglePlayerController Player;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    //public EnrollGameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.RespawnPoint = transform.position;
            //gameManager.SaveScore();
            spriteRenderer.color = Color.green;
            this.enabled = false;
        }
    }
}