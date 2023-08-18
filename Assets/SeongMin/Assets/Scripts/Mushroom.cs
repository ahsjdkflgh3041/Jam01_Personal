using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private float force = 25f;
    private Rigidbody2D touchedRigidbody;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private AudioSource boing;

    private bool hasCollided = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f - (force / 50), 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D touchedRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (touchedRigidbody != null)
            {
                touchedRigidbody.velocity = new Vector2(0f, force);
                boing.Play();
            }
        }

        hasCollided = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hasCollided = false;
        }
    }
}
