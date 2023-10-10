using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Vector3 position;
    public float damage;
    private Vector3 oldPosition;
    public TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.position;
        oldPosition = this.transform.position;
        trailRenderer = transform.Find("BulletTrail").GetComponent<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        trailRenderer.enabled = true;
        position = this.transform.position;
        Vector3 velocity = (position - oldPosition) / Time.deltaTime;
        trailRenderer.widthMultiplier = damage/50;


        oldPosition = position;
    }
}
