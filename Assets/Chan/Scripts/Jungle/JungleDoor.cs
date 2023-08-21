using UnityEngine;

public class JungleDoor : MonoBehaviour
{
    [SerializeField]
    public Transform targetPosition;

    public int fromTo;

    private bool _isActive = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool isActive {
        get 
        {
            return _isActive;
        }
        set
        {
            if (value != _isActive)
            {
                _isActive = value;
                if (value)
                {
                    spriteRenderer.color = Color.black;
                }
                else
                {
                    spriteRenderer.color = Color.red;
                }
            }
        }
    }
}