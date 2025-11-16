using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    Vector3 position;
    private CircleCollider2D astCollider;

    private bool spawnProtection = true;
    private float spawnProtectionTimer = 0.0f;
    private float spawnProtectionDuration = 1f;

    private void Awake()
    {
        astCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (spawnProtection)
        {
            spawnProtectionTimer += Time.deltaTime;
            if (spawnProtectionTimer >= spawnProtectionDuration)
            {
                spawnProtection = false;
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (spawnProtection) return;
        position = transform.position;
        float asteroidRadius = astCollider.radius;
        if (position.x - asteroidRadius  < ScreenUtils.ScreenLeft || position.x + asteroidRadius > ScreenUtils.ScreenRight)
        {
            transform.position = new Vector3(-position.x, position.y, position.z);
        }

        if (position.y + asteroidRadius > ScreenUtils.ScreenTop || position.y - asteroidRadius < ScreenUtils.ScreenBottom)
        {
            transform.position = new Vector3(position.x, -position.y, position.z);
        }

        if ((position.x - asteroidRadius < ScreenUtils.ScreenLeft || position.x + asteroidRadius > ScreenUtils.ScreenRight) &&
            (position.y + asteroidRadius > ScreenUtils.ScreenTop || position.y - asteroidRadius < ScreenUtils.ScreenBottom))
        {
            transform.position = new Vector3(-position.x, -position.y, position.z);
        }
    }
}
