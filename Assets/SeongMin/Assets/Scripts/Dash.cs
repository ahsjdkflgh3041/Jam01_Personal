using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private float x, y;
    private float dashSpeed = 30f;
    private float dashMaintainTime = 0.2f; // �뽬 ���� �ð�
    private float dashCooltime = 0.5f;
    private bool isReady = true;//��Ÿ�� ���µ��� false
    public bool isDashing = false;
    [SerializeField]
    private int countDash = 0; // �뽬 Ƚ�� ����

    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSprites;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSprites = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Dash") && countDash < 1 && isReady)  //�뽬 ������ 
        {
            DashFunction();  // �뽬
            countDash++;  // �ϰ� ī��Ʈ �߰�
        }

        if (playerMovement.IsGrounded())
        {
            countDash = 0;   // ���� �������� �뽬 ī��Ʈ �ʱ�ȭ

        }
    }

    private void DashFunction()
    {
        StartCoroutine(dashCoroutine());
    }

    IEnumerator dashCoroutine()
    {
        isDashing = true;
        isReady = false;
        playerRigidbody.gravityScale = 0f;

        if (x == 0 && y == 0)
        {
            if (playerSprites.flipX)
                playerRigidbody.velocity = new Vector2(-1f, 0f).normalized * dashSpeed;
            else
                playerRigidbody.velocity = new Vector2(1f, 0f).normalized * dashSpeed;
        }
        else
        {
            playerRigidbody.velocity = new Vector2(x, y).normalized * dashSpeed;
        }
        yield return new WaitForSeconds(dashMaintainTime);
        isDashing = false;
        playerRigidbody.velocity = new Vector2(0f, 0f);
        playerRigidbody.gravityScale = 4;
        yield return new WaitForSeconds(dashCooltime);
        isReady = true;
    }
}
