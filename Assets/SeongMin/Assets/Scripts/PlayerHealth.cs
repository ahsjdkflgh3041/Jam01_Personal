using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Dash dash;

    // Start is called before the first frame update
    void Start()
    {
        dash = GetComponent<Dash>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!dash.isDashing)//대쉬중일경우 적 처치 대쉬 아닐 시 죽음
            {
                Debug.Log("Dead!");
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Niddle"))
        {
            Debug.Log("Dead!");
        }
        if (collision.gameObject.CompareTag("Crash"))
        {
            if (dash.isDashing)//대쉬중일경우 벽 부수기
            {
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);

            }
        }
    }
	private void OnCollisionStay2D(Collision2D collision)
	{
        /*
        if (collision.gameObject.CompareTag("Crash"))
        {
            if (dash.isDashing)//대쉬중일경우 벽 부수기
            {
                //Destroy(collision.gameObject);
                collision.gameObject.SetActive(false);
                
            }
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Dead!");
        }
        else if (collision.gameObject.CompareTag("Niddle"))
        {
            Debug.Log("Dead!");
        }
    }
}
