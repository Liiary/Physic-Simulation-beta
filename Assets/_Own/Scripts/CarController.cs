﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [System.Serializable]
    public struct AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool Motor;
        public bool Steering;
    }

    public List<AxleInfo> AxleInfos;
    public float MotorTorque;
    public float SteeringAngle;
    public float BrakeForce;
    public float HandBrakeForce;
    public Transform CenterOfMass;
    public Transform SpawnPoint;
    public GameObject PauseMenuCanvas;

    public static bool PauseGame;

    private void Update()
    {
        GetComponent<Rigidbody>().centerOfMass = CenterOfMass.localPosition;
    }

    public void FixedUpdate()
    {
        float motor = MotorTorque * Input.GetAxis("Vertical");
        float steering = SteeringAngle * Input.GetAxis("Horizontal");

        foreach(AxleInfo axleInfo in AxleInfos)
        {
            if (axleInfo.Steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.Motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                axleInfo.leftWheel.brakeTorque = HandBrakeForce;
                axleInfo.rightWheel.brakeTorque = HandBrakeForce;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }

            if(Input.GetAxis("Vertical") == 0)
            {
                axleInfo.leftWheel.brakeTorque = BrakeForce;
                axleInfo.rightWheel.brakeTorque = BrakeForce;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }

            ApplyPositionToVisuals(axleInfo.leftWheel);
            ApplyPositionToVisuals(axleInfo.rightWheel);

            if (Input.GetKeyDown(KeyCode.R))
            {
                this.transform.position = SpawnPoint.position;
                this.transform.rotation = SpawnPoint.rotation;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame = true;
                PauseMenuCanvas.gameObject.SetActive(true);
                this.GetComponent<AudioSource>().enabled = false;
                this.GetComponent<CarController>().enabled = false;
                Time.timeScale = 0;
            }
        }
    }

    public void ApplyPositionToVisuals(WheelCollider wCollider)
    {
        if(wCollider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = wCollider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        wCollider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
