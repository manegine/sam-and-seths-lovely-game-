using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class PIDcontroller
{
    public float Kp;
    public float Ki;
    public float Kd;
    public float desired;
    public float actual;
    public float response;
    private float integral;
    private float previousError;

    public void Start()
    {
        this.Kp = 0.05f;
        this.Ki = 0.0002f;
        this.Kd = 0.0004f;
        this.previousError = 0.0f;
        this.integral = 0.0f;
        this.desired = 0.0f;
        this.actual = 0.0f;
    }


    public void Compute()
    {
        float error = this.desired - this.actual;
        this.integral += error * Time.deltaTime;
        float derivative = (error - this.previousError) / Time.deltaTime;
        //Debug.Log(error * this.Kp);
        //Debug.Log(derivative * this.Kd);
        //Debug.Log(this.integral * this.Ki);
        this.response = (error * this.Kp) + (derivative * this.Kd) + (this.integral * this.Ki);
        this.response = Mathf.Max(Mathf.Min(this.response, 1), -1);
    }
}
