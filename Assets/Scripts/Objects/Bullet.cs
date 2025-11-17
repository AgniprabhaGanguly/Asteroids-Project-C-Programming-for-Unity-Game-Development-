using UnityEngine;

public class Bullet : MonoBehaviour
{
    Timer destroyTimer;
    
    Vector3 position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyTimer = gameObject.AddComponent<Timer>();
        destroyTimer.Duration = 0.5f;
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
