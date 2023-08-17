using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8f;
    [SerializeField]
    private float exceleratedSpeed = 10f;

    private float jumpPower = 20f;
    [SerializeField]
    private Transform groundCheck;
    private float groundCheckRange = 1f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask enemyLayer;
    private Rigidbody2D playerRigidbody;
    private float x;
    [SerializeField]
    private SpriteRenderer playerSprites;
    private Dash dash;

    private Hang hang;

    // Start is called before the first frame update
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        dash = GetComponent<Dash>();
        hang = GetComponent<Hang>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (!dash.isDashing && !hang.isHanging) // �뽬�� Ȥ�� ���� ��� �� �ƴϸ� ��������
        {
            SetHeading();
            if (IsGrounded() && Input.GetButtonDown("Jump"))
                Jump();
        }

        if ( Input.GetKeyDown( KeyCode.UpArrow ) )
        {
            GameManager.I.ChangeSceneToLobby();
        }

    }

    private void FixedUpdate()
    {
        if (!dash.isDashing) // �뽬 �ƴϸ� �̵�
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position += new Vector3(x, 0f, 0f) * Time.deltaTime * speed;
    }


    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, enemyLayer);

    }

    private void SetHeading()
    {
        x = Input.GetAxis("Horizontal");

        if (x > 0)
            playerSprites.flipX = false;
        else if (x < 0)
            playerSprites.flipX = true;
    }

    public void Jump()
    {
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpPower);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            playerRigidbody.velocity = Vector2.zero;
            gameObject.transform.SetParent(collision.gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            gameObject.transform.SetParent(null);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "endingDoor1")
        {
            UnityEngine.Debug.Log("endingDoor1 Enter");
        }   
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.name == "endingDoor1")
        {
            UnityEngine.Debug.Log("endingDoor1 Exit");
        }   
    }
}
