using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Timer destroyTimer;
    [SerializeField] private GameObject prefabExplosion;
    
    Vector3 position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyTimer = gameObject.AddComponent<Timer>();
        destroyTimer.Duration = 2f;
        destroyTimer.Run();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTimer.Finished)
        {
            Destroy(gameObject);
        }
    }

    // destroy asteroid
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
            GameObject explosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    //screen wrap
    private void OnBecameInvisible()
    {
        position = transform.position;
        float bulletHalfLength = GetComponent<CapsuleCollider2D>().size.x / 2;
        if (position.x - bulletHalfLength  < ScreenUtils.ScreenLeft || position.x + bulletHalfLength > ScreenUtils.ScreenRight)
        {
            transform.position = new Vector3(-position.x, position.y, position.z);
        }

        if (position.y + bulletHalfLength > ScreenUtils.ScreenTop || position.y - bulletHalfLength < ScreenUtils.ScreenBottom)
        {
            transform.position = new Vector3(position.x, -position.y, position.z);
        }

        if ((position.x - bulletHalfLength < ScreenUtils.ScreenLeft || position.x + bulletHalfLength > ScreenUtils.ScreenRight) &&
            (position.y + bulletHalfLength > ScreenUtils.ScreenTop || position.y - bulletHalfLength < ScreenUtils.ScreenBottom))
        {
            transform.position = new Vector3(-position.x, -position.y, position.z);
        }
        
    }
}
