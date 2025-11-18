using UnityEngine;
using Random = UnityEngine.Random;
using System;
public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tripleShotPrefab;
    [SerializeField] private int tripleShotSpawnRate;
    
    private float currentScore;

    private Timer tripleShotTimer;
    private bool tsIsSpawnedOnce; // prevent triple shot powerups spawning multiple times

    private Vector3 tripleShotColliderSize;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject tripleShot = Instantiate(tripleShotPrefab, Vector3.zero, Quaternion.identity);
        tripleShotColliderSize = tripleShot.GetComponent<BoxCollider2D>().bounds.extents;
        Destroy(tripleShot);
        
        tsIsSpawnedOnce = true;
        tripleShotTimer = gameObject.AddComponent<Timer>();
        tripleShotTimer.Duration = tripleShotSpawnRate;
        tripleShotTimer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tsIsSpawnedOnce)
        {
            spawnTripleShot();
            tsIsSpawnedOnce = true;
        }
        if (tripleShotTimer.Finished)
        {
            tsIsSpawnedOnce = false;
            tripleShotTimer.Run();
        }
    }

    void spawnTripleShot()
    {
        GameObject tripleShot = Instantiate(tripleShotPrefab, Vector3.zero, Quaternion.identity);
        //randomize up down left right
        int spawnDirectionIndex = Random.Range(0, 4);
        Direction spawnDirection = (Direction)Enum.GetValues(typeof(Direction)).GetValue(spawnDirectionIndex);

        tripleShotColliderSize = tripleShot.GetComponent<BoxCollider2D>().bounds.extents;
        Rigidbody2D rbTripleShot = tripleShot.GetComponent<Rigidbody2D>();

        float directionAngle;
        Vector2 directionVec;
        switch (spawnDirection)
        {
            case Direction.Up:
                tripleShot.transform.position = new Vector3(0, ScreenUtils.ScreenBottom - tripleShotColliderSize.y);
                directionAngle = Random.Range(75, 105) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                rbTripleShot.AddForce(directionVec * 2, ForceMode2D.Impulse);
                break;
            case Direction.Down:
                tripleShot.transform.position = new Vector3(0, ScreenUtils.ScreenTop + tripleShotColliderSize.y);
                directionAngle = Random.Range(255, 285) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                rbTripleShot.AddForce(directionVec * 2, ForceMode2D.Impulse);
                break;
            case Direction.Left:
                tripleShot.transform.position = new Vector3(ScreenUtils.ScreenRight + tripleShotColliderSize.x,0);
                directionAngle = Random.Range(165, 195) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                rbTripleShot.AddForce(directionVec * 2, ForceMode2D.Impulse);
                break;
            case Direction.Right:
                tripleShot.transform.position = new Vector3(ScreenUtils.ScreenLeft - tripleShotColliderSize.x, 0);
                directionAngle = Random.Range(-15, 15) * Mathf.Deg2Rad;
                directionVec = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
                rbTripleShot.AddForce(directionVec * 2, ForceMode2D.Impulse);
                break;
        }
    }
}
