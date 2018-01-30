using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{


    public float playerWalkingSpeed = 5f;
    public float playerRunningSpeed = 15f;
    public float jumpStrength = 5f;
    public float verticalRotationLimit = 80;

    float forwardMovement;
    float sidewaysMovement;

    float verticalVelocity;

    float verticalRotation = 0;
    CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Looking horizontal
        float horizontalRotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalRotation, 0);

        // Looking vertical
        verticalRotation -= Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Player Movement only if grounded
        if (cc.isGrounded)
        {
            // Player Movement - walking
            forwardMovement = Input.GetAxis("Vertical") * playerWalkingSpeed;
            sidewaysMovement = Input.GetAxis("Horizontal") * playerWalkingSpeed;

            // Player Movement - running
            if (Input.GetKey(KeyCode.LeftShift))
            {
                forwardMovement = Input.GetAxis("Vertical") * playerRunningSpeed;
                sidewaysMovement = Input.GetAxis("Horizontal") * playerRunningSpeed;
            }
        }

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        // Jump
        if (Input.GetButton("Jump") && cc.isGrounded)
        {
            verticalVelocity = jumpStrength;
        }

        Vector3 playerMovement = new Vector3(sidewaysMovement, verticalVelocity, forwardMovement);

        cc.Move(transform.rotation * playerMovement * Time.deltaTime);
    }
}
