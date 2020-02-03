using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float dist = 4.0f;

    public float rotateSpeedV = 2.0f;
    public float rotateSpeedH = 2.0f;

    float yaw;
    float pitch;

    private void Start()
    {
        yaw = 0.0f;
        pitch = 25.0f;
    }

    private void Awake()
    {
        transform.position = player.transform.position + new Vector3(0, 2.0f, -4.0f);
    }

    private void FixedUpdate()
    {
        yaw += rotateSpeedH * Input.GetAxis("Mouse X");
        pitch -= rotateSpeedV * Input.GetAxis("Mouse Y");

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -dist) + player.transform.position;
    }
}
