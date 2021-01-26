using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlight : MonoBehaviour
{
    public float lookspeed = 15.0f;
    public float movespeed = 5.0f;
    private float rotx = 0.0f;
    private float roty = 0.0f;

    private Camera cam;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.isActiveAndEnabled)
        {
            if (Input.GetKey(KeyCode.KeypadMinus))
            {
                movespeed -= 0.1f;
                if (movespeed <= 0.0f)
                {
                    movespeed = 0.1f;
                }
            }
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                movespeed += 0.1f;
                if (movespeed >= 5.0f)
                {
                    movespeed = 5.0f;
                }
            }

            rotx += Input.GetAxis("Mouse X") * lookspeed;
            roty += Input.GetAxis("Mouse Y") * lookspeed;
            roty = Mathf.Clamp(roty, -90.0f, 90.0f);

            transform.localRotation = Quaternion.AngleAxis(rotx, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(roty, Vector3.left);

            transform.position += transform.forward * movespeed * Input.GetAxis("Vertical");
            transform.position += transform.right * movespeed * Input.GetAxis("Horizontal");
        }
        
        
    }
}
