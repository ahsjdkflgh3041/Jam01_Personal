using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Dash dash;
    [SerializeField]
    private GameObject finalEnemy;

    private void Start()
    {
        dash = GetComponent<Dash>();
        dash.enabled = false;
    }
    void Update()
    {
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("DashPowerUp"))
        {
            Destroy(collision.gameObject);
            finalEnemy.SetActive(true);
            ActiveDash();
        }
	}

	public void ActiveDash() 
    {
        dash.enabled = true;
    }
}
