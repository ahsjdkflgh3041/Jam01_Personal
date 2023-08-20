using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class JunglePlayerController : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField]
    private List<GameObject> playerModels;

    [SerializeField]
    private GameObject finalEnemy;

    [SerializeField]
    private GameObject endText;

    //0 사람 1 원숭이 2 코뿔소 3 고릴라
    private int _characterLevel;
    private int characterLevel
    {
        get
        {
            return _characterLevel;
        }
        set
        {
            for (int i = 0; i < playerModels.Count; i++)
            {
                if (i == value)
                {
                    playerModels[i].SetActive(true);
                }
                else
                {
                    playerModels[i].SetActive(false);
                }
            }
            _characterLevel = value;
        }
    }


    [Header("RigidBody")]
    [SerializeField]
    private Rigidbody2D rb;

    private Rigidbody2D connectedRb, previousConnectedRb;

    //[SerializeField]
    //private HingeJoint2D hj;

   public bool faceRight;

    Vector2 playerInput;
    public Vector2 RespawnPoint;
[Header("Door")]
    [SerializeField]
    private int doorAhead = 0;

    [SerializeField]
    private RectTransform BS;
    private bool desiredSceneChange;
    private bool onDoor;
    
    
[Header("Physics variables")]
    [SerializeField, Range(0f, 20f)]
    float gravityScaler = 2f;

    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;
    
    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f, maxAirAcceleration = 1f;

    public Vector2 velocity;
    Vector2 desiredVelocity, connectionVelocity, jumpVelocity;

    private Vector2 connectionWorldPosition;
    
[Header("Jump")]
    [SerializeField, Range(0f, 20f)]
    private float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    [SerializeField, Range(1f, 2f)]
    float wallJumpMagnification = 1.2f;

    bool desiredJump;
    int jumpPhase;


    private bool isCollidedMushRoom = false;
    
[Header("Snap")]
    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 75f;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    LayerMask probeMask = -1;
    
    int stepsSinceLastGrounded, stepsSinceLastJump;

    float minGroundDotProduct;

    Vector2 contactNormal, steepNormal;

    int groundContactCount, steepContactCount;

    public bool OnGround => groundContactCount > 0;

    public bool OnSteep => steepContactCount > 0;
    
[Header("Grip")]
    // 유저의 줄타기 입력을 제한함
    [SerializeField]
    private bool restrictGrip = false;
    
    // 줄타기 대상 트랜스폼
    private Transform gripableTransform;
    private int stepsSinceLastDesiredGrip;
    private bool desiredGrip;
    public bool isGriping = false;
    // 줄타기 트리거 위에 있는지 여부
    private bool OnGripable = false;

    /*
[Header("Rope")]
    private Transform gripableRopeTransform;
    private Transform gripableRopeParentTransform;
    private bool desiredGripRope;
    private bool isGripingRope = false;
    private bool OnGripableRope = false;
    */
[Header("Dash")]
    // 대쉬 중 유저의 입력을 제한함
    [SerializeField]
    private bool RestrictInputWhenDash = false;
    
    [SerializeField, Range(0f, 100f)]
    private float dashForce = 40f;
    
    [SerializeField, Range(0f, 3f)]
    private float dashMaintainTime;
    
    [SerializeField, Range(0f, 10f)]
    private float dashCoolDown;
    
    [SerializeField, Range(0, 5)]
    private int maxAirDashes = 1;
    
    private bool desireDash;
    private bool canDash = true;
    public bool isDashing = false;
    private int dashPhase;

 [Header("Steep")]

    //벽에 붙었을때 미끄러지는 속도
    [SerializeField, Range(-10f, 0f)]
    private float minspeed;

    //벽에 붙었을때 미끄러지는 속도에 다다르기까지 걸리는 시간
    [SerializeField, Range(0f, 3f)]
    private float steepTime;

    private float wallElapsedTime = 0f;

[Header("GameManager")]
    [SerializeField]
    private JungleGameManager gameManager;
    private Transform targetPosition;
    private int fromTo;


    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        rb.gravityScale = gravityScaler;
        OnValidate();
        RespawnPoint = transform.position;

        faceRight = true;

        transform.rotation = Quaternion.identity;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //hj.enabled = false;

        characterLevel = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Respawn();
        
        if (!RestrictInputWhenDash)
        {
            playerInput.x = Input.GetAxis("Horizontal");
            playerInput.y = Input.GetAxis("Vertical");
            playerInput = Vector2.ClampMagnitude(playerInput, 1f);

            desiredVelocity =
                new Vector2(playerInput.x, playerInput.y) * maxSpeed;

            desiredJump |= Input.GetButtonDown("Jump");
            
            desireDash |= Input.GetButtonDown("Fire3");

            if (onDoor && Input.GetKeyDown(KeyCode.UpArrow))
            {
                RespawnPoint = targetPosition.position;
                StartCoroutine(ChangeScene());
            }
        }

        if (!restrictGrip)
        {
            if (!isGriping)
            {
                desiredGrip = Input.GetAxis("Vertical") > 0 ? true : false;
            }
        }
        /*
        if (!isGripingRope)
        {
            desiredGripRope = Input.GetAxis("Vertical") > 0 ? true : false;
        }*/
    }

    private void FixedUpdate()
    {
        InputCheatKey();

        UpdateState();

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        if (characterLevel >= 1)
        {
            if (desiredGrip)
            {
                // grip을 성공하였을 때만 false로 전환 -> Grip()에서 처리
                //desiredGrip = false;
                Grip();
            }

            if (isGriping)
            {
                MovePosition();
            }

            if (characterLevel >= 2)
            {
                if (desireDash)
                {
                    desireDash = false;
                    Dash();
                }
            }
        }
        /*
        if (desiredGripRope)
        {
            GripRope();
        }*/

        /*
        if (isGripingRope)
        {
            MovePosition();
        }*/

        if (!isGriping && !isDashing)
        {
            AdjustVelocity();
        }

        if (desiredVelocity.x < 0f)
        {
            faceRight = false;
        }
        else if (desiredVelocity.x > 0f)
        {
            faceRight = true;
        }

        if (OnSteep && characterLevel > 1 && playerInput.x != 0)
        {
            Steep();
        }
        rb.velocity = velocity;
        ClearState();
    }

    private void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterLevel = 2;

        }
    }

    private void Steep()
    {
        wallElapsedTime += Time.deltaTime;
        velocity = new Vector2(velocity.x, Mathf.Lerp(velocity.y, minspeed - 1, Mathf.Clamp01(wallElapsedTime / steepTime)));//벽에 붙으면 천천히 아래로 미끄러지다 멈춤 -1()
    }


    IEnumerator ChangeScene()
    {
        switch (fromTo)
        {
            case 10:
                gameManager.PlayDialogue(12);
                break;
            case 20:
                gameManager.PlayDialogue(22);
                break;
            default:
                break;
        }
        BS.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        transform.position = targetPosition.position;
        yield return new WaitForSeconds(.5f);
        BS.gameObject.SetActive(false);
        Debug.Log(fromTo);
        
        
    }

    private void Respawn()
    {
        ExitGrip();
        transform.position = RespawnPoint;
        gameManager.Respawn();
        velocity = Vector2.zero;
    }

    void Jump()
    {
        if (isCollidedMushRoom)
            return;

        Vector2 jumpDirection;
        if (isGriping)
        {
            ExitGrip();
            jumpDirection = new Vector2(playerInput.x, 0) + Vector2.up;
        }
        else if (OnGround)
        {
            jumpDirection = contactNormal;
        }
        else if (OnSteep && characterLevel >= 1 && playerInput.x != 0)
        {
            jumpDirection = steepNormal;
            jumpPhase = 0;
        }
        // 플랫폼에서 떨어지는 경우
        else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps)
        {
            if (jumpPhase == 0)
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
        }
        /*
        else if (isGripingRope)
        {
            ExitGripRope();
            jumpDirection = new Vector2(playerInput.x, 0) + Vector2.up;
        }*/
        else
        {
            return;
        }

        wallElapsedTime = 0f;//점프 키를 누를 때 마다 벽에 붙어있던 시간을 초기화 시킴

        stepsSinceLastJump = 0;
        jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(2f * -Physics2D.gravity.y * gravityScaler * jumpHeight * ((OnSteep && characterLevel >= 1) ? wallJumpMagnification : 1f));
        jumpDirection = (jumpDirection + Vector2.up).normalized;
        /*
        float alignedSpeed = Vector2.Dot(velocity, jumpDirection);
        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }*/
        
        //velocity += jumpDirection * jumpSpeed;
        jumpVelocity = jumpDirection * jumpSpeed;
        
        // 떨어지는 플랫폼에서 Y+ 방향으로 힘이 부족함, Y 방향 벡터 초기화
        if (velocity.y < 0)
        {
            velocity.x += jumpVelocity.x;
            velocity.y = jumpVelocity.y;
        }
        else
        {
            velocity += jumpVelocity;
        }
    }

    void Grip()
    {
        if (OnGripable)
        {
            rb.velocity = Vector2.zero;
            velocity = Vector2.zero;
            isGriping = true;
            desiredGrip = false;
            transform.SetParent(gripableTransform);
            ToggleGravity(false);
        }
    }
    
    private void ExitGrip()
    {
        transform.SetParent(null);
        isGriping = false;
        OnGripable = false;
        ToggleGravity(true);
    }
    /*
    void GripRope()
    {
        if (OnGripableRope)
        {
            rb.velocity = Vector2.zero;
            velocity = Vector2.zero;
            isGripingRope = true;
            desiredGripRope = false;
            hj.enabled = true;
            hj.connectedBody = gripableRopeTransform.GetComponent<Rigidbody2D>();
            //transform.SetParent(gripableRopeParentTransform);
            ToggleGravity(false);
        }
    }*/
    /*
    void ExitGripRope()
    {
        //transform.SetParent(null);
        hj.connectedBody = null;
        hj.enabled = false;
        isGripingRope = false;
        ToggleGravity(true);
    }*/

    
    void Dash()
    {
        if (OnGround || OnSteep)
        {
            dashPhase = 0;
        }
        else if (maxAirDashes > 0 && dashPhase <= maxAirDashes)
        {
            if (dashPhase == 0)
            {
                dashPhase = 1;
            }
        }
        else if (isGriping)
        {
            ExitGrip();
        }
        else
        {
            return;
        }

        dashPhase += 1;
        
        if (canDash)
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        Vector2 initialVelocity = velocity;
        RestrictInputWhenDash = true;
        canDash = false;
        isDashing = true;
        // 대쉬 중에는 중력의 영향을 받지 않음
        ToggleGravity(false);

        if (OnSteep)
        {
            velocity = new Vector2(faceRight ? -1f : 1f, 0f).normalized * dashForce;
        }
        else
        {
            // 바라보는 방향으로 대쉬
            velocity = new Vector2(faceRight ? 1f : -1f, 0f).normalized * dashForce;
        }
          
        yield return new WaitForSeconds(dashMaintainTime);
        RestrictInputWhenDash = false;
        isDashing = false;
        rb.velocity = new Vector2(initialVelocity.x, 0f);
        ToggleGravity(true);
        yield return new WaitForSeconds(dashCoolDown - dashMaintainTime);
        canDash = true;
    }
    
    

    void ToggleGravity(bool toggle)
    {
        rb.gravityScale = toggle ? gravityScaler : 0f;
    }

#region Collision

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (characterLevel >= 3)
        {
            Time.timeScale = 0f;
            BS.gameObject.SetActive(true);
            endText.SetActive(true);
        }
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            rb.velocity = Vector2.zero;
            transform.SetParent(collision.transform);
        }
        EvaluateCollision(collision);
        if (isDashing)
        {
            StopCoroutine(DashCoroutine());
            RestrictInputWhenDash = false;
            isDashing = false;
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);

        if (OnSteep && characterLevel >= 1)
        {
            if (collision.rigidbody != null)
            {
                velocity += collision.rigidbody.velocity;
            }
        }
    }

    void EvaluateCollision(Collision2D collision)
    {
        // 사망 관련
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 대쉬 중인 경우, 적 처치
            if (isDashing)
            {
                //TODO: Restart시 적 복구 원할 시 리스트에 담아야함
                collision.gameObject.SetActive(false);
            }
            else
            {
                Respawn();
            }
        }
        if(collision.gameObject.CompareTag("DeadZone"))
        {
            Respawn();
        }
        if (collision.gameObject.CompareTag("Niddle"))
        {
            Respawn();
        }
        // 오브젝트 관련
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            isCollidedMushRoom = true;
        }
        
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;

            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount += 1;
                contactNormal += normal;
                connectedRb = collision.rigidbody;
            }
            else if (normal.y > -0.01f)
            {
                // 천장이 아닌 벽인 경우
                steepContactCount += 1;
                steepNormal += normal;
                if (groundContactCount == 0) {
                    connectedRb = collision.rigidbody;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            isCollidedMushRoom = false;
        }
    }

    #endregion

#region Trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
            Respawn();
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 대쉬 중인 경우, 적 처치
            if (isDashing)
            {
                //TODO: Restart시 적 복구 원할 시 리스트에 담아야함
                collision.gameObject.SetActive(false);
            }
            else
            {
                Respawn();
            }
        }
        if (collision.gameObject.CompareTag("Niddle"))
        {
            Respawn();
        }

        if (collision.gameObject.CompareTag("Vine"))
        {
            if (!isGriping)
            {
                gripableTransform = collision.transform;
            }
            OnGripable = true;
        }
        /*
        if (collision.gameObject.CompareTag("Rope"))
        {
            var collisionTransform = collision.transform;
            gripableRopeParentTransform = collisionTransform.parent;
            gripableRopeTransform = collisionTransform;
            //transform.SetParent(gripableRopeParentTransform);
            OnGripableRope = true;
        }*/
        if (collision.gameObject.CompareTag("MonkeyPowerUp"))
        {
            collision.gameObject.SetActive(false);
            //TODO: 2스테이지 보스 소환 -> 게임 메니저
            //TODO: 능력 해금 -> bool or Level 조절
            //finalEnemy.SetActive(true);
            gameManager.PlayDialogue(11);
            characterLevel = 1;
        }

        if (collision.gameObject.CompareTag("DashPowerUp"))
        {
            collision.gameObject.SetActive(false);
            //TODO: 2스테이지 보스 소환 -> 게임 메니저
            //TODO: 능력 해금 -> bool or Level 조절
            gameManager.PlayDialogue(21);
            gameManager.ActiveBoss();
            //finalEnemy.SetActive(true);
            characterLevel = 2;
        }

        if (collision.gameObject.CompareTag("End"))
        {
            gameManager.PlayDialogue(3);
            //끝
            characterLevel = 3;
        }

        // 스테이지 이동 문
        if (collision.gameObject.CompareTag("Door"))
        {
            var door = collision.gameObject.GetComponent<JungleDoor>();
            if (door.isActive)
            {
                targetPosition = door.targetPosition;
                fromTo = door.fromTo;
                onDoor = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 겹치는 트리거에 대한 처리 (붙잡고 있는 트리거가 아니면 무시하기)
        if (collision.gameObject.CompareTag("Vine") && collision.transform == gripableTransform)
        {
            ExitGrip();
            OnGripable = false;
        }
        /*
        if (collision.gameObject.CompareTag("Rope") && collision.transform.parent != gripableRopeParentTransform)
        {
            ExitGripRope();
            OnGripableRope = false;
        }*/
        if (collision.gameObject.CompareTag("Door"))
        {
            onDoor = false;
        }
    }

#endregion
    void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        
        velocity = rb.velocity;
        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1)
            {
                jumpPhase = 0;
                dashPhase = 0;
            }
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = Vector2.up;
        }
        if (connectedRb) {
            if (connectedRb.isKinematic || connectedRb.mass >= rb.mass) {
                UpdateConnectionState();
            }
        }
    }
    
    void UpdateConnectionState () {
        if (connectedRb == previousConnectedRb) {
            Vector3 connectionMovement =
                connectedRb.position - connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }
        connectionWorldPosition = connectedRb.position;
    }

    Vector2 ProjectOnContactPlane(Vector2 vector)
    {
        return vector - contactNormal * Vector2.Dot(vector, contactNormal);
    }

    void AdjustVelocity()
    {
        Vector2 xAxis = ProjectOnContactPlane(Vector2.right).normalized;
        
        Vector2 relativeVelocity = velocity - connectionVelocity;
        float currentX = Vector3.Dot(relativeVelocity, xAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        
        velocity += xAxis * (newX - currentX);
    }

    void MovePosition()
    {
        transform.position += new Vector3(0f, playerInput.y, 0f) * (Time.deltaTime * maxSpeed);
        var tempPosition = transform.position;
        tempPosition.x = gripableTransform.position.x;
        transform.position = tempPosition;
    }

    void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = connectionVelocity = Vector2.zero;
        previousConnectedRb = connectedRb;
        connectedRb = null;
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        var hit = Physics2D.Raycast(rb.position, Vector2.down, probeDistance, probeMask);
        if (hit.collider == null)
        {
            return false;
        }
        if (hit.normal.y < minGroundDotProduct)
        {
            return false;
        }
        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector2.Dot(velocity, hit.normal);
        if (dot > 0f && !isDashing && !isGriping)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }
        connectedRb = hit.rigidbody;
        return true;
    }

    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }
}
