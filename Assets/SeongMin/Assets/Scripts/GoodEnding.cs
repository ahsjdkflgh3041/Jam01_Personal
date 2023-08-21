using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEnding : MonoBehaviour
{
    [SerializeField]
    private JungleGameManager gameManager;

    public void PlayGoodEnding()
    {
        gameManager.LoadGoodEnding();
    }
}
