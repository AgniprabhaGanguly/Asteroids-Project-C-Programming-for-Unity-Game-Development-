using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    Vector3 position;
    private CircleCollider2D astCollider;
    
    [SerializeField] private GameObject prefabAsteroidExplosion;
    [SerializeField] private AudioClip asteroidExplosionFX;
    
    [field:SerializeField] public float speedAsteroid { get; set; }
    [field:SerializeField] public float speedSmallAsteroid { get; set; }
    
    bool splitOnce = false;

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
    
    
    // destroy asteroid on contact with bullet once
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //add score
            if(gameObject.CompareTag("Asteroid")) ScoreScript.Instance.AddScore(10); // for big asteroids add 10 points
            else if(gameObject.CompareTag("S_Asteroid")) ScoreScript.Instance.AddScore(5); // for small add 5 points

            if (gameObject.CompareTag("S_Asteroid"))
            {
                Destroy(gameObject);
                GameObject explosion = Instantiate(prefabAsteroidExplosion, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySoundFX(asteroidExplosionFX, transform, 1f);
                Destroy(other.gameObject); // this destroys bullet
            }
            else if(!splitOnce && gameObject.CompareTag("Asteroid"))//split asteroid
            {
                GameObject ast1 = Instantiate(gameObject, transform.position, Quaternion.identity);
                ast1.tag = "S_Asteroid";
                ast1.transform.localScale = new Vector3(1f, 1f, 0f);
                
                GameObject ast2 = Instantiate(gameObject, transform.position, Quaternion.identity);
                ast2.tag = "S_Asteroid";
                ast2.transform.localScale = new Vector3(1f, 1f, 0f);
                
                Destroy(gameObject); // destroy asteroid
                Destroy(other.gameObject); // destroy bullet
                
                //add force to the new asteroids
                float directionAngle = Random.Range(0,360) * Mathf.Deg2Rad;
                Vector2 directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                
                ast1.GetComponent<Rigidbody2D>().AddForce(directionVec * speedSmallAsteroid, ForceMode2D.Impulse);
                
                directionAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                
                ast2.GetComponent<Rigidbody2D>().AddForce(directionVec * speedSmallAsteroid, ForceMode2D.Impulse);
                
                splitOnce = true;
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
