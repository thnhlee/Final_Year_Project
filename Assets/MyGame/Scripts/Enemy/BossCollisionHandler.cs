using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("ThinhLe/BossCollisionHandler")]
public class BossCollisionHandler : MonoBehaviour
{
    private void Start()
    {
        // Get all colliders on this object
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            // Create a composite filter that ignores small enemies but maintains other collisions
            CompositeCollider2D compositeCollider = collider as CompositeCollider2D;
            if (compositeCollider != null)
            {
                compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
        }
    }
}
