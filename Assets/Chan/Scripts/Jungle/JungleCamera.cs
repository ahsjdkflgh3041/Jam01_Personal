using UnityEngine;

[RequireComponent(typeof(Camera))]
public class JungleCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField, Range(40f, 180f)]
    private float FOV = 60f;
    
    [SerializeField]
    public Transform focus = default;

    [SerializeField, Range(1f, 20f)]
    float distance = 5f;

    [SerializeField, Min(0f)]
    float focusRadius = 1f;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    Vector2 focusPoint;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        focusPoint = focus.position;
    }

    void LateUpdate()
    {
        UpdateFieldOfView();
        UpdateFocusPoint();
        Vector2 lookDirection = transform.forward;
        transform.localPosition = focusPoint - lookDirection * distance;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    void UpdateFieldOfView()
    {
        mainCamera.fieldOfView = FOV;
    }

    void UpdateFocusPoint()
    {
        Vector2 targetPoint = focus.position;
        if (focusRadius > 0f)
        {
            float distance = Vector2.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector2.Lerp(targetPoint, focusPoint, t);
        }
    }
}