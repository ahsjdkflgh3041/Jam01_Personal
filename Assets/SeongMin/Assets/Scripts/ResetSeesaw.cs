using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSeesaw : MonoBehaviour
{
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.eulerAngles = Vector3.zero;
    }
}
