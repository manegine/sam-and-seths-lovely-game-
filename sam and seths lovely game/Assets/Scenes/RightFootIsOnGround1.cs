using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightFootIsOnGround : MonoBehaviour
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
        Boolean onGround = false;
        Vector3 Position = this.transform.position;
        float raycastLength = 0.1f;
        Vector3 raycastOrigin = Position + Vector3.up * 0.1f;
        Ray collisionRay = new Ray(raycastOrigin, Vector3.down);
        if (Physics.Raycast(collisionRay, out RaycastHit hit, raycastLength))
        {
            this.onGround = true;
        }
    }
}
