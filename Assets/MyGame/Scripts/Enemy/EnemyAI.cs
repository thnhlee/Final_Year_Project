using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/EnemyAI")]


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDir = 1f;
    //[SerializeField] private Transform player;
    private GameObject player;
    private enum State
    {
        //Roaming,
        TargetPlayer
    }
    private State state;
    private EnemyPathFinding enemyPathfinding;
    private SpriteRenderer mySpriteRenderer;
    private Vector2 lastPosition;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathFinding>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        state = State.TargetPlayer;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        //StartCoroutine(RoamingRoutine());
        StartCoroutine(TargetPlayerRoutine());

    }
    private void Update()
    {
        Flip();
    }

    ////Enemy roaming randomly
    //private IEnumerator RoamingRoutine()
    //{
    //    while (state == State.Roaming)
    //    {
    //        Vector2 roamPosition = GetRoamingPosition();
    //        enemyPathfinding.MoveTo(roamPosition);


    //        yield return new WaitForSeconds(roamChangeDir);
    //    }
    //}
    //private Vector2 GetRoamingPosition()
    //{
    //    return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    //}

    //Enemy target player
    private IEnumerator TargetPlayerRoutine()
    {
        while (state == State.TargetPlayer)
        {
            if (player != null)
            {
                Vector2 targetPosition = player.transform.position;
                enemyPathfinding.MoveTo(targetPosition);
                
            }

            yield return new WaitForSeconds(roamChangeDir);
        }
    }
    public void UpdateLastPosition(Vector2 newPosition)
    {
        lastPosition = newPosition;
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


}
