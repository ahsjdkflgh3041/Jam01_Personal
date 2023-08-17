using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class Moving : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private bool isLoop;

    int index = 0;

    [SerializeField]
    private float movingTime = 2f;
    private float passedTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        passedTime += Time.deltaTime;
        float t = Mathf.Clamp01(passedTime / movingTime);

        transform.position = Vector2.Lerp(points[index % points.Length].position, points[(index + 1) % points.Length].position, t);

        if (t >= 1)
        {
            if (isLoop)
            {
                passedTime = 0;
                index++;
            }
        }
    }

    public void ResetPosition()
    {
        passedTime = 0;
        index = 0;
        transform.position = points[0].position;
    }
}
