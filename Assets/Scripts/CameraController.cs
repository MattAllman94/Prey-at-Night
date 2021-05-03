using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Prey    
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
        if (_GM.gameState == GameState.INGAME)
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
