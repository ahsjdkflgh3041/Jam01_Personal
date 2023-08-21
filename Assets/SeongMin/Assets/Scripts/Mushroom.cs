using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField]
    private float force = 25f;
    private Rigidbody2D touchedRigidbody;

    private float rotation;

    [SerializeField]
    private AudioSource boing;

    private bool hasCollided = false;

    private void Start()
    {
        rotation = transform.eulerAngles.z;
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
                float angleToRad = (rotation + 90) * Mathf.Deg2Rad;
                touchedRigidbody.AddForce(new Vector2(Mathf.Cos(angleToRad), Mathf.Sin(angleToRad)) * force, ForceMode2D.Impulse);
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
