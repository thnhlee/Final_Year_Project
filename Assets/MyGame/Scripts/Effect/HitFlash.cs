using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/HitFlash")]

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Material hitFlashMat;
    [SerializeField] private float restoreDefaultMatTime = 0.1f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = hitFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }

    public float GetRestoreMatTime()
    {
        return restoreDefaultMatTime;
    }
}
