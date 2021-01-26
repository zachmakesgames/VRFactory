using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSelection : MonoBehaviour
{
    public Camera fpsCamera;
    public List<Camera> securityCameras;
    public Camera flyovercam;

    private int cameraIndex;

    // Start is called before the first frame update
    void Start()
    {
        cameraIndex = 0;
        foreach(Camera cam in securityCameras)
        {
            cam.enabled = false;
        }
        //flyovercam.GetComponentInParent<Animator>().StopPlayback();
        flyovercam.enabled = true;
        fpsCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (securityCameras.Count > 0)
            {
                if (flyovercam.enabled)
                {
                    flyovercam.enabled = false;
                    securityCameras[cameraIndex].enabled = true;
                    cameraIndex += 1;
                    if (cameraIndex >= securityCameras.Count)
                    {
                        cameraIndex = 0;
                    }
                }
                else
                {
                    if (cameraIndex == 0)
                    {
                        securityCameras[securityCameras.Count - 1].enabled = false;
                    }
                    else
                    {
                        securityCameras[cameraIndex - 1].enabled = false;
                    }
                    flyovercam.enabled = true;
                }    
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (!flyovercam.enabled)
            {
                if (cameraIndex == 0)
                {
                    securityCameras[securityCameras.Count - 1].enabled = false;
                }
                else
                {
                    securityCameras[cameraIndex - 1].enabled = false;
                }
                securityCameras[cameraIndex].enabled = true;
                cameraIndex += 1;
                if (cameraIndex >= securityCameras.Count)
                {
                    cameraIndex = 0;
                }
            }
        }
    }
}
