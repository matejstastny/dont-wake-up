/*
 * Author: Matěj Šťastný
 * Date created: 6/3/2025
 * GitHub link: https://github.com/matysta/dont-wake-up
 */

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    // Constants ----------------------------------------------------------------------------------------

    private const float MoveSpeed = 10f;
    private const float JumpForce = 6f;
    private const float GroundCheckDistance = 1.1f;
    private const float MouseSensitivity = 600f;
    private const float BulletSpawnDistance = 1.5f;
    private const float CameraVerticalClamp = 90f;
    
    // References ---------------------------------------------------------------------------------------
    
    [Header("References")]
    public Transform cameraTransform;
    public GameObject bulletPrefab;
    public GameObject flashlight;
    public LayerMask groundMask;


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

    // Collision ----------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            _gameManager.Heal();
        }
        Destroy(other.gameObject);
    }

    // Events -------------------------------------------------------------------------------------------

    public void ToggleMovement(bool movementEnabled)
    {
        _rb.isKinematic = !movementEnabled;
    }

    private void HandlePauseToggle()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
#else
        if (Input.GetKeyDown(KeyCode.Escape))
#endif
        {
            _gameManager.TogglePause(!_gameManager.IsPaused(), true);
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -CameraVerticalClamp, CameraVerticalClamp);
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
            Vector3 spawnPos = cameraTransform.position + cameraTransform.forward * BulletSpawnDistance;
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
}
