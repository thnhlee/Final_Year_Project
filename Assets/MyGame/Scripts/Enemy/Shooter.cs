using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[AddComponentMenu("ThinhLe/Shooter")]

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBtwBurst;
    [SerializeField] private float reload;

    private bool isShooting = false;

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        for (int i = 0; i < burstCount; i++) 
        {
            Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.transform.right = targetDirection;

            if (newBullet.TryGetComponent(out Projectile projectile))
            {
                projectile.UpdateMoveSpeed(bulletMoveSpeed);
            }

            yield return new WaitForSeconds(timeBtwBurst);

        }

        yield return new WaitForSeconds(reload);
        isShooting = false;
    }
}