using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneCable : MonoBehaviour
{
    public GameObject anchorTop;
    public GameObject anchorBottom;

    public float minScaleY;
    public float maxScaleY;

    public float minDistance;
    public float maxDistance;

    public float sx = 0.0005f;
    public float sz = 0.0005f;

    public bool printDistance = false;

    // Start is called before the first frame update
    void Start()
    {
        minDistance = Vector3.Distance(anchorTop.transform.position, anchorBottom.transform.position);
        transform.position = (anchorTop.transform.position + anchorBottom.transform.position) / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(anchorTop.transform.position, anchorBottom.transform.position);
        if (printDistance)
        {
            Debug.Log(d);
            printDistance = false;
        }
        transform.position = (anchorTop.transform.position + anchorBottom.transform.position) / 2.0f;
        //Debug.Log(maxDistance);

        float scalefactor = (d - minDistance) / (maxDistance - minDistance);
        float sy = (scalefactor * (maxScaleY - minScaleY)) + minScaleY;
        transform.localScale = new Vector3(sx, sy, sz);
    }
}
