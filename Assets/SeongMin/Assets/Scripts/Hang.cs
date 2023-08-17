using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hang : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private PlayerMovement playerMovement;

    private bool isColliding = false;
    public bool isHanging = false;

    private Transform vine;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isColliding)
        {
            if (Input.GetAxis("Vertical") > 0)
            {
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.gravityScale = 0f;
                isHanging = true;
                transform.SetParent(vine);
            }
        }
        if (isHanging)
        {
            Move();
            transform.position = new Vector3(vine.position.x, transform.position.y, transform.position.z);
            if (Input.GetButtonDown("Jump"))
            {
                ExitVine();
                playerMovement.Jump();
            }
        }
    }
    private void Move()
    {
        float y = Input.GetAxis("Vertical");
        transform.position += new Vector3(0f, y, 0f) * Time.deltaTime * playerMovement.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Vine"))
        {
            vine = collision.gameObject.transform;
            isColliding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Vine"))
        {
            ExitVine();
        }
    }

    private void ExitVine()
    {
        transform.SetParent(null);
        isColliding = false;
        isHanging = false;
        playerRigidbody.gravityScale = 4f;
    }
}
