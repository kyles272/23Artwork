using System;
using Unity.VisualScripting;
using UnityEngine;

public class CarryInteractable : Interactable
{
    // When the player interacts with this object, they are able to translate it and rotate it
    Rigidbody rb;

    private bool isCarried = false;

    private Player player;

    private static float rotationSpeed = 7.5f; // Speed of rotation when carrying the object

    public FixedJoint carryJoint;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact(Player player)
    {
        if (PlayerState.instance.currentState == PlayerStateType.CarryingObject
            && !isCarried)
        {
            Debug.Log("Player is already carrying an object.");
            return;
        }
        if (PlayerState.instance.currentState == PlayerStateType.RotatingCarryObject)
        {
            Debug.Log("Player is already rotating a carried object.");
            return;
        }
        else if (isCarried)
        {
            Debug.Log("Player is dropping the carried object.");

            // Drop logic
            if(carryJoint != null)
            {
                Destroy(carryJoint); // Remove the joint
            }
            

            rb.useGravity = true;

            player.SetIsRotatingCarryObject(false);
            player.SetIsCarrying(false);
            isCarried = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            return;
        }

        // Pick up logic
        this.player = player;
        isCarried = true;
        player.SetIsCarrying(true);

        rb.useGravity = false;

        gameObject.layer = LayerMask.NameToLayer("Carry");

        // Initialize the carry joint
        carryJoint = gameObject.AddComponent<FixedJoint>();
        carryJoint.connectedBody = player.carryPoint.GetComponent<Rigidbody>();

        carryJoint.breakForce = Mathf.Infinity;
        carryJoint.breakTorque = Mathf.Infinity;

    }


    public void RotateCarryObject(Vector2 lookInput)
    {
        if (!player.ToggleRotation) return;

        lookInput = lookInput.normalized; // Normalize the input to prevent speed increase with larger input values

        Vector3 rotation = new Vector3(lookInput.y, lookInput.x, 0);
        transform.rotation *= Quaternion.Euler(rotation);
    }
}
