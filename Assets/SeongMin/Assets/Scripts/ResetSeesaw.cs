using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSeesaw : MonoBehaviour, IResetable
{

    private void Start()
    {
    }

    public void Reset()
    {
        Debug.Log("Seesaw");
        transform.eulerAngles = Vector3.zero;

        Moving moving = GetComponent<Moving>();
        if(moving != null)
            moving.Reset();
    }
}
