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
        xRotation = value.Get<Vector2>().x;
        yRotation = value.Get<Vector2>().y;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera = GameObject.Find("camera");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movement
        transform.Translate(new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed * Time.deltaTime);

        // Horizontal rotation (Player body)
        transform.Rotate(Vector3.up * xRotation * viewSpeed * Time.deltaTime);

        // Vertical rotation (Camera)
        if (!isInverted)
        {
            currentXRotation -= yRotation * viewSpeed * Time.deltaTime;  // Invert vertical rotation
        }
        else
        {
            currentXRotation += yRotation * viewSpeed * Time.deltaTime;  // Normal vertical rotation
        }

        // Clamp the vertical rotation to the desired limits
        currentXRotation = Mathf.Clamp(currentXRotation, minY, maxY);

        // Apply the clamped vertical rotation to the camera
        _camera.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
    }
}
