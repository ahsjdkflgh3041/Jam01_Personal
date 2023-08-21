using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterEnding : MonoBehaviour
{
    [SerializeField]
    private JungleGameManager gameManager;
    [SerializeField]
    private JunglePlayerController playerController;
    private Animation endingAnimation;

    private void Awake ()
    {
        endingAnimation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        endingAnimation.Play();
    }


    private void Respawn()
    {
        gameManager.RespawnAfterBadEnding();
        playerController.RespawnAfterBadEnding();
    }
}
