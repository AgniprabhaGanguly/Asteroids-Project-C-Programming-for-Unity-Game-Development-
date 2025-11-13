using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Ship : MonoBehaviour
{
    private Timer shootDelay;
    
    private Rigidbody2D shipRb;
    private InputAction thrustInput;
    private InputAction rotateLeft;
    private InputAction rotateRight;
    private InputAction brakeInput;
    private InputAction shootInput;

    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private float rate_of_fire;
    
    [SerializeField] float thrustForce = 3f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float brakeForce = 1.5f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float bulletVelocity = 20f;

    private CircleCollider2D _collider;
    private Vector3 position;
    private void Awake()
    {
        shipRb = gameObject.GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        shootDelay = gameObject.AddComponent<Timer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootDelay.Duration = rate_of_fire;
        shootDelay.Run();
        
        thrustInput = InputSystem.actions.FindAction("Thrust");
        thrustInput.Enable();
        brakeInput = InputSystem.actions.FindAction("Brake");
        brakeInput.Enable();
        rotateLeft = InputSystem.actions.FindAction("RotateLeft");
        rotateLeft.Enable();
        rotateRight = InputSystem.actions.FindAction("RotateRight");
        rotateRight.Enable();
        shootInput = InputSystem.actions.FindAction("Shoot");
        shootInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float rotationRadian = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rotationRadian), Mathf.Sin(rotationRadian));

        if (thrustInput.IsPressed())
        {
            if (shipRb.linearVelocity.magnitude < maxSpeed) shipRb.AddForce(direction * thrustForce * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (brakeInput.IsPressed())
        {
            if (shipRb.linearVelocity.magnitude > 0.1f)
            {
                shipRb.linearVelocity = shipRb.linearVelocity.normalized * (shipRb.linearVelocity.magnitude - brakeForce * Time.deltaTime);
            }
            else shipRb.linearVelocity = Vector2.zero;
        }

        if (rotateLeft.IsPressed())
        {
            transform.Rotate(0.0f, 0.0f, rotateSpeed * Time.deltaTime);
        }

        if (rotateRight.IsPressed())
        {
            transform.Rotate(0.0f, 0.0f, -rotateSpeed * Time.deltaTime);
        }

        if (shootInput.IsPressed())
        {
            if (shootDelay.Finished)
            {
                // Find position of bullet (depend on where ship is rotated - transform.right give gameobject's right takes into account the rotation object has made)
                Vector3 bulletPosition = transform.position + transform.right * (_collider.radius + 0.5f);
                
                //Instantiate bullet
                GameObject bullet = (GameObject)Instantiate(prefabBullet, bulletPosition, transform.rotation);
                
                //Add force to bullet
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(transform.right * bulletVelocity * Time.fixedDeltaTime, ForceMode2D.Impulse);
                shootDelay.Run();
            }
        }
    }

    private void OnBecameInvisible()
    {
        position = transform.position;
        float shipRadius = _collider.radius;
        if (position.x - shipRadius  < ScreenUtils.ScreenLeft || position.x + shipRadius > ScreenUtils.ScreenRight)
        {
            transform.position = new Vector3(-position.x, position.y, position.z);
        }

        if (position.y + shipRadius > ScreenUtils.ScreenTop || position.y - shipRadius < ScreenUtils.ScreenBottom)
        {
            transform.position = new Vector3(position.x, -position.y, position.z);
        }

        if ((position.x - shipRadius < ScreenUtils.ScreenLeft || position.x + shipRadius > ScreenUtils.ScreenRight) &&
            (position.y + shipRadius > ScreenUtils.ScreenTop || position.y - shipRadius < ScreenUtils.ScreenBottom))
        {
            transform.position = new Vector3(-position.x, -position.y, position.z);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}
