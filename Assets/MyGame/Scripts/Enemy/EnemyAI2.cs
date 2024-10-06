using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[AddComponentMenu("ThinhLe/EnemyAI2")]

public class EnemyAI2 : MonoBehaviour
{
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCD = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    public Seeker seeker;
    public Path path;
    public Coroutine moveCoroutine;
    private KnockBack knockBack;
    private SpriteRenderer mySpriteRenderer;


    private enum State
    {
        Roaming,
        Attacking
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
    }

    private void FixedUpdate()
    {
        MovementStateControl();
        if (knockBack.gettingKnockedBack) { return; }
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                FindTarget();
                break;
            case State.Attacking:
                Attacking();
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
            (enemyType as IEnemy).Attack();

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


}
