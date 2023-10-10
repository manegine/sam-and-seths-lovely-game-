using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputHandler : MonoBehaviour
{
    public float desiredSpineOrientationX = 0;
    public float desiredSpineOrientationY = 0;
    public float desiredSpineOrientationZ = 0;
    public float orientationX = 0;
    public float orientationY = 0;
    public float oldOrientationY = 0;
    public float orientationZ = 0;
    public float turningOutput = 0;
    public float maxTurnStrength = 100;
    public float xSensitivity = 0.005f;
    public PIDcontroller turnToCameraPID;
    public float desiredTurnSpeed = 0;
    public float TurnSpeed = 0;
    private Rigidbody rb;
    private Vector2 lastMousePosition;
    private Vector2 mouseDelta;
    // Start is called before the first frame update
    void Start()
    {
        this.turnToCameraPID = new PIDcontroller();
        turnToCameraPID.Start();
        rb = GetComponent<Rigidbody>();
        lastMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        mouseDelta = mousePos - lastMousePosition;
        TurnSpeed = orientationY - oldOrientationY;

        orientationX = this.transform.forward.x;
        orientationY = this.transform.forward.y;
        orientationZ = this.transform.forward.z;

        TurnSpeed = orientationY - oldOrientationY;
        desiredTurnSpeed = 1 * mouseDelta.x;

        turnToCameraPID.desired = desiredTurnSpeed;
        turnToCameraPID.actual = TurnSpeed;
        turnToCameraPID.Compute();
        float output = turnToCameraPID.response;
        rb.AddTorque(transform.up * output * xSensitivity);

        lastMousePosition = mousePos;
        oldOrientationY = orientationY;


    }
}
