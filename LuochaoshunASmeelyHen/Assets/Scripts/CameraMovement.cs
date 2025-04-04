using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed = 0.3f;

    public CharacterController cameraControler;
    void Update()
    {
        float x, z;
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x,z,0);
        transform.Translate(movement * cameraSpeed);

    }
}
