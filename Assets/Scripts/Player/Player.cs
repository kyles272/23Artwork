using System;
using Unity.VisualScripting;
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

    public PlayerInput playerInput{ get; private set; }

    private RaycastHit hit;

    public Inventory inventory{get; private set;}

    public bool isCarrying { get; private set; } = false;

    public Transform carryPoint;

    [SerializeField] private float hitRange = 2f;

    HUD HUD;

    public void SetIsCarrying(bool result)
    {
        isCarrying = result;
    }

    public RaycastHit GetRaycastHit()
    {
        return hit;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();

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

    public void OnInteract()
    {
        Debug.Log("Interact button pressed");
        if (hit.collider!= null && hit.collider.gameObject.GetComponent<Interactable>() != null)
        {
            // Call the Interact method on the Interactable component
            hit.collider.GetComponent<Interactable>().Interact(this);
        }
    }

    public void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += OnLook;
        //playerInput.actions["Scroll"].performed += inventory.CycleItems;

        playerInput.actions["Interact"].performed += ctx => OnInteract();
    }


    void Start()
    {
        HUD = GameObject.Find("UI").transform.Find("HUD").GetComponent<HUD>();
        if (HUD == null)
        {
            Debug.LogError("HUD not found");
        }
        Cursor.lockState = CursorLockMode.Locked;
        _camera = GameObject.Find("camera");
        rb = GetComponent<Rigidbody>();

        // Disable Rigidbody rotation so manual rotation doesn't conflict
        rb.freezeRotation = true;

        // Initialize camera rotation
        _camera.transform.localRotation = Quaternion.Euler(0, 0, 0);

        hit = new RaycastHit();

        //Connect to Inventory System
        inventory = FindAnyObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory System not found in Hierarchy");
        }

        //Intialize carry point
        carryPoint = new GameObject("CarryPoint").transform;
        carryPoint.SetParent(_camera.transform);
        carryPoint.localPosition = new Vector3(0, 0, 2f);
    }

    void Update()
    {
        // Draw a ray for debugging purposes
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hitRange, Color.red);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, hitRange))
        {
            //Check if the object has an Interactable component, show UI prompt to tell the player they can interact.
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                HUD.ShowInteractPrompt(true);
                //Debug.Log("Press 'E' to interact");
            }
        }
        else
        {
            HUD.ShowInteractPrompt(false);
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
