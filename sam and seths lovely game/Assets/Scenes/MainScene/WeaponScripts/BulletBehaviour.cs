using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Vector3 position;
    public float damage;
    private Vector3 oldPosition;
    public TrailRenderer trailRenderer;
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
        position = this.transform.position;
        oldPosition = this.transform.position;
        trailRenderer = transform.Find("BulletTrail").GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
        Destroy(parentTransform.gameObject, 10.0f);
        Destroy(gameObject, 10.0f); // destroy all bullets after 10 seconds
    }

    // Update is called once per frame
    void Update()
    {
        // render the bullet's trail
        trailRenderer.enabled = true;
        position = this.transform.position;
        Vector3 velocity = (position - oldPosition) / Time.deltaTime;
        trailRenderer.widthMultiplier = damage/50;
        oldPosition = position;

        // check if it hit something. If so destroy the bullet
        Ray bulletRay = new Ray(transform.position, transform.forward * -1);
        float raycastLength = 0.2f;
        if (Physics.Raycast(bulletRay, out RaycastHit hit, raycastLength))
        {
            Destroy(parentTransform.gameObject, 0.0f);
            Destroy(gameObject, 0.0f);
        }

    }
}
