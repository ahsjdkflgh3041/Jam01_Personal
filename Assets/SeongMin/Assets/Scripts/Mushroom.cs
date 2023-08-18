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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 1f - (force / 50), 0f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchedRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (touchedRigidbody != null)
        {
            touchedRigidbody.velocity += new Vector2(0f, force);
            boing.Play();
            touchedRigidbody = null;
        }
    }
}
