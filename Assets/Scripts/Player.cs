using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 movementInput;

    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float viewSpeed = 2f;
    [SerializeField] bool isInverted = false;

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
        // Get mouse input for looking
        Vector2 lookInput = value.Get<Vector2>();
        xRotation = lookInput.x;
        yRotation = lookInput.y;
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
        // Movement
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        transform.Translate(moveDirection * movementSpeed * Time.deltaTime);

        // Horizontal rotation (Player body)
        transform.Rotate(Vector3.up * xRotation * viewSpeed * Time.deltaTime);

        // Vertical rotation (Camera)
        if (!isInverted)
        {
            currentXRotation -= yRotation * viewSpeed * Time.deltaTime;  // Inverted vertical rotation
        }
        else
        {
            currentXRotation += yRotation * viewSpeed * Time.deltaTime;  // Normal vertical rotation
        }

        // Clamp the vertical rotation to the desired limits
        currentXRotation = Mathf.Clamp(currentXRotation, minY, maxY);

        // Apply the clamped vertical rotation to the camera
        _camera.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);

        // Debug: Log the rotation values
        Debug.Log($"xRotation: {xRotation}, yRotation: {yRotation}, currentXRotation: {currentXRotation}");
    }
}
