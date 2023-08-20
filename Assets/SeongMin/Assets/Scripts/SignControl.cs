using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignControl : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> signs;

    public void SetSign(int num)
    {
        signs[num].SetActive(false);
    }
}
