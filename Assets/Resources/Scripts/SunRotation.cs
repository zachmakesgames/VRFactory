using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public float dayLength;

    // Start is called before the first frame update
    void Start()
    {
        dayLength = 500.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.unscaledTime % dayLength;
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(360.0f * t, -10.0f, 0.0f);
        transform.rotation = q;
    }
}
