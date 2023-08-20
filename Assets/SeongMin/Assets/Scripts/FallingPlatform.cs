using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour, IResetable
{
    private Rigidbody2D platformRigidbody;
    private Vector2 initialPosition;

    private void Start()
    {
        platformRigidbody = GetComponent<Rigidbody2D>();
        platformRigidbody.gravityScale = 0f;
        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            platformRigidbody.gravityScale = 1f;
        }
    }

    public void Reset()
    {
        platformRigidbody.gravityScale = 0f;
        platformRigidbody.velocity = Vector2.zero;
        transform.position = initialPosition;
    }
}
