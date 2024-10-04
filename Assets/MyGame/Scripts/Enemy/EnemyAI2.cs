using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.AI;
[AddComponentMenu("ThinhLe/EnemyAI2")]

public class EnemyAI2 : MonoBehaviour
{
    public Seeker seeker;
    public bool roaming = true;
    public float moveSpeed = 3f;

    public float nextWayPointDistance;
    public bool updateContinuePath;
    bool reachDestination = false;

    //-----Enemy fire
    public bool isShootable = false;


    private SpriteRenderer mySpriteRenderer;
    private Vector2 lastPosition;


    Path path;
    Coroutine moveCoroutine;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating("CalculatePath", 0f, 0.5f);
        reachDestination = true;
        lastPosition = transform.position;
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
            //Enemy move around Player
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
            if(distance < nextWayPointDistance) 
            {
                currentWayPoint++;
            }

            yield return null;
        }

        reachDestination = true;
    }


}
