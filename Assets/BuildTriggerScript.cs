using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTriggerScript : MonoBehaviour
{

    public GameObject TargetObject;
    public string BuildItemTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == BuildItemTag)
        {
            Debug.Log("The build item has entered the trigger");
        }
    }
}
