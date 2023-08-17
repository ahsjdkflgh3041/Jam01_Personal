using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitMoving : MonoBehaviour
{
    private Moving moving;

    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponent<Moving>();
        moving.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            moving.enabled = true;
        }
    }

    public void ResetPosition()
    {
        moving.enabled = false;
    }
}
