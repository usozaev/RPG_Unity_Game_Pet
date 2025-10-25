using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float sensetivityX;
    public float sensetivityY;
    public Transform orientation;
    public Transform playerBody;

    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
         Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X")*Time.deltaTime*sensetivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y")*Time.deltaTime*sensetivityY;

        //Unity handles rotations in a strange way
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // limits rotation to not go over 90 degrees up and down

        //
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0);
        

    }
}
