using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpineResponseToFeet : MonoBehaviour
{
    public GameObject leftFootIsOnGroundObject;
    public GameObject rightFootIsOnGroundObject;
    public GameObject player;
    public PIDcontroller PIDcontroller;
    public float desiredFootForce;
    private float forceFromFeet;
    public float availableForce;
    public float desiredHeightAboveGround;
    public float heightAboveGround;
    // Start is called before the first frame update
    void Start()
    {
        this.PIDcontroller = new PIDcontroller();
        GameObject[] body_parts;
        body_parts = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in body_parts)
        {
            if (obj.name == "PlayerRagdoll")
            {
                player = obj;
            }
        }
        this.PIDcontroller.Start();
        this.desiredHeightAboveGround = 3f;
        this.heightAboveGround = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerEntity>().Alive == false)
        {
            Debug.Log("player's dead");
            return;
        }

        GameObject[] body_parts;
        body_parts = GameObject.FindGameObjectsWithTag("Player");

        Vector3 LeftFootPosition = this.transform.position;
        Vector3 RightFootPosition = this.transform.position;
        Vector3 MyPosition = this.transform.position;
        Vector3 ForceDirection = new Vector3(0.0f, 1.0f, 0.0f);
        Rigidbody MyRigidBody = this.GetComponent<Rigidbody>();
        float AverageFootDistance;


        float FootForceMagnitude = 600.0f; //scales the available force for standing; the strength of the feet
        float MaxFootForce = 300.0f; //scales the PID's upper and lower bounds


        float RightFootForce = FootForceMagnitude;
        float LeftFootForce = FootForceMagnitude;


        // find the feet, their locations and set their force to 0 if they aren't on the ground
        foreach (GameObject obj in body_parts)
        {
            if (obj.name == "LeftFoot")
            {
                bool isOnGround = leftFootIsOnGroundObject.GetComponent<LeftFootIsOnGround>().onGround;
                LeftFootPosition = obj.transform.position;
                if (isOnGround == true)
                {
                    LeftFootForce = FootForceMagnitude;
                }
                else
                {
                    LeftFootForce = 0.0f;
                }
            }
            if (obj.name == "RightFoot")
            {
                bool isOnGround = rightFootIsOnGroundObject.GetComponent<RightFootIsOnGround>().onGround;
                RightFootPosition = obj.transform.position;
                if (isOnGround == true)
                {
                    RightFootForce = FootForceMagnitude;
                }
                else
                {
                    RightFootForce = 0.0f;
                }
            }
        }

        // find our height above the ground
        Ray heightRay = new Ray(this.transform.position, Vector3.down);
        float raycastLength = 20.0f;
        if (Physics.Raycast(heightRay, out RaycastHit hit, raycastLength))
        {
            this.heightAboveGround = transform.position.y - hit.point.y;
        }
        else
        {
            this.heightAboveGround = 20.0f;
        }


        // foot force is inversely proportional to the distance between the feet and the head
        AverageFootDistance = Math.Abs(MyPosition[1] - LeftFootPosition[1]) + Math.Abs(MyPosition[1] - RightFootPosition[1]);
        if (LeftFootForce + RightFootForce == 100.0f)
        {
            this.availableForce = ((LeftFootForce + RightFootForce * 1.5f) / 2) * (1 / Math.Max(AverageFootDistance * 2, 0.5f));
        }
        else
        {                                   // average force between the feet, multiplied by 1/distance, which can't go below 0.5m
            this.availableForce = ((LeftFootForce + RightFootForce) / 2) * (1 / Math.Max(AverageFootDistance * 2, 0.5f));
        }

        //update the PID to work out how much force we want to use
        PIDcontroller.desired = this.desiredHeightAboveGround;
        PIDcontroller.actual = this.heightAboveGround;
        PIDcontroller.Compute();
        this.desiredFootForce = PIDcontroller.response * MaxFootForce;
        this.forceFromFeet = Math.Max(Math.Min(desiredFootForce, this.availableForce), this.availableForce * -1);
        MyRigidBody.AddForce(ForceDirection * this.forceFromFeet);
    }
}
