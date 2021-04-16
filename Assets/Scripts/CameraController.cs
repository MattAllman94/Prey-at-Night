using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float rotationSpeed = 1;
    public Transform player;
    private float mouseX, mouseY;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
