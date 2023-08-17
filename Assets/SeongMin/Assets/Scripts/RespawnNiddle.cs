using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnNiddle : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float spawnDuration;
    // Start is called before the first frame update
    void Start()
    {
        spawnDuration = Random.Range(2f, 5f);
        StartCoroutine("Cycle");
    }

    IEnumerator Cycle()
    {
        transform.position = spawnPoint.position;
        yield return new WaitForSeconds(spawnDuration);
        Instantiate(this, spawnPoint.position, spawnPoint.rotation);
        Destroy(gameObject);
    }
}
