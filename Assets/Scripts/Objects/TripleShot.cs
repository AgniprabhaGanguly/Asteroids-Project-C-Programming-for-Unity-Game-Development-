using UnityEngine;

public class TripleShot : MonoBehaviour
{
    [field:SerializeField] public float TripleShotDuration { get; private set; } = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnBecameInvisible()
    {
        Vector3 position = transform.position;
        Vector3 tripleShotColliderSize = GetComponent<Collider2D>().bounds.extents;
        
        if (position.x - tripleShotColliderSize.x  < ScreenUtils.ScreenLeft || position.x + tripleShotColliderSize.x > ScreenUtils.ScreenRight)
        {
            transform.position = new Vector3(-position.x, position.y, position.z);
        }

        if (position.y + tripleShotColliderSize.y > ScreenUtils.ScreenTop || position.y - tripleShotColliderSize.y < ScreenUtils.ScreenBottom)
        {
            transform.position = new Vector3(position.x, -position.y, position.z);
        }

        if ((position.x - tripleShotColliderSize.x < ScreenUtils.ScreenLeft || position.x + tripleShotColliderSize.x > ScreenUtils.ScreenRight) &&
            (position.y + tripleShotColliderSize.y > ScreenUtils.ScreenTop || position.y - tripleShotColliderSize.y < ScreenUtils.ScreenBottom))
        {
            transform.position = new Vector3(-position.x, -position.y, position.z);
        }
    }
}
