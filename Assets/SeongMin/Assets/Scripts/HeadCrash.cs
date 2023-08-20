using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCrash : MonoBehaviour
{
    [SerializeField]
    private JunglePlayerController playerController;

    private Collider2D chargeCollider;

    private void Start()
    {
        chargeCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        chargeCollider.enabled = playerController.isDashing;

        transform.localScale = new Vector3(playerController.faceRight ? 1f : -1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crash"))
        {
            if (playerController.isDashing)
            {
                //TODO: Restart�� ������Ʈ ���� ���� �� ����Ʈ�� ��ƾ���
                collision.gameObject.SetActive(false);
            }
        }
    }
}
