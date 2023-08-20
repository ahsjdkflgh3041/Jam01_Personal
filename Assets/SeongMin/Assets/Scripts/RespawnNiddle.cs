using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNiddle : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float spawnDuration;
    [SerializeField]
    private bool isRandomDuration;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("Cycle");
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            if (isRandomDuration)
            {
                spawnDuration = Random.Range(2f, 5f);
            }
            transform.position = spawnPoint.position;
            yield return new WaitForSeconds(spawnDuration);
            rb.velocity = Vector2.zero;
        }
    }
}
