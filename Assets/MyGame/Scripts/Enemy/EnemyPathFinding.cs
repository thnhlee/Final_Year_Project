using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/EnemyPathFinding")]

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private KnockBack knockBack;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockBack.gettingKnockedBack) { return; }
        rb.MovePosition(rb.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));


        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }


        GetComponent<EnemyAI>().UpdateLastPosition(rb.position);
    }

    public void MoveTo(Vector2 targetPosition)
    {
        //Enemy roaming random
        //moveDirection = targetPosition;

        //Enemy target the player 
        Vector2 currentPosition = rb.position;
        moveDirection = (targetPosition - currentPosition).normalized;
    }



}
