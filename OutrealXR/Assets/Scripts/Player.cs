using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform lookAroundTransform;
    public float movespeed = 10f;
    public float sensitivity = 5f;
    public bool invert;

    CharacterController characterCotnroller;

    float yaw;
    float pitch;

    void Start()
    {
        characterCotnroller = GetComponent<CharacterController>();
    }

    void Update()
    {
        LookAround();
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = transform.TransformDirection(direction);
        direction *= movespeed * Time.deltaTime;

        characterCotnroller.Move(direction);
    }

    private void LookAround()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            yaw += Input.GetAxis("Mouse Y") * sensitivity * (invert ? -1 : 1);
            yaw = Mathf.Clamp(yaw, -90f, 90f);
            lookAroundTransform.rotation = Quaternion.Slerp(lookAroundTransform.rotation, Quaternion.Euler(yaw, lookAroundTransform.eulerAngles.y, lookAroundTransform.eulerAngles.z), 10f * Time.deltaTime);

            pitch += Input.GetAxis("Mouse X") * sensitivity;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, pitch, transform.eulerAngles.z), 10f * Time.deltaTime);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
