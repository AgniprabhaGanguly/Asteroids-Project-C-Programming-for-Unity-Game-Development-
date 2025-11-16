using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Ship : MonoBehaviour
{
    private Timer shootDelay;
    
    private Rigidbody2D shipRb;
    private InputAction moveForward;
    private InputAction strafeLeft;
    private InputAction strafeRight;
    private InputAction moveBackward;
    private InputAction shootInput;

    private GameObject reticleObj;
    [SerializeField] GameObject reticle;
    [SerializeField] float reticalDistance;

    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private float rate_of_fire;
    
    [SerializeField] float thrustForce = 3f;
    [SerializeField] private float brakeForce = 8f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float bulletVelocity = 20f;
    
    [SerializeField] private GameObject shipExplosion;
    [SerializeField] private AudioClip shipExplosionFX;
    [SerializeField] private AudioClip bulletFiringFX;
    
    private CircleCollider2D _collider;
    private Vector3 position;

    private bool keyIsPressed = false;

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
        
        Vector3 offset = transform.right * reticalDistance;
        reticleObj = Instantiate(reticle, transform.position + offset, Quaternion.identity);
        
        moveForward = InputSystem.actions.FindAction("forward");
        moveForward.Enable();
        moveBackward = InputSystem.actions.FindAction("backward");
        moveBackward.Enable();
        strafeLeft = InputSystem.actions.FindAction("left");
        strafeLeft.Enable();
        strafeRight = InputSystem.actions.FindAction("right");
        strafeRight.Enable();
        shootInput = InputSystem.actions.FindAction("Shoot");
        shootInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float rotationRadian = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        
        // Read inputs this frame
        bool forward = moveForward.IsPressed();
        bool backward = moveBackward.IsPressed();
        bool left = strafeLeft.IsPressed();
        bool right = strafeRight.IsPressed();
        
        bool isMovingInput = forward || backward; 

        // Thrust while held (continuous force → Force + deltaTime)
        if (forward)
        {
            shipRb.AddForce(transform.right * thrustForce * Time.deltaTime, ForceMode2D.Impulse);
            updateTriggerPos();
        }
        
        if (backward)
        {
            shipRb.AddForce(-transform.right * thrustForce * Time.deltaTime, ForceMode2D.Impulse);
            updateTriggerPos();
        }
        
        if (left)
        {
            transform.Rotate(0.0f, 0.0f, rotateSpeed * Time.deltaTime);
            updateTriggerPos();
        }

        if (right)
        {
            transform.Rotate(0.0f, 0.0f, -rotateSpeed * Time.deltaTime);
            updateTriggerPos();
        }

        // Auto‑brake when no movement input
        if (!isMovingInput)
        {
            Vector2 v = shipRb.linearVelocity;
            const float stopSpeed = 0.1f;
            if (v.sqrMagnitude > stopSpeed * stopSpeed)
            {
                shipRb.AddForce(-v.normalized * brakeForce * Time.deltaTime, ForceMode2D.Impulse);
            }
            else
            {
                shipRb.linearVelocity = Vector2.zero;
            }
        }

        // Cap speed after all forces for the frame
        if (shipRb.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            shipRb.linearVelocity = Vector2.ClampMagnitude(shipRb.linearVelocity, maxSpeed);

        if (shootInput.IsPressed())
        {
            if (shootDelay.Finished)
            {
                // Find position of bullet (depend on where ship is rotated - transform.right give gameobject's right takes into account the rotation object has made)
                Vector3 bulletPosition = transform.position + transform.right * (_collider.radius + 0.5f);
                
                //Instantiate bullet
                GameObject bullet = (GameObject)Instantiate(prefabBullet, bulletPosition, transform.rotation);
                AudioManager.Instance.PlaySoundFX(bulletFiringFX, transform, 0.3f);
                
                //Add force to bullet
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.AddForce(transform.right * bulletVelocity * Time.deltaTime, ForceMode2D.Impulse);
                shootDelay.Run();  
            }
        }
    }

    private void updateTriggerPos()
    {
        Vector3 offset = transform.right * reticalDistance;
        reticleObj.transform.position = transform.position + offset;
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
            GameTimer.Instance.StopStopwatch();
            Instantiate(shipExplosion, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySoundFX(shipExplosionFX, transform, 1f);
            Destroy(gameObject);
        }
        
    }
}
