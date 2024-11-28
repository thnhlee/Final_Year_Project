using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[AddComponentMenu("ThinhLe/EnemyAI2")]

public class EnemyAI2 : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashCooldown = 3f;


    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCD = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    public Seeker seeker;
    public Path path;
    public Coroutine moveCoroutine;
    private KnockBack knockBack;
    private SpriteRenderer mySpriteRenderer;

    //Dash
    private bool isDashing = false;
    private bool canDash = true;
    private float baseMoveSpeed;
    private float lastDashTime;


    private enum State
    {
        Roaming,
        Attacking,
        Dashing
    }
    private State state;


    public bool roaming = true;
    public float moveSpeed = 3f;
    public float nextWayPointDistance = 0.3f;
    public bool updateContinuePath;
    bool reachDestination = false;

    private bool canAttack = true;
    private Vector2 lastPosition;



    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
        state = State.Roaming;
    }

    private void Start()
    {
        InvokeRepeating("CalculatePath", 0f, 0.5f);
        reachDestination = true;

        baseMoveSpeed = moveSpeed;
        lastDashTime = -dashCooldown;
    }

    private void Update()
    {
        if (gameObject.CompareTag(""))
        {
            // Check if dash is available
            if (!canDash && Time.time >= lastDashTime + dashCooldown)
            {
                canDash = true;
            }

            // Randomly attempt to dash when in attack range
            if (state == State.Attacking && canDash && !isDashing && Random.value < 0.01f)
            {
                StartCoroutine(DashRoutine());
            }
        }

    }

    private void FixedUpdate()
    {
        MovementStateControl();
        if (knockBack.gettingKnockedBack) { return; }
    }


    private void MovementStateControl()
    {
        if (isDashing) return;
        switch (state)
        {
            default:
            case State.Roaming:
                FindTarget();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Dashing:
                break;
        }
    }


    //-----Flip the enemy to the correct direction
    private void Flip()
    {
        Vector2 currentPosition = transform.position;
        if (currentPosition.x < lastPosition.x)
        {
            mySpriteRenderer.flipX = true;
        }
        else if (currentPosition.x > lastPosition.x)
        {
            mySpriteRenderer.flipX = false;
        }

        lastPosition = currentPosition;
    }

    //-----Caculating path to the Player
    private void CalculatePath()
    {
        Vector2 target = FindTarget();

        if (seeker.IsDone() && (reachDestination || updateContinuePath))
        {
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p) 
    {
        if (p.error) return;
        path = p;
        MoveToTarget();
    }

    //-----Locate player's location
    Vector2 FindTarget()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (roaming == true)
        {
            //Draw a space for enemy to move around Player
            return (Vector2)playerPos + (Random.Range(10f, 10f) * new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized);
        }
        else
        {
            return playerPos;
        }
    }

    //-----Move to Player
    private void MoveToTarget()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());

    }

    IEnumerator MoveToTargetCoroutine()
    {
        int currentWayPoint = 0;
        reachDestination = false;

        while (currentWayPoint < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - (Vector2)transform.position).normalized;
            Vector2 force = direction * moveSpeed * Time.deltaTime;
            transform.position += (Vector3)force;

            Flip();

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);
            if (distance < nextWayPointDistance) 
            {
                currentWayPoint++;
            }

            yield return null;
        }

        reachDestination = true;
    }

    //Enemy attack
    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            if(enemyType != null && enemyType is IEnemy) 
            { 
                (enemyType as IEnemy).Attack(); 
            }

            if (stopMovingWhileAttacking)
            {
                StopMoving();
            }
            else
            {
                MoveToTarget();
            }

            StartCoroutine(AttackCDRoutine());
        }
       
    }

    private IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private void StopMoving()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        reachDestination = true;
    }


    private IEnumerator DashRoutine()
    {
        isDashing = true;
        canDash = false;
        lastDashTime = Time.time;
        state = State.Dashing;
        float BaseMoveSpeed = moveSpeed;
        moveSpeed *= dashSpeed;



        // Generate random direction
        float randomAngle = Random.Range(0f, 360f);
        Vector2 dashDirection = new Vector2(
            Mathf.Cos(randomAngle * Mathf.Deg2Rad),
            Mathf.Sin(randomAngle * Mathf.Deg2Rad)
        ).normalized;

        // Perform dash
        float dashTimer = 0;
        while (dashTimer < dashDuration)
        {
            if (!knockBack.gettingKnockedBack)
            {
                transform.position += (Vector3)(dashDirection * moveSpeed * Time.deltaTime);
            }
            dashTimer += Time.deltaTime;
            yield return null;
        }

        // End dash
        moveSpeed = BaseMoveSpeed;
        isDashing = false;


        // Return to appropriate state
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }
        else
        {
            state = State.Roaming;
        }
    }

}
