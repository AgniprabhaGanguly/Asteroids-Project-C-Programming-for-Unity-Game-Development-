using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public class Bullet : MonoBehaviour
{
    Timer destroyTimer;
    [SerializeField] private GameObject prefabAsteroidExplosion;
    [SerializeField] private AudioClip asteroidExplosionFX;
    
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
            GameObject asteroid = other.gameObject;

            if (asteroid.transform.localScale.Equals(new Vector3(0.7f, 0.7f, 0.7f)))
            {
                Destroy(asteroid);
                GameObject explosion = Instantiate(prefabAsteroidExplosion, asteroid.transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySoundFX(asteroidExplosionFX, asteroid.transform, 1f);
                Destroy(gameObject); // this destroys bullet
            }
            else //split asteroid
            {
                GameObject ast1 = Instantiate(asteroid, asteroid.transform.position, Quaternion.identity);
                ast1.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                
                GameObject ast2 = Instantiate(asteroid, asteroid.transform.position, Quaternion.identity);
                ast2.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                
                Destroy(asteroid); // destroy asteroid
                Destroy(gameObject); // destroy bullet
                
                //add force to the new asteroids
                float directionAngle = Random.Range(0,360) * Mathf.Deg2Rad;
                Vector2 directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                
                ast1.GetComponent<Rigidbody2D>().AddForce(directionVec * 2, ForceMode2D.Impulse);
                
                directionAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                
                ast2.GetComponent<Rigidbody2D>().AddForce(directionVec * 2, ForceMode2D.Impulse);
            }
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
