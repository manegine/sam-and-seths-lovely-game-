using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFootIsOnGround : MonoBehaviour
{
    public bool onGround;
    // Start is called before the first frame update
    void Start()
    {
        this.onGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.onGround = false;
        Vector3 Position = this.transform.position;
        Vector3 Direction = Vector3.down;
        float raycastLength = 0.2f;
        Vector3 raycastOrigin = Position;
        Ray collisionRay = new Ray(raycastOrigin, Direction);

        //cast the ray and check if it hits a collidable object
        if (Physics.Raycast(collisionRay, out RaycastHit hit, raycastLength))
        {
            this.onGround = true;
        }
    }
}
