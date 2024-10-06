using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/EnemyAI")]


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDir = 1f;
    //[SerializeField] private Transform player;
    private GameObject player;
    private State state;
    private EnemyPathFinding enemyPathfinding;
    private SpriteRenderer mySpriteRenderer;
    private Vector2 lastPosition;

    private enum State
    {
        //Roaming,
        TargetPlayer
    }

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


}
