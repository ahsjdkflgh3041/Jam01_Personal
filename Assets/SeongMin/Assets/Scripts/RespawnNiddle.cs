using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNiddle : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float spawnDuration;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnDuration = Random.Range(2f, 5f);
        StartCoroutine("Cycle");
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            transform.position = spawnPoint.position;
            yield return new WaitForSeconds(spawnDuration);
            rb.velocity = Vector2.zero;
        }
    }
}
