using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    static Vector3 GetCameraCenter()
    {
        Vector3 cameraCenter = Camera.main.transform.position;//初始化，同时给予一个默认值
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return cameraCenter = hit.point;
        }
        else
        {
            return cameraCenter;
        }

    }
    public float cameraSpeed = 0.3f;

    public CharacterController cameraControler;
    void Update()
    {
        float x, z;
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x,z,0);
        transform.Translate(movement * cameraSpeed);

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Vector3 cameraCenter = GetCameraCenter();
            transform.RotateAround(cameraCenter, Vector3.up, 90);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Vector3 cameraCenter = GetCameraCenter();
            transform.RotateAround(cameraCenter, Vector3.up, -90);
        }


    }
}
