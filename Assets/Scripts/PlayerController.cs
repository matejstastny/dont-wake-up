/*
 * Author: Matěj Šťastný
 * Date created: 6/3/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public GameObject bulletPrefab;
    public GameObject flashlight;
    public LayerMask groundMask;

    [Header("Constants")]
    private const float MoveSpeed = 10f;
    private const float JumpForce = 6f;
    private const float GroundCheckDistance = 1.1f;
    private const float MouseSensitivity = 600f;

    [Header("Private")]
    private GameManager _gameManager;
    private Rigidbody _rb;
    private float _xRotation;
    private bool _isGrounded;
    private bool _flashlightOn = true;

    // Start --------------------------------------------------------------------------------------------
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager")?.GetComponent<GameManager>();
        flashlight.SetActive(_flashlightOn);
    }

    // Update -------------------------------------------------------------------------------------------
    
    private void Update()
    {
        HandlePauseToggle();
        if (_gameManager.IsPaused()) return;

        HandleMouseLook();
        HandleGroundCheck();
        HandleJump();
        HandleShoot();
        HandleFlashlightToggle();
    }

    private void FixedUpdate()
    {
        if (_gameManager.IsPaused()) return;
        HandleMovement();
    }
    
    // Events -------------------------------------------------------------------------------------------

    private void HandlePauseToggle()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
#else
        if (Input.GetKeyDown(KeyCode.Escape))
#endif
        {
            _gameManager.TogglePause();

            if (_gameManager.IsPaused())
                Pause();
            else
                Resume();
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void HandleGroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance, groundMask);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void HandleShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * 1.5f;
            Quaternion spawnRot = cameraTransform.rotation * Quaternion.Euler(90f, 0f, 0f);
            Instantiate(bulletPrefab, spawnPos, spawnRot);
        }
    }

    private void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1))
        {
            _flashlightOn = !_flashlightOn;
            flashlight.SetActive(_flashlightOn);
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * MoveSpeed;
        _rb.velocity = new Vector3(move.x, _rb.velocity.y, move.z);
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