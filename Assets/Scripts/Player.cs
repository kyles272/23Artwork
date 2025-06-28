using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 movementInput;

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] bool isInverted = false;

    [SerializeField] private float cameraSpeed = 100f; // Speed of camera rotation

    GameObject _camera;
    private float xRotation = 0f;
    private float yRotation = 0f;

    Rigidbody rb;

    // Clamp angles for vertical look (Y-axis rotation)
    [SerializeField] private float minY = -40f;  // Min vertical rotation angle
    [SerializeField] private float maxY = 40f;   // Max vertical rotation angle

    private float currentXRotation = 0f;  // Track current pitch (vertical rotation)

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        Vector2 lookInput = value.Get<Vector2>();

        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            // Analog stick sensitivity multiplier
            xRotation = lookInput.x * 100f; // Tweak multiplier as needed
            yRotation = lookInput.y * 100f;
        }
        else
        {
            // Mouse input (already in delta)
            xRotation = lookInput.x;
            yRotation = lookInput.y;
        }
    }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera = GameObject.Find("camera");
        rb = GetComponent<Rigidbody>();

        // Disable Rigidbody rotation so manual rotation doesn't conflict
        rb.freezeRotation = true;
    }

    void Update()
    {
        //Check what is in front of the player
        RaycastHit hit;
        // Draw a ray for debugging purposes
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * 2f, Color.red);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 2f))
        {
            // If something is hit, log the name of the object
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
    }

    void FixedUpdate()
    {
        // Movement
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);
        // Horizontal rotation (Player body)
        transform.Rotate(Vector3.up * xRotation * Time.deltaTime);

        // Vertical rotation (Camera)
        if (!isInverted)
        {
            currentXRotation -= yRotation * Time.deltaTime;  // Inverted vertical rotation
        }
        else
        {
            currentXRotation += yRotation * Time.deltaTime;  // Normal vertical rotation
        }

        // Clamp the vertical rotation to the desired limits
        currentXRotation = Mathf.Clamp(currentXRotation, minY, maxY);

        // Apply the clamped vertical rotation to the camera
        _camera.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
    }
}
