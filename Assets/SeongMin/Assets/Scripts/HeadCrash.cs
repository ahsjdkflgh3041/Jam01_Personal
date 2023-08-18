using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCrash : MonoBehaviour
{
    [SerializeField]
    private JunglePlayerController playerController;

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(playerController.faceRight ? 1f : -1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Crash"))
        {
            if (playerController.isDashing)
            {
                //TODO: Restart시 오브젝트 복구 원할 시 리스트에 담아야함
                collision.gameObject.SetActive(false);
            }
        }
    }
}
