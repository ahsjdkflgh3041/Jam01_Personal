using UnityEngine;

public class Moving : MonoBehaviour, IResetable
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private bool isLoop;
    [SerializeField]
    private bool movingOnTouch;
    [SerializeField]
    private bool moving;

    int index = 0;

    [SerializeField]
    private float movingTime = 2f;
    private float passedTime;

    // Start is called before the first frame update
    void Start()
    {
        moving = !movingOnTouch;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            passedTime += Time.deltaTime;
            float t = Mathf.Clamp01(passedTime / movingTime);

            transform.position = Vector2.Lerp(points[index % points.Length].position, points[(index + 1) % points.Length].position, t);

            if (t >= 1)
            {
                if (isLoop)
                {
                    passedTime = 0f;
                    index++;
                }
            }
        }
    }

    public void Reset()
    {
        Debug.Log(gameObject.name + "Position Reset!");
        moving = !movingOnTouch;
        index = 0;
        passedTime = 0f;
        transform.position = points[0].position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (movingOnTouch && collision.gameObject.CompareTag("Player"))
        {
            moving = true;
        }
    }
}
