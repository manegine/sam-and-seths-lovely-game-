using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class SpineResponseToFeet : MonoBehaviour
{
    public GameObject leftFootIsOnGroundObject;
    public GameObject rightFootIsOnGroundObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] body_parts;
        body_parts = GameObject.FindGameObjectsWithTag("Player");

        Vector3 LeftFootPosition = this.transform.position;
        Vector3 RightFootPosition = this.transform.position;
        Vector3 MyPosition = this.transform.position;
        Vector3 ForceDirection = new Vector3(0.0f, 1.0f, 0.0f);
        Rigidbody MyRigidBody = this.GetComponent<Rigidbody>();
        float AverageFootDistance;
        float FootForceMagnitude = 100.0f;
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
                    LeftFootForce = 100.0f;
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
                    RightFootForce = 100.0f;
                }
                else
                {
                    RightFootForce = 0.0f;
                }
            }
        }


        // foot force is inversely proportional to the distance between the feet and the head
        AverageFootDistance = Math.Abs(MyPosition[1] - LeftFootPosition[1]) + Math.Abs(MyPosition[1] - RightFootPosition[1]);
        MyRigidBody.AddForce(ForceDirection * (LeftFootForce+RightFootForce)/2 * 1/Math.Min(AverageFootDistance, 1));
    }
}
