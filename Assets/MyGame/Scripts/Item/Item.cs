using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
[AddComponentMenu("ThinhLe/Items")]

public class Item : MonoBehaviour
{
    private enum Items
    {
        Coin,
        Health,
        Potion
    }

    [SerializeField] private Items itemType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelerationRate = 0.2f;
    [SerializeField] private float moveSpeed = 3f;

    //Pop-up item
    [SerializeField] private AnimationCurve animeCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;

    private Rigidbody2D rb;
    private Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2f, 2f);
        float randomY = transform.position.y + Random.Range(-1f, 1f);

        Vector2 endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed/ popDuration;
            float heightT = animeCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            DectectItemType();
            Destroy(gameObject);
        }
    }

    private void DectectItemType()
    {
        switch (itemType)
        {
            case Items.Coin:
                CoinManager.Instance.UpdateCurrentCoin();
                break;
            case Items.Health:
                PlayerHealth.Instance.Healing();
                break;
            case Items.Potion:
                break;
        }
    }
}
