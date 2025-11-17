using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] Sprite asteroidMagenta;
    [SerializeField] Sprite asteroidWhite;
    [SerializeField] Sprite asteroidGreen;

    private float colliderRadius;
    
    
    /*
    private Vector2 min = new Vector2();
    private Vector2 max = new Vector2();
    private const int maxSpawnTries = 20;
    
    
    // min and max spawn locations 
    private const float border = 100;
    float minSpawnX;
    float minSpawnY;
    float maxSpawnX;
    float maxSpawnY;
    */
    
    Timer spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject asteroid = Instantiate(asteroidPrefab);
        colliderRadius = asteroid.GetComponent<CircleCollider2D>().radius;
        Destroy(asteroid);

        /*
        minSpawnX = border;
        minSpawnY = border;
        maxSpawnX = Screen.width - border;
        maxSpawnY = Screen.height - border;
        */
        
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = 6f;
        spawnTimer.Run();
        
        SpawnAsteroid(Direction.Up);
        SpawnAsteroid(Direction.Down);
        SpawnAsteroid(Direction.Left);
        SpawnAsteroid(Direction.Right);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer.Finished)
        {
            int direction = Random.Range(1, 3);
            switch (direction)
            {
                case 1:
                    SpawnAsteroid(Direction.Up);
                    SpawnAsteroid(Direction.Down);
                    break;
                case 2:
                    SpawnAsteroid(Direction.Left);
                    SpawnAsteroid(Direction.Right);
                    break;
            }
            spawnTimer.Run();
        }
    }

    /*
    void SetMinMax(Vector3 location)
    {
        min.x = location.x - colliderRadius;
        min.y = location.y - colliderRadius;
        max.x = location.x + colliderRadius;
        max.y = location.y + colliderRadius;
    }
    */

    void SpawnAsteroid(Direction moveDirection)
    {
        // instantiate
        GameObject asteroid = Instantiate(asteroidPrefab, Vector3.zero, Quaternion.identity);
        float directionAngle, speed;
        Vector2 directionVec;
        Rigidbody2D asteroidRb = asteroid.GetComponent<Rigidbody2D>();
        // give sprite
        int spriteIndex = Random.Range(1, 4);
        SpriteRenderer asteroidRenderer = asteroid.GetComponent<SpriteRenderer>();
        if (spriteIndex == 1) asteroidRenderer.sprite = asteroidMagenta;
        else if (spriteIndex == 2) asteroidRenderer.sprite = asteroidWhite;
        else if (spriteIndex == 3) asteroidRenderer.sprite = asteroidGreen;
        //check direction
        switch (moveDirection)
        {
            case Direction.Up:
                asteroid.transform.position = new Vector3(0, ScreenUtils.ScreenBottom - colliderRadius);
                // throw asteroid with a random velocity in a 30deg arc (base is 90 for up)
                directionAngle = Random.Range(75, 105) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                speed = Random.Range(0.5f, 1.5f);
                asteroidRb.AddForce(directionVec * speed, ForceMode2D.Impulse);
                break;
            case Direction.Down:
                asteroid.transform.position = new Vector3(0, ScreenUtils.ScreenTop + colliderRadius);
                // throw asteroid with a random velocity in a 30deg arc (base is 270 for down)
                directionAngle = Random.Range(255, 285) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                speed = Random.Range(0.5f, 1.5f);
                asteroidRb.AddForce(directionVec * speed, ForceMode2D.Impulse);
                break;
            case Direction.Left:
                asteroid.transform.position = new Vector3(ScreenUtils.ScreenRight + colliderRadius, 0);
                // throw asteroid with a random velocity in a 30deg arc (base is 180 for left)
                directionAngle = Random.Range(165, 195) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                speed = Random.Range(0.5f, 1.5f);
                asteroidRb.AddForce(directionVec * speed, ForceMode2D.Impulse);
                break;
            case Direction.Right:
                asteroid.transform.position = new Vector3(ScreenUtils.ScreenLeft - colliderRadius, 0);
                // throw asteroid with a random velocity in a 30deg arc (base is 0 for right)
                directionAngle = Random.Range(-15, 15) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                speed = Random.Range(0.5f, 1.5f);
                asteroidRb.AddForce(directionVec * speed, ForceMode2D.Impulse);
                break;
        }
    }
}
