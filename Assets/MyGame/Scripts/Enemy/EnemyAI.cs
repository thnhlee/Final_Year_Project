using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/EnemyAI")]


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDir = 2f;
    private enum State
    {
        Roaming
    }
    private State state;
    private EnemyPathFinding enemyPathfinding;
    private SpriteRenderer mySpriteRenderer;
    private Vector2 lastPosition;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathFinding>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        state = State.Roaming;
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }
    private void Update()
    {
        Flip();
    }
    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);


            yield return new WaitForSeconds(roamChangeDir);
        }
    }

    //Flip the enemy to the right direction
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

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
