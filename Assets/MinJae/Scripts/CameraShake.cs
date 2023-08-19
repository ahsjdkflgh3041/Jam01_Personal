using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public JungleCamera JungleCamera;
    
    public float shakeDuration = 0.1f;    // 쉐이크 지속 시간
    public float shakeMagnitude = 0.2f;   // 쉐이크 강도
    public float dampingSpeed = 1.0f;     // 감쇠 속도

    Vector3 initialPosition;

    private float currentShakeDuration = 0f;

    private void OnEnable()
    {
        initialPosition = transform.position;
        JungleCamera = GetComponent<JungleCamera>();
    }

    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.position = initialPosition;
            JungleCamera.enabled = true;
        }
    }
}