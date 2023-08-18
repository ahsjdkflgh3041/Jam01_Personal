using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Getct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i< collision.contacts.Length; i++)
            Debug.Log(collision.GetContact(i).normal);//왜 특정 오브젝트에서만 2번 나오는가?
    }
}
