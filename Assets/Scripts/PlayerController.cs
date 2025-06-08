using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject bulletPrefab;
    public GameObject flashlight;
    public LayerMask groundMask;

    private const float MoveSpeed = 10f;
    private const float JumpForce = 6f;
    private const float GroundCheckDistance = 1.1f;
    private const float MouseSensitivity = 600f;

    private GameManager _gameManager;
    private Rigidbody _rb;
    private bool _isGrounded;
    private bool _flashlightOn;
    private float _xRotation;

    // Start -------------------------------------------------------------------------------------

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _flashlightOn = true;
    }

    // Updates -----------------------------------------------------------------------------------

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
#else
        if (Input.GetKeyDown(KeyCode.Escape))
#endif
        {
            _gameManager.TogglePause();
            if (_gameManager.IsPaused())
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        if (_gameManager.IsPaused()) return;

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        _isGrounded = Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance, groundMask);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1))
        {
            _flashlightOn = !_flashlightOn;
            flashlight.SetActive(_flashlightOn);
        }
    }

    private void FixedUpdate()
    {
        if (_gameManager.IsPaused()) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * MoveSpeed;
        Vector3 newVelocity = new Vector3(move.x, _rb.velocity.y, move.z);
        _rb.velocity = newVelocity;
    }

    // Actions -----------------------------------------------------------------------------------

    private void Shoot()
    {
        Instantiate(
            bulletPrefab,
            cameraTransform.position + cameraTransform.forward * 1.5f,
            cameraTransform.rotation * Quaternion.Euler(90f, 0f, 0f)
        );
    }
    
    private void Pause()
    {
        _rb.isKinematic = true;
    }

    private void Resume()
    {
        _rb.isKinematic = false;
    }
}