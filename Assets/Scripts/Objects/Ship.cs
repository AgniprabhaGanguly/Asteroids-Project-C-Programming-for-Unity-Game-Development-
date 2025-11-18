using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class Ship : MonoBehaviour
{
    private Timer shootDelay;
    
    private Rigidbody2D shipRb;
    private InputAction moveForward;
    private InputAction strafeLeft;
    private InputAction strafeRight;
    private InputAction moveBackward;
    private InputAction shootInput;
    private InputAction tripleShotInput;
    
    
    //RETICLE
    private GameObject reticleObj;
    [SerializeField] GameObject reticle;
    [SerializeField] float reticalDistance;

    //BULLET
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private float rate_of_fire;
    
    // SHIP PHYSICS
    [SerializeField] float thrustForce = 3f;
    [SerializeField] private float brakeForce = 8f;
    [SerializeField] float maxSpeed = 4f;
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] float bulletVelocity = 20f;
    
    // AUDIO & EFFECTS
    [SerializeField] private GameObject shipExplosion;
    [SerializeField] private AudioClip shipExplosionFX;
    [SerializeField] private AudioClip bulletFiringFX;
    
    //POWERUPS
    bool tripleShotActivated = false;
    Timer tripleShotTimer;
    float tripleShotDuration;
    public TextMeshProUGUI tripleShotCount;
    
    private CircleCollider2D _collider;
    private Vector3 position;
    
    private void Awake()
    {
        shipRb = gameObject.GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        shootDelay = gameObject.AddComponent<Timer>();
        tripleShotTimer = gameObject.AddComponent<Timer>();
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
        tripleShotInput = InputSystem.actions.FindAction("tripleShot");
        tripleShotInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // after collision ship does not keep rotating due to the angular velocity exerted
        shipRb.angularVelocity = 0f;
        
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
                AudioManager.Instance.PlaySoundFX(bulletFiringFX, transform, 0.3f);
                 ShootBullet();
                 if (tripleShotActivated)
                 {
                     ShootBullet(-15);
                     ShootBullet(15);
                 }
                 shootDelay.Run();
            }
        }

        if (tripleShotInput.WasPressedThisFrame())
        {
            if (int.Parse(tripleShotCount.text.ToString()) > 0)
            {
                tripleShotActivated = true;
                tripleShotCount.text = (int.Parse(tripleShotCount.text) - 1).ToString();
                tripleShotTimer.Duration = tripleShotDuration;
                tripleShotTimer.Run();
            }
        }

        if (tripleShotTimer.Finished) tripleShotActivated = false;
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
        if (other.gameObject.CompareTag("Asteroid"))
        {
            HealthScript.Instance.TakeDamage(50);
            if (HealthScript.Instance.Health <= 0)
            {
                GameTimer.Instance.StopStopwatch();
                Instantiate(shipExplosion, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySoundFX(shipExplosionFX, transform, 1f);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("S_Asteroid"))
        {
            HealthScript.Instance.TakeDamage(25);
            if (HealthScript.Instance.Health <= 0)
            {
                GameTimer.Instance.StopStopwatch();
                Instantiate(shipExplosion, transform.position, Quaternion.identity);
                AudioManager.Instance.PlaySoundFX(shipExplosionFX, transform, 1f);
                Destroy(gameObject);
            }
        }
    }
    
    //for powerups
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TripleShot"))
        {
            tripleShotDuration = other.GetComponent<TripleShot>().TripleShotDuration;
            Destroy(other.gameObject);

            tripleShotCount.text = (int.Parse(tripleShotCount.text) + 1).ToString();
        }
    }

    private void ShootBullet(float angleDeg = 0)
    {
        
        // Find position of bullet (depend on where ship is rotated - transform.right give gameobject's right takes into account the rotation object has made)
        Vector3 bulletPosition = transform.position + transform.right * _collider.radius;
                
        //Instantiate bullet
        GameObject bullet = Instantiate(prefabBullet, bulletPosition, transform.rotation);
                
        //Add force to bullet at an angle
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        
        // Quaternion.AngleAxis rotates angle (angleDeg) about axis (Vector3.forward ie z axis) with base vector (transform.right)
        Vector2 directionVec = Quaternion.AngleAxis(angleDeg, Vector3.forward) * transform.right;
        bulletRb.AddForce(directionVec * bulletVelocity * Time.deltaTime, ForceMode2D.Impulse);
    }
}
