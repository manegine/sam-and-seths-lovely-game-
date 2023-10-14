using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputHandler : MonoBehaviour
{
    public float xSensitivity = 1f;
    public float playerCameraDirection = 0.0f;
    public Vector3 targetDirection = Vector3.zero;
    public Vector2 mousePos = Vector2.zero;
    public float rotationSpeed = 45f;
    public PIDcontroller turnToCameraPID;
    private Rigidbody rb;
    public Quaternion targetRotation;
    public Quaternion currentRotation;
    public Quaternion deltaRotation;
    // Start is called before the first frame update
    void Start()
    {
        this.turnToCameraPID = new PIDcontroller();
        turnToCameraPID.Start();
        rb = GetComponent<Rigidbody>();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Update the target direction with both pitch and yaw changes.
        targetDirection += new Vector3(0f, mouseDelta.x * xSensitivity * Time.deltaTime, 0f);

        // Use Quaternion.Lerp to smoothly interpolate between current and target rotation.
        targetRotation = Quaternion.Euler(targetDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
