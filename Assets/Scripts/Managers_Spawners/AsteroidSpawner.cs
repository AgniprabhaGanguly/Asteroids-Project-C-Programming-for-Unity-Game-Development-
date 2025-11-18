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

    private float speedAsteroid;
    private float speedSmallAsteroid;
    
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
    [SerializeField] float timeToSpawnAsteroid;
    Timer difficultyTimer;
    [SerializeField] private float timeToIncreaseDifficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject asteroid = Instantiate(asteroidPrefab);
        colliderRadius = asteroid.GetComponent<CircleCollider2D>().radius;
        speedAsteroid = asteroid.GetComponent<Asteroid>().speedAsteroid;
        speedSmallAsteroid = asteroid.GetComponent<Asteroid>().speedSmallAsteroid;
        Destroy(asteroid);
        
        
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = timeToSpawnAsteroid;
        spawnTimer.Run();
        
        // every 30s ramp up difficulty by reducing spawn timer duration
        difficultyTimer = gameObject.AddComponent<Timer>();
        difficultyTimer.Duration = timeToIncreaseDifficulty;
        difficultyTimer.Run();
        
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
            if (difficultyTimer.Finished)
            {
                difficultyTimer.Run();
                
                // decrease time taken to spawn
                if (spawnTimer.Duration >= 2f)
                    spawnTimer.Duration -= 1f;
                
                // increase speed of asteroid
                speedAsteroid = speedAsteroid + 0.5f;
                speedSmallAsteroid = speedSmallAsteroid + 0.5f;
            }
            int direction = Random.Range(1, 5);
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
                case 3:
                    SpawnAsteroid(Direction.Up);
                    SpawnAsteroid(Direction.Right);
                    break;
                case 4:
                    SpawnAsteroid(Direction.Down);
                    SpawnAsteroid(Direction.Left);
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
        
        //update speed according to difficulty
        var asteroidComponent = asteroid.GetComponent<Asteroid>();
        asteroidComponent.speedAsteroid = speedAsteroid;
        asteroidComponent.speedSmallAsteroid = speedSmallAsteroid;
        
        float directionAngle;
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
                asteroidRb.AddForce(directionVec * speedAsteroid, ForceMode2D.Impulse);
                break;
            case Direction.Down:
                asteroid.transform.position = new Vector3(0, ScreenUtils.ScreenTop + colliderRadius);
                // throw asteroid with a random velocity in a 30deg arc (base is 270 for down)
                directionAngle = Random.Range(255, 285) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                asteroidRb.AddForce(directionVec * speedAsteroid, ForceMode2D.Impulse);
                break;
            case Direction.Left:
                asteroid.transform.position = new Vector3(ScreenUtils.ScreenRight + colliderRadius, 0);
                // throw asteroid with a random velocity in a 30deg arc (base is 180 for left)
                directionAngle = Random.Range(165, 195) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                asteroidRb.AddForce(directionVec * speedAsteroid, ForceMode2D.Impulse);
                break;
            case Direction.Right:
                asteroid.transform.position = new Vector3(ScreenUtils.ScreenLeft - colliderRadius, 0);
                // throw asteroid with a random velocity in a 30deg arc (base is 0 for right)
                directionAngle = Random.Range(-15, 15) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                asteroidRb.AddForce(directionVec * speedAsteroid, ForceMode2D.Impulse);
                break;
        }
    }
}
