using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[AddComponentMenu("ThinhLe/PlayerController")]


public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private GameObject UIStats;
    [SerializeField] private GameObject Resume;
    public InventoryManager InventoryBag;
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float dashSpeed = 2f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private KnockBack knockBack;

    private Collider2D playerCollider;
    private const int maxSpeedIncrease = 11;

    private bool facingLeft = false;
    private bool isDashing = false;
    AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();

        GameObject[] audioGameObjects = GameObject.FindGameObjectsWithTag("Audio");
        //Find the first GameObject with the AudioManager component
        audioManager = Array.Find(audioGameObjects, go => go.GetComponent<AudioManager>() != null)?.GetComponent<AudioManager>();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
		InventoryBag = GameObject.Find("UI").GetComponent<InventoryManager>();

		playerControls.Combat.Dash.performed += _ => Dash();
        playerControls.Combat.Stats.performed += _ => Stat();
        playerControls.PauseGame.Pause.performed += _ => OnClickResume();
        playerControls.Inventory.Bag.performed += _ => OpenInventory();

		Inventory.Instance.EquipStartingWeapon();
    }

    private void Stat()
    {
        UIStats.SetActive(!UIStats.activeSelf);
        if( Time.timeScale == 0 )
        { 
            Time.timeScale = 1;
        }
        else Time.timeScale = 0;      
    }

    private void OnClickResume()
    {
        Resume.SetActive(!Resume.activeSelf);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else Time.timeScale = 0;
    }

    //Open Inventory UI
    private void OpenInventory()
    {
        if (!InventoryBag.menuActivated)
        {
			Time.timeScale = 0;
			InventoryBag.menuActivated = true;
			InventoryBag.InventoryMenu.SetActive(true);
		}
        else
        {
			Time.timeScale = 1;
			InventoryBag.menuActivated = false;
			InventoryBag.InventoryMenu.SetActive(false);
		}
	}

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
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
        if (knockBack.gettingKnockedBack || PlayerHealth.Instance.isDead)
        {
            return;
        }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public void IncreaseMoveSpeed()
    {
        if (moveSpeed < maxSpeedIncrease)
        {
            moveSpeed += 1;
        }
        
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
        float dashTime = 0.25f;
        float dashCD = 0.5f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeed;
        Invulnerable(false);
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    //Invulnerable while dashing
    private void Invulnerable(bool ignore)
    {
        Projectile[] bullets = FindObjectsOfType<Projectile>();
        EnemyHealth enemy = FindObjectOfType<EnemyHealth>();

        foreach (Projectile bullet in bullets)
        {
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            if (bulletCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider, ignore);

            }
        }

        if (enemy != null)
        {
            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                Physics2D.IgnoreCollision(enemyCollider, playerCollider, ignore);

            }
        }
    }



}
