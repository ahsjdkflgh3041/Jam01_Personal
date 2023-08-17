using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom2 : MonoBehaviour
{
    [SerializeField]
    private float force = 25f;
    private Rigidbody2D touchedRigidbody;

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchedRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (touchedRigidbody != null)
        {
            touchedRigidbody.velocity += new Vector2(0f, force);
        }
    }
}
