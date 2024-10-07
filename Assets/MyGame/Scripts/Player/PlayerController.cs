using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[AddComponentMenu("ThinhLe/PlayerController")]


public class PlayerController : Singleton<PlayerController>
{
        
    public bool FacingLeft { get { return facingLeft; } }   

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private KnockBack knockBack;
    private float baseMoveSpeed;
    private Collider2D playerCollider;

    private bool facingLeft = false;
    private bool isDashing = false;
    private bool isInvulnerable = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        baseMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        PlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }

    private void PlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }


    //Player's movement
    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);

    }
 
    private void Move()
    {
        if (knockBack.gettingKnockedBack) 
        {
            return; 
        }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }


    //DASH
    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            Invulnerable(true);
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDash());
        }
    }

    private IEnumerator EndDash()
    {
        float dashTime = 1f;
        float dashCD = 0.25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = baseMoveSpeed;
        Invulnerable(false);
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    //Invulnerable while dashing
    private void Invulnerable(bool ignore)
    {
        Projectile[] bullets = FindObjectsOfType<Projectile>();

        foreach (Projectile bullet in bullets)
        {
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (bulletCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider, ignore);
            }
        }
    }



}
